using System.Collections.Generic;
using System.Linq;

namespace DevBox.CodeGenerator.Core.Models
{
    public class Tabela
    {
        public Tabela(string nomeTabela)
        {
            NomeTabela = nomeTabela;
        }

        public string NomeTabela { get; set; }

        public IEnumerable<CampoTabela> CamposTabela { get; set; }

        public IEnumerable<CampoTabela> CamposInsert()
        {
            return CamposTabela.Where(x => !x.AutoIncremento);
        }

        public bool PossuiAutoIncremento()
        {
            return CamposTabela.Any(x => x.AutoIncremento);
        }
    }
}
