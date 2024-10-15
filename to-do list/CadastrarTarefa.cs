using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public bool verificarCompleta;

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
        public int cadastrarTarefa()
        {
            int idTarefa = 0;

            using (MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor))
            {
                MysqlConexaoBanco.Open();

                string insert = "INSERT INTO tarefas (nome, completa) VALUES (@Nome, @Completa); SELECT LAST_INSERT_ID();";

                using (MySqlCommand comandoSql = new MySqlCommand(insert, MysqlConexaoBanco))
                {
                    comandoSql.Parameters.AddWithValue("@Nome", Nome);
                    comandoSql.Parameters.AddWithValue("@Completa", Completa = 1);

                    idTarefa = Convert.ToInt32(comandoSql.ExecuteScalar());
                }
            }

            return idTarefa;
        }

            public MySqlDataReader localizarTarefa(int numeroTarefas)
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string select = $"select id, nome , completa from tarefas where id = '{numeroTarefas}';";

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
                string update = $"update tarefas set completa = '{Completa}' where id = '{Id}';";
                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = update;
                comandoSql.ExecuteNonQuery();
                if (Completa == 1)
                {
                    verificarCompleta = true;
                    Completa -= 1;
                }
                else
                {
                    verificarCompleta = false;
                    Completa += 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método atualizarTarefa " + ex.Message);
                return false;
            }
        }
        public bool atualizarNome()
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();
                string updateNome = $"update tarefas set nome = '{Nome}' where id = '{Id}';";
                MySqlCommand comandoSql = MysqlConexaoBanco.CreateCommand();
                comandoSql.CommandText = updateNome;
                comandoSql.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro no banco de dados - método atualizarNome " + ex.Message);
                return false;
            }
        }


        public bool deletarTarefa()
        {
            try
            {
                MySqlConnection MysqlConexaoBanco = new MySqlConnection(ConexaoBanco.bancoServidor);
                MysqlConexaoBanco.Open();

                string delete = $"delete from tarefas where id = '{Id}';";
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
