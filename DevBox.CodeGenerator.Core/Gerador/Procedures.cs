using DevBox.CodeGenerator.Core.Models;
using System;

namespace DevBox.CodeGenerator.Core.Gerador
{
    public class Procedures
    {
        private readonly Tabela _tabela;

        public Procedures(Tabela tabela)
        {
            _tabela = tabela;
        }

        public string Cadastrar()
        {

            string template = $@"
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[SP_Inserir{_tabela.NomeTabela}]') AND objectproperty(id, N'IsPROCEDURE')=1)
	DROP PROCEDURE [dbo].[SP_Inserir{_tabela.NomeTabela}]
GO

CREATE PROCEDURE [dbo].[SP_Inserir{_tabela.NomeTabela}]";

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
	Arquivo Fonte.....: NomeArquivo.sql
	Objetivo..........: Objetivo
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
        ) VALUES 
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

    }
}
