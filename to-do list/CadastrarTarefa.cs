using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace to_do_list
{
    internal class CadastrarTarefa
    {
        private int id;
        private string nome;
        private int completa;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Nome
        {
            get { return nome; }
            set { nome = value; }
        }
        public int Completa
        {
            get { return completa; }
            set { completa = value; }
        }

        //cadastrar funcionario no banco
        public bool cadastrarTarefa()
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string insert = $" insert into tarefas (id, nome, completa) values ('{Id}','{Nome}', '{Completa}')";

                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = insert;

                comandoSql.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método cadastrarTarefa" + ex.Message);
                return false;
            }
        }

        public MySqlDataReader localizarTarefa(int numeroTarefas)
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string select = $"select id, nome from tarefas where id = '{numeroTarefas}';";

                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = select;

                MySqlDataReader reader = comandoSql.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método localizarTarefa: " + ex.Message);
                return null;
            }
        }

        public bool atualizarTarefa()
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string update = $"update tarefas set nome = '{Nome}', completa = '{Completa}' where nome = '{Nome}';";
                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = update;

                comandoSql.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método atualizarTarefa " + ex.Message);
                return false;
            }
        }

        public bool deletarTarefa()
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string delete = $"delete from tarefas where nome = '{Nome}';";
                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = delete;

                comandoSql.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método deletarTarefa: " + ex.Message);
                return false;
            }
        }

        public int contarTarefas()
        {
            int count = 0;
            MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);

            try
            {
                MysqlConexaoBanco.Open();
                string query = "SELECT COUNT(*) FROM tarefas";
                MySqlCommand comandoSql = new MySqlCommand(query, MysqlConexaoBanco);

                count = Convert.ToInt32(comandoSql.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no banco de dados - método contarTarefas: " + ex.Message);
            }
            return count;
        }
    }
}
