namespace DevBox.CodeGenerator.Core.Models
{
    public class CampoTabela
    {
        public string NomeColuna { get; set; }
        public string TipoColuna { get; set; }
        public bool Nulo { get; set; }
        public bool ChavePrimaria { get; set; }
        public bool AutoIncremento { get; set; }

        public int? QuantidadeMaximaCaracteres { get; set; }
        public byte? PrecisaoNumero { get; set; }
        public int? EscalaNumero { get; set; }

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

        public string NomeDeclaracaoSql()
        {
            return $"@{NomeColuna} {TipoColuna}{ComplementoDeclaracaoSql()}{ComplementoNulo()}";
        }
    }
}
