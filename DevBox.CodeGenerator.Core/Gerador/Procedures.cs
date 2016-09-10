using DevBox.CodeGenerator.Core.Models;
using System;
using System.Linq;
using System.Text;

namespace DevBox.CodeGenerator.Core.Gerador
{
    public class Procedures
    {
        private readonly Tabela _tabela;

        public Procedures(Tabela tabela)
        {
            _tabela = tabela;
        }

        private string Cabecalho(Operacao operacao)
        {
            return $@"
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_{operacao}{_tabela.NomeTabela}]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_{operacao}{_tabela.NomeTabela}]
GO

CREATE PROCEDURE [dbo].[SP_{operacao}{_tabela.NomeTabela}]";
        }

        public string Cadastrar()
        {
            string template = Cabecalho(Operacao.Inserir);

            foreach(var item in _tabela.CamposInsert())
            {
                template += $@"
    {item.NomeDeclaracaoSql()},";
            }
            template = template.TrimEnd(',');

            template += $@"
	
	AS

	/*
	Documentação
	Arquivo Fonte.....: {_tabela.NomeTabela}.sql
	Objetivo..........: Inserir um registro na tabela {_tabela.NomeTabela}
	Autor.............: NomeAutor
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Inserir{_tabela.NomeTabela}]
	*/

	BEGIN

		INSERT INTO [dbo].[{_tabela.NomeTabela}]
        (";
            foreach(var item in _tabela.CamposInsert())
            {
                template += $@"
            {item.NomeColuna},";
            }
            template = template.TrimEnd(',');
            template += $@"
        ) 
        VALUES 
        (";
            foreach (var item in _tabela.CamposInsert())
            {
                template += $@"
            @{item.NomeColuna},";
            }
            template = template.TrimEnd(',');
            template += $@"
        )

		RETURN {(_tabela.PossuiAutoIncremento() ? "SCOPE_IDENTITY()" : "0")}

	END
GO";
            return template;
        }

        public string Editar()
        {
            string template = Cabecalho(Operacao.Atualizar);

            foreach (var item in _tabela.CamposUpdate())
            {
                template += $@"
    {item.NomeDeclaracaoSql()},";
            }
            template = template.TrimEnd(',');

            template += $@"
	
	AS

	/*
	Documentação
	Arquivo Fonte.....: {_tabela.NomeTabela}.sql
	Objetivo..........: Atualizar um registro na tabela {_tabela.NomeTabela}
	Autor.............: NomeAutor
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Inserir{_tabela.NomeTabela}]
	*/

	BEGIN

		UPDATE [dbo].[{_tabela.NomeTabela}]
            SET ";
            foreach (var item in _tabela.CamposUpdate().Where(x => !x.ChavePrimaria))
            {
                template += $@"{item.NomeColuna} = @{item.NomeColuna},{Environment.NewLine}{"\t\t\t\t"}";
            }
            template = template.TrimEnd().TrimEnd(',');
            template += $@"
            WHERE {_tabela.ChavePrimaria().NomeColuna} = @{_tabela.ChavePrimaria().NomeColuna}
	END
GO";
            return template;
        }

        public string Excluir()
        {
            return null;
        }

        public string Selecionar()
        {
            return null;
        }

        public string BuscarPorId()
        {
            return null;
        }

        public string GerarCompleto()
        {
            var sb = new StringBuilder();
            sb.Append(Cadastrar())
              .Append(Environment.NewLine)
              .Append(Editar())
              .Append(Environment.NewLine)
              .Append(Excluir())
              .Append(Environment.NewLine)
              .Append(Selecionar())
              .Append(Environment.NewLine)
              .Append(BuscarPorId());

            return sb.ToString();
        }

        private enum Operacao
        {
            Inserir,
            Atualizar,
            Excluir,
            Selecionar,
            Buscar
        }
    }
}
