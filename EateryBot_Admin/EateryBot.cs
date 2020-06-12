using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace EateryBot_Admin
{
    public partial class EateryBot : Form
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString();
        MySqlConnection conn = new MySqlConnection(connectionString);
        MySqlCommand cmd;
        DataTable table;
        DataTable table1;
        DataTable table2;
        DataTable table3;
        MySqlDataAdapter da;
        BindingSource source;
        public EateryBot()
        {
            InitializeComponent();
        }

        private void EateryBot_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void EateryBot_Load(object sender, EventArgs e)
        {
            MySqlDataAdapter da = new MySqlDataAdapter();
            table = new DataTable();
            da.SelectCommand = new MySqlCommand("SELECT * FROM Users", conn);
            da.Fill(table);
            BindingSource source = new BindingSource();
            source.DataSource = table;
            users.DataSource = source;

            MySqlDataAdapter da1 = new MySqlDataAdapter();
            table1 = new DataTable();
            da1.SelectCommand = new MySqlCommand("SELECT * FROM Products", conn);
            da1.Fill(table1);
            BindingSource source1 = new BindingSource();
            source1.DataSource = table1;
            products.DataSource = source1;

            MySqlDataAdapter da2 = new MySqlDataAdapter();
            table2 = new DataTable();
            da2.SelectCommand = new MySqlCommand("SELECT * FROM Courses", conn);
            da2.Fill(table2);
            BindingSource source2 = new BindingSource();
            source2.DataSource = table2;
            courses.DataSource = source2;

            MySqlDataAdapter da3 = new MySqlDataAdapter();
            table3 = new DataTable();
            da3.SelectCommand = new MySqlCommand("SELECT * FROM Orders", conn);
            da3.Fill(table3);
            BindingSource source3 = new BindingSource();
            source3.DataSource = table3;
            orders.DataSource = source3;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            var query = "INSERT INTO Users(FirstName, LastName, ID) VALUES (@fName, @lName, @uID)";
            cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@fName", fNameBox.Text);
            cmd.Parameters.AddWithValue("@lName", lNameBox.Text);
            cmd.Parameters.AddWithValue("@uID", idBox.Text);

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    fNameBox.Clear();
                    lNameBox.Clear();
                    idBox.Clear();
                    MessageBox.Show("Добавлено.");
                }

                table.Rows.Clear();
                users.Refresh();

                string sql = "SELECT * FROM Users ";
                cmd = new MySqlCommand(sql, conn);

                try
                {

                    da = new MySqlDataAdapter(cmd);
                    da.Fill(table);
                    source = new BindingSource();
                    source.DataSource = table;
                    users.DataSource = source;
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

        private void delButton_Click(object sender, EventArgs e)
        {
            string selected = users.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            var query = "DELETE FROM Users WHERE UserID= " + id;
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
                        fNameBox.Clear();
                        lNameBox.Clear();
                        idBox.Clear();
                        MessageBox.Show("Удалено.");
                    }
                    else 
                    {
                        MessageBox.Show("Ошибка удаления.");
                    }
                }

                table.Rows.Clear();
                users.Refresh();

                string sql1 = "SELECT * FROM Users ";
                cmd = new MySqlCommand(sql1, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table1 = new DataTable();
                da.Fill(table1);
                BindingSource source1 = new BindingSource();
                source1.DataSource = table1;
                users.DataSource = source1;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            fNameBox.Clear();
            lNameBox.Clear();
            idBox.Clear();
        }

        private void users_Click(object sender, EventArgs e)
        {
            try
            {
                fNameBox.Text = users.SelectedRows[0].Cells[1].Value.ToString();
                lNameBox.Text = users.SelectedRows[0].Cells[2].Value.ToString();
                idBox.Text = users.SelectedRows[0].Cells[3].Value.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка выделения строки");
            }
        }

        private void updButton_Click(object sender, EventArgs e)
        {
            var query = "UPDATE Users SET FirstName = @fName, LastName = @lName, ID = @uID WHERE UserID = @id";
            cmd = new MySqlCommand(query, conn);
            String selected = users.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@fName", fNameBox.Text);
            cmd.Parameters.AddWithValue("@lName", lNameBox.Text);
            cmd.Parameters.AddWithValue("@uID", idBox.Text);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    fNameBox.Clear();
                    lNameBox.Clear();
                    idBox.Clear();
                    MessageBox.Show("Обновлено");
                }

                conn.Close();

                table.Rows.Clear();
                users.Refresh();

                string sql2 = "SELECT * FROM Users";
                cmd = new MySqlCommand(sql2, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table2 = new DataTable();
                da.Fill(table2);
                BindingSource source2 = new BindingSource();
                source2.DataSource = table2;
                users.DataSource = source2;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void productAdd_Click(object sender, EventArgs e)
        {
            var query = "INSERT INTO Products(Name, Amount, Price) VALUES (@name, @amount, @price)";
            cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@name", productNameBox.Text);
            cmd.Parameters.AddWithValue("@amount", productAmount.Text);
            cmd.Parameters.AddWithValue("@price", productPrice.Text);

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    productNameBox.Clear();
                    productAmount.Clear();
                    productPrice.Clear();
                    MessageBox.Show("Добавлено.");
                }

                table1.Rows.Clear();
                products.Refresh();

                string sql = "SELECT * FROM Products ";
                cmd = new MySqlCommand(sql, conn);
                try
                {
                    da = new MySqlDataAdapter(cmd);
                    DataTable table6 = new DataTable();
                    da.Fill(table6);
                    BindingSource source6 = new BindingSource();
                    source6.DataSource = table6;
                    products.DataSource = source6;
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

        private void productUpd_Click(object sender, EventArgs e)
        {
            var query = "UPDATE Products SET Name = @name, Amount = @amount, Price = @price WHERE ProductID = @id";
            cmd = new MySqlCommand(query, conn);
            String selected = products.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", productNameBox.Text);
            cmd.Parameters.AddWithValue("@amount", productAmount.Text);
            cmd.Parameters.AddWithValue("@price", productPrice.Text);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    productNameBox.Clear();
                    productAmount.Clear();
                    productPrice.Clear();
                    MessageBox.Show("Обновлено");
                }

                conn.Close();

                table1.Rows.Clear();
                products.Refresh();

                string sql2 = "SELECT * FROM Products";
                cmd = new MySqlCommand(sql2, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table5 = new DataTable();
                da.Fill(table5);
                BindingSource source5 = new BindingSource();
                source5.DataSource = table5;
                products.DataSource = source5;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void productDel_Click(object sender, EventArgs e)
        {
            string selected = products.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            var query = "DELETE FROM Products WHERE ProductID= " + id;
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
                        productNameBox.Clear();
                        productAmount.Clear();
                        productPrice.Clear();
                        MessageBox.Show("Удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления.");
                    }
                }

                table1.Rows.Clear();
                products.Refresh();

                string sql1 = "SELECT * FROM Products ";
                cmd = new MySqlCommand(sql1, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table3 = new DataTable();
                da.Fill(table3);
                BindingSource source3 = new BindingSource();
                source3.DataSource = table3;
                products.DataSource = source3;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void products_Click(object sender, EventArgs e)
        {
            try
            {

                productNameBox.Text = products.SelectedRows[0].Cells[1].Value.ToString();
                productAmount.Text = products.SelectedRows[0].Cells[2].Value.ToString();
                productPrice.Text = products.SelectedRows[0].Cells[3].Value.ToString();
            }
            catch (Exception) {
                MessageBox.Show("Ошибка выделения строки");
            }
        }

        private void productClear_Click(object sender, EventArgs e)
        {
            productNameBox.Clear();
            productAmount.Clear();
            productPrice.Clear();
        }

        private void courseClear_Click(object sender, EventArgs e)
        {
            courseName.Clear();
            courseWeight.Clear();
            coursePrice.Clear();
            courseIDComp.Clear();
        }

        private void courseAdd_Click(object sender, EventArgs e)
        {
            var query = "INSERT INTO Courses(Name, Weight, Price, CompositionID) VALUES (@name, @amount, @price, @compID)";
            cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@name", courseName.Text);
            cmd.Parameters.AddWithValue("@amount", courseWeight.Text);
            cmd.Parameters.AddWithValue("@price", coursePrice.Text);
            cmd.Parameters.AddWithValue("@compID", courseIDComp.Text);

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    courseName.Clear();
                    courseWeight.Clear();
                    coursePrice.Clear();
                    courseIDComp.Clear();
                    MessageBox.Show("Добавлено.");
                }

                table2.Rows.Clear();
                courses.Refresh();

                string sql = "SELECT * FROM Courses ";
                cmd = new MySqlCommand(sql, conn);
                try
                {
                    da = new MySqlDataAdapter(cmd);
                    DataTable table7 = new DataTable();
                    da.Fill(table7);
                    BindingSource source7 = new BindingSource();
                    source7.DataSource = table7;
                    courses.DataSource = source7;
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

        private void courseDel_Click(object sender, EventArgs e)
        {
            string selected = courses.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            var query = "DELETE FROM Courses WHERE CourseID= " + id;
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
                        courseName.Clear();
                        courseWeight.Clear();
                        coursePrice.Clear();
                        courseIDComp.Clear();
                        MessageBox.Show("Удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления.");
                    }
                }

                table2.Rows.Clear();
                courses.Refresh();

                string sql1 = "SELECT * FROM Courses ";
                cmd = new MySqlCommand(sql1, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table8 = new DataTable();
                da.Fill(table8);
                BindingSource source8 = new BindingSource();
                source8.DataSource = table8;
                courses.DataSource = source8;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void courseUpd_Click(object sender, EventArgs e)
        {
            var query = "UPDATE Courses SET Name = @name, Weight = @weight, Price = @price, CompositionID = @compID WHERE CourseID = @id";
            cmd = new MySqlCommand(query, conn);
            String selected = courses.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@name", courseName.Text);
            cmd.Parameters.AddWithValue("@weight", courseWeight.Text);
            cmd.Parameters.AddWithValue("@price", coursePrice.Text);
            cmd.Parameters.AddWithValue("@compID", courseIDComp.Text);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() > 0)
                {
                    courseName.Clear();
                    courseWeight.Clear();
                    coursePrice.Clear();
                    courseIDComp.Clear();
                    MessageBox.Show("Обновлено");
                }

                conn.Close();

                table2.Rows.Clear();
                courses.Refresh();

                string sql2 = "SELECT * FROM Courses";
                cmd = new MySqlCommand(sql2, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table9 = new DataTable();
                da.Fill(table9);
                BindingSource source9 = new BindingSource();
                source9.DataSource = table9;
                courses.DataSource = source9;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void courses_Click(object sender, EventArgs e)
        {
            try
            {
                courseName.Text = courses.SelectedRows[0].Cells[1].Value.ToString();
                courseWeight.Text = courses.SelectedRows[0].Cells[2].Value.ToString();
                coursePrice.Text = courses.SelectedRows[0].Cells[3].Value.ToString();
                courseIDComp.Text = courses.SelectedRows[0].Cells[4].Value.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка выделения строки");
            }
        }

        private void orderDel_Click(object sender, EventArgs e)
        {
            string selected = orders.SelectedRows[0].Cells[0].Value.ToString();
            int id = Convert.ToInt32(selected);

            var query = "DELETE FROM Orders WHERE ID= " + id;
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
                        MessageBox.Show("Удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Ошибка удаления.");
                    }
                }

                table3.Rows.Clear();
                orders.Refresh();

                string sql1 = "SELECT * FROM Orders ";
                cmd = new MySqlCommand(sql1, conn);

                conn.Close();

                da = new MySqlDataAdapter(cmd);
                DataTable table9 = new DataTable();
                da.Fill(table9);
                BindingSource source9 = new BindingSource();
                source9.DataSource = table9;
                orders.DataSource = source9;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                conn.Close();
            }
        }

        private void orders_Click(object sender, EventArgs e)
        {
            
        }

        private void exitButt_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuButton_Click(object sender, EventArgs e)
        {
            Menu m = new Menu();
            this.Hide();
            m.Show();
        }
    }
}

