using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace App_Perusahaan
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public MySqlConnection conn;

        private void openDBConn()
        {
            conn = new MySqlConnection();
            conn.ConnectionString = "server = 139.255.11.84; uid = student; pwd = isbmantap; database = KMMI5; ";

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtAdmin = new DataTable();
                MySqlCommand cmdLoginAdmin = new MySqlCommand();
                cmdLoginAdmin.Connection = conn;
                cmdLoginAdmin.CommandText = "select id_admin as `ID`, `password` as `Pass` from admin where id_admin = '" + tBoxLoginID.Text.ToString() + "'";
                MySqlDataAdapter sqlAdapter = new MySqlDataAdapter();
                sqlAdapter = new MySqlDataAdapter(cmdLoginAdmin);
                sqlAdapter.Fill(dtAdmin);

                if (tBoxLoginID.Text == dtAdmin.Rows[0]["ID"].ToString() && tBoxLoginPass.Text == dtAdmin.Rows[0]["Pass"].ToString())
                {
                   
                    FormMenu fMenu = new FormMenu();
                    fMenu.init(this);
                    fMenu.ShowDialog();
                }
                else
                {
                    MessageBox.Show("ID atau Password yang dimasukkan salah!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            openDBConn();
            tBoxLoginPass.UseSystemPasswordChar = true;
        }

        private void cBoxShow_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cBoxShow.Checked == true)
                {
                    tBoxLoginPass.UseSystemPasswordChar = false;
                }
                else if (cBoxShow.Checked == false)
                {
                    tBoxLoginPass.UseSystemPasswordChar = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
