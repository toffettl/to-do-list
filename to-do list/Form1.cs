using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace to_do_list
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            carregarLista();
        }
        private void carregarLista()
        {
            CadastrarTarefa cadastrarTarefa = new CadastrarTarefa();
            int totalTarefas = cadastrarTarefa.contarTarefas();

            for (int i = 0; i <= totalTarefas; i++)
            {
                MySqlDataReader reader = cadastrarTarefa.localizarTarefa(i);

                if (reader != null && reader.Read())
                {
                    string idTarefa = reader["id"].ToString();
                    string nomeTarefa = reader["nome"].ToString();
                    int statusTarefa = Convert.ToInt32(reader["completa"]);
                    Color corStatus;
                    if (statusTarefa == 1)
                    {
                        corStatus = Color.Red;
                    }
                    else
                    {
                        corStatus = Color.Blue;
                    }
                    criarPanel(Convert.ToInt32(idTarefa), nomeTarefa, Convert.ToInt32(statusTarefa), corStatus);
                }
            }
        }
        public void criarPanel(int idTarefa, string nomeTarefa, int statusTarefa, Color corStatus)
        {
            CadastrarTarefa cadTarefas = new CadastrarTarefa();
            Panel painel = new Panel()
            {
                BackColor = Color.FromArgb(164, 172, 134),
                Size = new Size(990, 50)
            };
            Label label = new Label()
            {
                Text = nomeTarefa,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Location = new Point(10, 10) //location dentro do painel
            };
            Button btnExcluir = new Button()
            {
                Location = new Point(800, 10),
                Text = "Remover painel",
                Tag = idTarefa
            };
            Button btnConcluir = new Button()
            {
                Location = new Point(500, 10),
                Text = "Concluir tarefa",
                Tag = idTarefa,
                BackColor = corStatus
            };
            Button btnAtualizar = new Button()
            {
                Location = new Point(300, 10),
                Text = "Atualizar tarefa",
                Tag = idTarefa
            };
            TextBox txtBoxNome = new TextBox()
            {
                Location = new Point(150, 10),
                Text = "Tarrefa",
                Tag = idTarefa

            };

            btnExcluir.Click += (s, args) => flowLayoutPanel2.Controls.Remove(painel);
            btnExcluir.Click += BtnExcluir_Click;

            btnConcluir.Click += BtnConcluir_Click;
            if(cadTarefas.Completa == 0)
            {
                corStatus = Color.Red;
            }
            else
            {
                corStatus = Color.Blue;
            }

            btnAtualizar.Click += btnAtualizar_Click;

            painel.Controls.Add(label);
            painel.Controls.Add(btnExcluir);
            painel.Controls.Add(btnConcluir);
            painel.Controls.Add(btnAtualizar);
            painel.Controls.Add(txtBoxNome);

            flowLayoutPanel2.Controls.Add(painel);

            void BtnExcluir_Click(object sender, EventArgs e)
            {
                try
                {
                    cadTarefas.Id = Convert.ToInt32(btnExcluir.Tag);
                    if (cadTarefas.deletarTarefa())
                    {
                        MessageBox.Show("Deletado com sucesso!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar tarefa!" + ex.Message);
                }
            }

            void BtnConcluir_Click(object sender, EventArgs e)
            {
                try
                {
                    cadTarefas.Id = Convert.ToInt32(btnConcluir.Tag);
                    cadTarefas.Nome = txtBoxNome.Text;
                    if (cadTarefas.atualizarTarefa())
                    {
                       if(cadTarefas.verificarCompleta == true)
                        {
                            btnConcluir.BackColor = Color.Red;
                        }
                        else
                        {
                            btnConcluir.BackColor = Color.Blue;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Concluída com sucesso!" + ex.Message);
                }
            }

            void btnAtualizar_Click(object sender, EventArgs e)
            {
                cadTarefas.Id = Convert.ToInt32(btnAtualizar.Tag);
                cadTarefas.Nome = txtBoxNome.Text;
                txtBoxNome.Text = cadTarefas.Nome;
                if (cadTarefas.atualizarTarefa())
                {
                    MessageBox.Show("Nome atualizado com sucesso!");
                }
            }
        }
        private void btnAdicionarTarefa_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtNomeTarefa.Text.Equals(""))
                {
                    CadastrarTarefa cadTarefa = new CadastrarTarefa();
                    int statusTarefa = 0;
                    Color corStatus = Color.Red;
                    cadTarefa.Nome = txtNomeTarefa.Text;
                    int tarefaId = cadTarefa.cadastrarTarefa();
                    criarPanel(tarefaId, txtNomeTarefa.Text, statusTarefa, corStatus);
                    if (tarefaId > 0)
                    {
                        MessageBox.Show($"Tarefa adicionada com sucesso!");
                        label1.Text = Convert.ToString(tarefaId);
                        txtNomeTarefa.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Não foi possivel cadastrar");
                    }
                }
                else
                {
                    MessageBox.Show("Favor preencher todos os campos corretamente!");
                    txtNomeTarefa.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastar tarefa: " + ex.Message);
            }
        }
    }
}
