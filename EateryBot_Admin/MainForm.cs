using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace EateryBot_Admin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            MySqlConnection conn = new MySqlConnection(connectionString.ToString());
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Admins WHERE Login=@login AND Password=@pass", conn);
            conn.Open();
            cmd.Parameters.AddWithValue("@login", loginBox.Text);
            cmd.Parameters.AddWithValue("@pass", passwdBox.Text);
            Console.WriteLine(passwdBox.Text, loginBox.Text);
            MySqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows == true)
            {
                EateryBot eatery = new EateryBot();
                eatery.Show();
                this.Hide();
            }
            else
                MessageBox.Show("Неверные данные для входа.");
            conn.Close();
        }
    }
}
