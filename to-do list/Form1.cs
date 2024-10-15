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
            timer1 = new Timer();
            timer1.Interval = 500; // 500 milissegundos
            timer1.Tick += timer1_Tick;
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
                    Color corStatusTxt;
                    if (statusTarefa == 0)
                    {
                        corStatus = ColorTranslator.FromHtml("#D2EBD3");
                        corStatusTxt = ColorTranslator.FromHtml("#74C178");
                    }
                    else
                    {
                        corStatus = ColorTranslator.FromHtml("#F5D0CD");
                        corStatusTxt = ColorTranslator.FromHtml("#d63a2f");
                    }
                    criarPanel(Convert.ToInt32(idTarefa), nomeTarefa, Convert.ToInt32(statusTarefa), corStatus, corStatusTxt);
                }
            }
        }
        public void criarPanel(int idTarefa, string nomeTarefa, int statusTarefa, Color corStatus, Color corStatusTxt)
        {
            CadastrarTarefa cadTarefas = new CadastrarTarefa();
            Panel painel = new Panel()
            {
                BackColor = Color.White,
                Size = new Size(990, 50)
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
                BackColor = corStatus,
                ForeColor = corStatusTxt,
                Font = new Font("Arial", 8, FontStyle.Bold)
            };
            TextBox txtBoxNome = new TextBox()
            {
                Location = new Point(10, 10),
                Text = nomeTarefa,
                ForeColor = ColorTranslator.FromHtml("#454950"),
                BackColor = ColorTranslator.FromHtml("#F7F7F7"),
                Font = new Font("Sergoe UI", 12),
                Tag = idTarefa,
                BorderStyle = BorderStyle.None
            };
            DateTimePicker boxHoras = new DateTimePicker();
            {
                Location = new Point(50, 10);
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

            txtBoxNome.TextChanged += btnAtualizar_Click;

            painel.Controls.Add(btnExcluir);
            painel.Controls.Add(btnConcluir);
            painel.Controls.Add(txtBoxNome);
            painel.Controls.Add(boxHoras);

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
                            btnConcluir.BackColor = ColorTranslator.FromHtml("#F5D0CD");
                            btnConcluir.ForeColor = ColorTranslator.FromHtml("#d63a2f");
                        }
                        else
                        {
                            btnConcluir.BackColor = ColorTranslator.FromHtml("#D2EBD3");
                            btnConcluir.ForeColor = ColorTranslator.FromHtml("#74C178");
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
                cadTarefas.Id = Convert.ToInt32(txtBoxNome.Tag);
                cadTarefas.Nome = txtBoxNome.Text;
                txtBoxNome.Text = cadTarefas.Nome;

                if (cadTarefas.atualizarNome())
                {
                    pictureBox1.Visible = true;
                    timer1.Start();
                }
            }
        }
        private void btnAdicionarTarefa_Click(object sender, EventArgs e)
        {
            try
            {
                    CadastrarTarefa cadTarefa = new CadastrarTarefa();
                    int statusTarefa = 0;
                    Color corStatus = ColorTranslator.FromHtml("#F5D0CD");
                    Color corStatusTxt = ColorTranslator.FromHtml("#d63a2f");
                    int tarefaId = cadTarefa.cadastrarTarefa();
                    criarPanel(tarefaId, cadTarefa.Nome, statusTarefa, corStatus, corStatusTxt);
                    if (tarefaId > 0)
                    {
                        
                        MessageBox.Show($"Tarefa adicionada com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Não foi possivel cadastrar");
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastar tarefa: " + ex.Message);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;

            // Para o Timer
            timer1.Stop();
        }
    }
}
