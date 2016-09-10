using DevBox.CodeGenerator.Core.Models;
using System;
using System.Linq;
using System.Text;

namespace DevBox.CodeGenerator.Core.Gerador
{
    public class Procedures
    {
        private readonly Tabela _tabela;
        public string Autor { get; set; }

        public Procedures(Tabela tabela, string autor)
        {
            _tabela = tabela;
            Autor = string.IsNullOrEmpty(autor) ? "NomeAutor" : autor;
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
            if (!_tabela.CamposInsert().Any())
                return string.Empty;

            var template = Cabecalho(Operacao.Inserir);

            foreach (var item in _tabela.CamposInsert())
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
	Autor.............: {Autor}
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Inserir{_tabela.NomeTabela}]
	*/

	BEGIN

		INSERT INTO [dbo].[{_tabela.NomeTabela}]
        (";
            foreach (var item in _tabela.CamposInsert())
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
            if (!_tabela.CamposUpdate().Any())
                return string.Empty;

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
	Autor.............: {Autor}
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Atualizar{_tabela.NomeTabela}]
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
            if (!_tabela.CamposDelete().Any())
                return string.Empty;

            string template = Cabecalho(Operacao.Excluir);

            template += $@"
    {_tabela.ChavePrimaria().NomeDeclaracaoSql()}";

            template += $@"
	
	AS

	/*
	Documentação
	Arquivo Fonte.....: {_tabela.NomeTabela}.sql
	Objetivo..........: Excluir um registro na tabela {_tabela.NomeTabela}
	Autor.............: {Autor}
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Excluir{_tabela.NomeTabela}]
	*/

	BEGIN

		DELETE [dbo].[{_tabela.NomeTabela}]
            WHERE {_tabela.ChavePrimaria().NomeColuna} = @{_tabela.ChavePrimaria().NomeColuna}
            
	END
GO";
            return template;
        }

        public string Selecionar()
        {
            if (!_tabela.CamposSelect().Any())
                return string.Empty;

            string template = Cabecalho(Operacao.Selecionar);

            foreach (var item in _tabela.CamposSelect().Where(x => !x.ChavePrimaria))
            {
                template += $@"
    {item.NomeDeclaracaoSql()} = NULL,";
            }
            template = template.TrimEnd(',');

            template += $@"
	
	AS

	/*
	Documentação
	Arquivo Fonte.....: {_tabela.NomeTabela}.sql
	Objetivo..........: Selecionar registros na tabela {_tabela.NomeTabela}
	Autor.............: {Autor}
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Selecionar{_tabela.NomeTabela}]
	*/

	BEGIN

		SELECT  ";
            foreach (var item in _tabela.CamposSelect())
            {
                template += $@"{_tabela.Apelido()}.{item.NomeColuna}{(string.IsNullOrEmpty(item.Alias) ? string.Empty : " AS " + item.Alias)},{Environment.NewLine}{"\t\t\t\t"}";
            }

            template = template.TrimEnd().TrimEnd(',');

            template += $@"
            FROM [dbo].[{_tabela.NomeTabela}] {_tabela.Apelido()} WITH(NOLOCK){Environment.NewLine}{"\t\t\t\t"} ";
            var cont = 0;
            foreach (var item in _tabela.CamposSelect().Where(x => !x.ChavePrimaria))
            {
                if (cont == 0)
                    template += $@"WHERE (@{item.NomeColuna} IS NULL OR {item.NomeColuna} = @{item.NomeColuna}){Environment.NewLine}{"\t\t\t\t"} ";
                else
                    template += $@"AND (@{item.NomeColuna} IS NULL OR {item.NomeColuna} = @{item.NomeColuna}){Environment.NewLine}{"\t\t\t\t"} ";
                cont++;
            }

            template += $@"
	END
GO";
            return template;
        }

        public string BuscarPorId()
        {
            if (!_tabela.CamposSelectId().Any())
                return string.Empty;

            string template = Cabecalho(Operacao.Buscar);

            template += $@"
    {_tabela.ChavePrimaria().NomeDeclaracaoSql()}";

            template += $@"
	
	AS

	/*
	Documentação
	Arquivo Fonte.....: {_tabela.NomeTabela}.sql
	Objetivo..........: Burcar registros na tabela {_tabela.NomeTabela} por Id
	Autor.............: {Autor}
 	Data..............: {DateTime.Today.ToShortDateString()}
	Ex................: EXEC [dbo].[SP_Buscar{_tabela.NomeTabela}]
	*/

	BEGIN

		SELECT  ";
            foreach (var item in _tabela.CamposSelectId())
            {
                template += $@"{_tabela.Apelido()}.{item.NomeColuna}{(string.IsNullOrEmpty(item.Alias) ? string.Empty : " AS " + item.Alias)},{Environment.NewLine}{"\t\t\t\t"}";
            }

            template = template.TrimEnd().TrimEnd(',');

            template += $@"
            FROM [dbo].[{_tabela.NomeTabela}] {_tabela.Apelido()} WITH(NOLOCK)
            WHERE {_tabela.Apelido()}.{_tabela.ChavePrimaria().NomeColuna} = @{_tabela.ChavePrimaria().NomeColuna}
	END
GO";
            return template;
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
