using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace EateryBot_Admin
{
    public partial class Menu : Form
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        MySqlConnection conn = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        DataTable table;
        MySqlDataAdapter da;
        BindingSource source;
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            MySqlDataAdapter da = new MySqlDataAdapter();
            table = new DataTable();
            da.SelectCommand = new MySqlCommand("SELECT * FROM Menu", conn);
            da.Fill(table);
            BindingSource source = new BindingSource();
            source.DataSource = table;
            menus.DataSource = source;
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void backButt_Click(object sender, EventArgs e)
        {
            this.Hide();
            EateryBot eB = new EateryBot();
            eB.Show();
        }

        private void menuAdd_Click(object sender, EventArgs e)
        {
            var query = "INSERT INTO Menu (DayOfWeek, First, FirstPrice, Second, SecondPrice, Salad, SaladPrice, Liquid, LiquidPrice) VALUES(@day, @fst, @fstPrice, @snd, @sndPrice, @salad," +
                "@saladPrice, @liquid, @liquidPrice)";
            cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@day", dayOfWeekBox.Text);
            cmd.Parameters.AddWithValue("@fst", firstBox.Text);
            cmd.Parameters.AddWithValue("@fstPrice", firstPriceBox.Text);
            cmd.Parameters.AddWithValue("@snd", secondbox.Text);
            cmd.Parameters.AddWithValue("@sndPrice", secondPriceBox.Text);
            cmd.Parameters.AddWithValue("@salad", salatBox.Text);
            cmd.Parameters.AddWithValue("@saladPrice", salatPriceBox.Text);
            cmd.Parameters.AddWithValue("@liquid", liquidBox.Text);
            cmd.Parameters.AddWithValue("@liquidPrice", liquidPriceBox.Text);

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    dayOfWeekBox.Clear();
                    firstPriceBox.Clear();
                    firstBox.Clear();
                    secondbox.Clear();
                    secondPriceBox.Clear();
                    salatBox.Clear();
                    salatPriceBox.Clear();
                    liquidBox.Clear();
                    liquidPriceBox.Clear();

                    MessageBox.Show("Добавлено.");
                }

                table.Rows.Clear();
                menus.Refresh();

                string sql = "SELECT * FROM Menu ";
                cmd = new MySqlCommand(sql, conn);

                try
                {

                    da = new MySqlDataAdapter(cmd);
                    da.Fill(table);
                    source = new BindingSource();
                    source.DataSource = table;
                    menus.DataSource = source;
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления. " + ex);
                conn.Close();
            }
        }

        private void menuDel_Click(object sender, EventArgs e)
        {
            string selected = menus.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            var query = "DELETE FROM Menu WHERE MenuCode= " + id;
            MySqlCommand cmd1 = new MySqlCommand(query, conn);
            conn.Open();

            try
            {
                da = new MySqlDataAdapter(cmd1);
                da.DeleteCommand = conn.CreateCommand();
                da.DeleteCommand.CommandText = query;

                if (MessageBox.Show("Вы уверены?", "Удалить", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd1.ExecuteNonQuery() == 1)
                    {
                        dayOfWeekBox.Clear();
                        firstPriceBox.Clear();
                        firstBox.Clear();
                        secondbox.Clear();
                        secondPriceBox.Clear();
                        salatBox.Clear();
                        salatPriceBox.Clear();
                        liquidBox.Clear();
                        liquidPriceBox.Clear();

                        MessageBox.Show("Удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления.");
                    }
                }

                table.Rows.Clear();
                menus.Refresh();

                string sql1 = "SELECT * FROM Menu ";
                cmd = new MySqlCommand(sql1, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable();
                da.Fill(table1);
                BindingSource source1 = new BindingSource();
                source1.DataSource = table1;
                menus.DataSource = source1;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void menus_Click(object sender, EventArgs e)
        {
            dayOfWeekBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            firstPriceBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            firstBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            secondbox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            secondPriceBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            salatBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            salatPriceBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            liquidBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
            liquidPriceBox.Text = menus.SelectedRows[0].Cells[1].Value.ToString();
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            dayOfWeekBox.Clear();
            firstPriceBox.Clear();
            firstBox.Clear();
            secondbox.Clear();
            secondPriceBox.Clear();
            salatBox.Clear();
            salatPriceBox.Clear();
            liquidBox.Clear();
            liquidPriceBox.Clear();
        }

        private void menuUpd_Click(object sender, EventArgs e)
        {
            var query = "UPDATE Menu SET DayOfWeek=@day, First=@fst, FirstPrice=@fstPrice, Second=@snd, SecondPrice=@sndPrice, Salad=@salad, SaladPrice=@saladPrice, Liquid=@liquid, LiquidPrice=@liquidPrice WHERE MenuCode=@menuCode";
            cmd = new MySqlCommand(query, conn);
            String selected = menus.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            cmd.Parameters.AddWithValue("@menuCode", id);
            cmd.Parameters.AddWithValue("@day", dayOfWeekBox.Text);
            cmd.Parameters.AddWithValue("@fst", firstBox.Text);
            cmd.Parameters.AddWithValue("@fstPrice", firstPriceBox.Text);
            cmd.Parameters.AddWithValue("@snd", secondbox.Text);
            cmd.Parameters.AddWithValue("@sndPrice", secondPriceBox.Text);
            cmd.Parameters.AddWithValue("@salad", salatBox.Text);
            cmd.Parameters.AddWithValue("@saladPrice", salatPriceBox.Text);
            cmd.Parameters.AddWithValue("@liquid", liquidBox.Text);
            cmd.Parameters.AddWithValue("@liquidPrice", liquidPriceBox.Text);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    dayOfWeekBox.Clear();
                    firstPriceBox.Clear();
                    firstBox.Clear();
                    secondbox.Clear();
                    secondPriceBox.Clear();
                    salatBox.Clear();
                    salatPriceBox.Clear();
                    liquidBox.Clear();
                    liquidPriceBox.Clear();
                    MessageBox.Show("Обновлено");
                }

                conn.Close();

                table.Rows.Clear();
                menus.Refresh();

                string sql2 = "SELECT * FROM Menu";
                cmd = new MySqlCommand(sql2, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table2 = new DataTable();
                da.Fill(table2);
                BindingSource source2 = new BindingSource();
                source2.DataSource = table2;
                menus.DataSource = source2;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }
    }
}
