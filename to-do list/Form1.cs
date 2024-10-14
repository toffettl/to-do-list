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
            CadastrarTarefa cadastrarTarefa = new CadastrarTarefa();
            int totalTarefas = cadastrarTarefa.contarTarefas();

            for (int i = 0; i <= totalTarefas; i++)
            {
                MySqlDataReader reader = cadastrarTarefa.localizarTarefa(i);

                if (reader != null && reader.Read())
                {
                    string nomeTarefa = reader["nome"].ToString();
                    criarPanel(nomeTarefa);
                }
            }
        }
        public void criarPanel(string nome)
        {
            CadastrarTarefa cadTarefas = new CadastrarTarefa();
            Panel painel = new Panel()
            {
                BackColor = Color.FromArgb(164, 172, 134),
                Size = new Size(990, 50)
            };
            Label label = new Label()
            {
                Text = nome,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Location = new Point(10, 10) //location dentro do painel
            };
            Button btnExcluir = new Button()
            {
                Location = new Point(800, 10),
                Text = "Remover painel",
                Name = txtNomeTarefa.Text,
            };
            Button btnConcluir = new Button()
            {
                Location = new Point(500, 10),
                Text = "Concluir tarefa",
                Name = txtNomeTarefa.Text
            };

            btnExcluir.Click += (s, args) => flowLayoutPanel2.Controls.Remove(painel);
            btnExcluir.Click += BtnExcluir_Click;

            btnConcluir.Click += BtnConcluir_Click;

            painel.Controls.Add(label);
            painel.Controls.Add(btnExcluir);
            painel.Controls.Add(btnConcluir);

            flowLayoutPanel2.Controls.Add(painel);

            void BtnExcluir_Click(object sender, EventArgs e)
            {
                try
                {
                    cadTarefas.Nome = btnExcluir.Name;
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
                cadTarefas.Nome = btnConcluir.Name;
                if (cadTarefas.atualizarTarefa())
                {
                    if(cadTarefas.Completa == 0)
                    {
                        cadTarefas.Completa += 1;
                        btnConcluir.BackColor = Color.Red;
                        cadTarefas.atualizarTarefa();
                    }
                    else
                    {
                        cadTarefas.Completa -= 1;
                        btnConcluir.BackColor = Color.AliceBlue;
                        cadTarefas.atualizarTarefa();
                    }
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
                    cadTarefa.Completa = 0;
                    cadTarefa.Nome = txtNomeTarefa.Text;
                    criarPanel(txtNomeTarefa.Text);
                    if (cadTarefa.cadastrarTarefa())
                    {
                        MessageBox.Show($"Tarefa adicionada com sucesso!");
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
