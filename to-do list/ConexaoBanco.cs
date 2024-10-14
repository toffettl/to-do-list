using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace to_do_list
{
    static class ConexaoBanco
    {
        private const string servidor = "localhost";
        private const string bancoDados = "dbtodolist";
        private const string usuario = "root";
        private const string senha = "Dinossauro1.";

        static public string bancoServidor = $"server={servidor}; user id ={usuario}; database={bancoDados}; password={senha}";
    }
}
