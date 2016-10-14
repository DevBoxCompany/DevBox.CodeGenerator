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

        private string _nomeTabela;
        public string NomeTabela
        {
            get
            {
                if (_nomeTabela.IndexOf('_') > 0)
                {
                    return _nomeTabela.Split('_')[1];
                }

                return _nomeTabela;
            }
            set { _nomeTabela = value; }
        }

        public string NomeTabelaCompleta => _nomeTabela;

        public IEnumerable<CampoTabela> CamposTabela { get; set; }

        public IEnumerable<CampoTabela> CamposInsert()
        {
            return CamposTabela.Where(x => !x.AutoIncremento && x.Post);
        }

        public IEnumerable<CampoTabela> CamposUpdate()
        {
            return CamposTabela.Where(x => x.Put);
        }

        public IEnumerable<CampoTabela> CamposDelete()
        {
            return CamposTabela.Where(x => x.Delete);
        }

        public IEnumerable<CampoTabela> CamposSelect()
        {
            return CamposTabela.Where(x => x.Get);
        }

        public IEnumerable<CampoTabela> CamposSelectId()
        {
            return CamposTabela.Where(x => x.GetId);
        }

        public CampoTabela ChavePrimaria()
        {
            return CamposTabela.First(x => x.ChavePrimaria);
        }

        public bool PossuiAutoIncremento()
        {
            return CamposTabela.Any(x => x.AutoIncremento);
        }

        public string Apelido()
        {
            return NomeTabela[0].ToString().ToLower();
        }
    }
}
