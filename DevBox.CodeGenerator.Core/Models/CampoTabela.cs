namespace DevBox.CodeGenerator.Core.Models
{
    public class CampoTabela
    {
        public int Id { get; set; }
        public string NomeColuna { get; set; }
        public string TipoColuna { get; set; }
        public bool Nulo { get; set; }
        public bool ChavePrimaria { get; set; }
        public bool AutoIncremento { get; set; }

        public int? QuantidadeMaximaCaracteres { get; set; }
        public byte? PrecisaoNumero { get; set; }
        public int? EscalaNumero { get; set; }

        public string Alias { get; set; }
        public bool Post { get; set; }
        public bool Put { get; set; }
        public bool Delete { get; set; }
        public bool Get { get; set; }
        public bool GetId { get; set; }

        private string ComplementoDeclaracaoSql()
        {
            if (TipoColuna == "decimal")
                return $"({PrecisaoNumero}, {EscalaNumero})";

            if (TipoColuna == "varchar")
                return $"({QuantidadeMaximaCaracteres})";

            return string.Empty;
        }

        private string ComplementoNulo()
        {
            return Nulo ? " = NULL" : string.Empty;
        }

        public string NomeDeclaracaoSqlSemNulos()
        {
            return $"{NomeColuna} {TipoColuna}{ComplementoDeclaracaoSql()}";
        }

        public string NomeDeclaracaoSql()
        {
            return $"@{NomeDeclaracaoSqlSemNulos()}{ComplementoNulo()}";
        }

        public string Comparacao(string apelido)
        {
            return TipoColuna == "varchar" 
                ? $"{apelido}.{NomeColuna} LIKE '%' + @{NomeColuna} + '%'"
                : $"{apelido}.{NomeColuna} = @{NomeColuna}";
        }
    }
}
