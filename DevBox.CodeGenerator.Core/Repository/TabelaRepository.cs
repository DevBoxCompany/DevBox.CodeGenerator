using DevBox.CodeGenerator.Core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DevBox.CodeGenerator.Core.Repository
{
    public class TabelaRepository
    {
        private readonly SqlConnection _sqlConnection;
        public TabelaRepository()
        {
            const string conn = @"Server=MUSSAK-PC\SQLEXPRESS;Database=BancoTeste;Trusted_Connection=True;MultipleActiveResultSets=true";
            _sqlConnection = new SqlConnection(conn);
        }

        //Usuario
        public List<CampoTabela> BuscarCamposPorNomeTabela(string nomeTabela)
        {
            var lstCampos = new List<CampoTabela>();
            var commandText = $@"SELECT COLUMN_NAME,
                                        DATA_TYPE,
                                        CHARACTER_MAXIMUM_LENGTH,
                                        NUMERIC_PRECISION,
                                        NUMERIC_SCALE,
                                        CAST(CASE WHEN IS_NULLABLE = 'NO' THEN 0 ELSE 1 END AS bit) AS IS_NULLABLE,
                                        CAST(COLUMNPROPERTY(OBJECT_ID('{nomeTabela}'), COLUMN_NAME, 'IsIdentity') AS bit) AS IS_IDENTITY,
                                        CAST( (
                                           CASE WHEN COLUMN_NAME = (SELECT INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME
                                                  FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                                                  WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1
                                                  AND TABLE_NAME = '{nomeTabela}'
                                                 ) THEN 1 ELSE 0 END

                                        ) AS bit)  AS IS_PK
                                    FROM INFORMATION_SCHEMA.COLUMNS
                                    WHERE TABLE_NAME = '{nomeTabela}'";

            _sqlConnection.Open();
            using (var command = new SqlCommand(commandText, _sqlConnection))
            using (var reader = command.ExecuteReader())
                while (reader.Read())
                    lstCampos.Add(new CampoTabela
                    {
                        NomeColuna = reader.GetString(reader.GetOrdinal("COLUMN_NAME")),
                        TipoColuna = reader.GetString(reader.GetOrdinal("DATA_TYPE")),
                        Nulo = reader.GetBoolean(reader.GetOrdinal("IS_NULLABLE")),
                        ChavePrimaria = reader.GetBoolean(reader.GetOrdinal("IS_PK")),
                        AutoIncremento = reader.GetBoolean(reader.GetOrdinal("IS_IDENTITY")),
                        QuantidadeMaximaCaracteres = reader.IsDBNull(reader.GetOrdinal("CHARACTER_MAXIMUM_LENGTH"))
                            ? default(int?)
                            : reader.GetInt32(reader.GetOrdinal("CHARACTER_MAXIMUM_LENGTH")),
                        PrecisaoNumero = reader.IsDBNull(reader.GetOrdinal("NUMERIC_PRECISION"))
                            ? default(byte?)
                            : reader.GetByte(reader.GetOrdinal("NUMERIC_PRECISION")),
                        EscalaNumero = reader.IsDBNull(reader.GetOrdinal("NUMERIC_SCALE"))
                            ? default(int?)
                            : reader.GetInt32(reader.GetOrdinal("NUMERIC_SCALE"))
                    });

            return lstCampos;
        }
    }
}
