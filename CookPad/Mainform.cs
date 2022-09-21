using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CookPad
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        readonly SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Simfo\source\repos\CookPad\CookPad\Database.mdf;Integrated Security=True"); //Data base connection.
        SqlCommand cmd;
        int id = 0;
        

        private void DownloadImage2(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Select image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                AddpictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void Load_data()
        {
            cmd = new SqlCommand("Select *from table1 order by id desc", conn);
            SqlDataAdapter da = new SqlDataAdapter
            {
                SelectCommand = cmd
            };
            DataTable dt = new DataTable();
            dt.Clear();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            _ = new DataGridViewImageColumn();
            DataGridViewImageColumn pic1 = (DataGridViewImageColumn)dataGridView1.Columns[2];
            pic1.ImageLayout = DataGridViewImageCellLayout.Stretch;

        }

        private void TabPage2_Click(object sender, EventArgs e)
        {
            Load_data();
        }

        public void DisplayData() // Data display.
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from table1";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Savereceip(object sender, EventArgs e)
        {
            System.Collections.IList index = Controls;
            for (int i = 0; i < index.Count; i++) // Check for empty fields.
            {
                _ = (Control)index[i];
                if (AddName_textbox.Text == "")
                {
                    MessageBox.Show("Не всі поля заповнені. Будь-ласка введіть дані в поля!");
                    break;
                }
                if (AddIngredients_textbox.Text == "")
                {
                    MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                    break;
                }
                if (AddSteps_textbox.Text == "")
                {
                    MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                    break;
                }
                if (AddTime_textbox.Text == "")
                {
                    MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                    break;
                }
                if (AddcomboBox1.Text == "")
                {
                    MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                    break;
                }
                if (AddpictureBox1.Image == null)
                {
                    MessageBox.Show("Зображення не було завантажено");
                    break;
                }
                else
                {
                    cmd = new SqlCommand("Insert into Table1(name,image,ingredients,steps,time,feature,favorites)Values(@name,@image,@ingredients,@steps,@time,@feature,@favorites)", conn); // Add recipes to the database.
                    MemoryStream memstr = new MemoryStream();
                    AddpictureBox1.Image.Save(memstr, AddpictureBox1.Image.RawFormat);
                    cmd.Parameters.AddWithValue("@image", memstr.ToArray());
                    cmd.Parameters.AddWithValue("@name", AddName_textbox.Text);
                    cmd.Parameters.AddWithValue("@ingredients", AddIngredients_textbox.Text);
                    cmd.Parameters.AddWithValue("@steps", AddSteps_textbox.Text);
                    cmd.Parameters.AddWithValue("@time", AddTime_textbox.Text);
                    cmd.Parameters.AddWithValue("@feature", AddcomboBox1.Text);
                    cmd.Parameters.AddWithValue("@favorites", "-");
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Рецепт успішно додано");
                    Load_data();
                    DisplayData();
                    ClearFields();
                }
            }
        }

        private void ClearFields()
        {
            AddName_textbox.Text = "";
            AddIngredients_textbox.Text = "";
            AddSteps_textbox.Text = "";
            AddTime_textbox.Text = "";
            Search_textbox.Text = "";
            Name_textbox.Text = "";
            Ingredients_textbox.Text = "";
            Steps_textbox.Text = "";
            Time_textbox.Text = "";
            Fav_textbox.Text = "";
            AddcomboBox1.SelectedIndex = -1;
            EditcomboBox2.SelectedIndex = -1;
            if (AddpictureBox1.Image != null)
            {
                AddpictureBox1.Image.Dispose();
                AddpictureBox1.Image = null;
            }
            if (EditpictureBox2.Image != null)
            {
                EditpictureBox2.Image.Dispose();
                EditpictureBox2.Image = null;
            }
        }

        private void Mainform_Load(object sender, EventArgs e)
        {
            DisplayData();
        }

        private void Delete_btn(object sender, EventArgs e) // Delete from database.
        {
            SqlCommand com = new SqlCommand("DELETE FROM table1 WHERE id=@id", conn);
            int id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
            int a = dataGridView1.CurrentRow.Index;
            dataGridView1.Rows.Remove(dataGridView1.Rows[a]);
            com.Parameters.AddWithValue("@id", id);
            conn.Open();
            try
            {
                com.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Запис видалено");
            }
            catch
            {
                MessageBox.Show("Видалити не вдалося. Спробуйте ще раз!");
            }
        }

        public void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            _ = new DataGridViewImageColumn();
            this.dataGridView1.Columns["Id"].Visible = false;
            DataGridViewImageColumn pic1 = (DataGridViewImageColumn)dataGridView1.Columns[2];
            pic1.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.DefaultCellStyle.WrapMode =
            DataGridViewTriState.True;
            dataGridView1.RowTemplate.Height = 150;
            dataGridView1.RowTemplate.MinimumHeight = 150;
            dataGridView1.Columns[2].Width = 150;
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[1].HeaderText = "Назва";
            dataGridView1.Columns[2].HeaderText = "Зображення";
            dataGridView1.Columns[3].HeaderText = "Інгредієнти";
            dataGridView1.Columns[4].HeaderText = "Кроки";
            dataGridView1.Columns[5].HeaderText = "Час (в хв)";
            dataGridView1.Columns[6].HeaderText = "Особливість";
            dataGridView1.Columns[7].HeaderText = "Обране";
            EditcomboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            AddcomboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        }

        private void Searchall_btn(object sender, EventArgs e) // Search for recipes by name in the database.
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = String.Format("Name like '%" + Search_textbox.Text + "%'");
        }

        private void Searchfav(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * from Table1 where Favorites = '+'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void SearchDesert(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * from Table1 where Feature = N'Десерти'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Updatebtn(object sender, EventArgs e)
        {
            Load_data();
            ClearFields();
        }

        private void SearchNoLactose(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * from Table1 where Feature = N'Страви без лактози'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void Searchvegan(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * from Table1 where Feature = N'Вегетаріанські страви'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                Name_textbox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                Ingredients_textbox.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                Steps_textbox.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                Time_textbox.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                Fav_textbox.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                EditcomboBox2.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                MemoryStream ms = new MemoryStream((byte[])dataGridView1.CurrentRow.Cells[2].Value);
                EditpictureBox2.Image = Image.FromStream(ms);
                btnSave.Text = "Зберегти";
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (btnSave.Text == "Зберегти")
            {
                System.Collections.IList index = Controls;
                for (int i = 0; i < index.Count; i++)
                {
                    _ = (Control)index[i];
                    if (Steps_textbox.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    if (Name_textbox.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    if (Ingredients_textbox.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    if (Steps_textbox.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    if (Time_textbox.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    if (EditcomboBox2.Text == "")
                    {
                        MessageBox.Show("Не всі поля заповнені. Будь-ласка заповніть поля!");
                        break;
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("Update table1 set Name=@Name,Ingredients=@Ingredients,Steps=@Steps,Time=@Time,Feature=@Feature,Favorites = @Favorites, Image=@Image WHERE id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", Name_textbox.Text);
                        cmd.Parameters.AddWithValue("@ingredients", Ingredients_textbox.Text);
                        cmd.Parameters.AddWithValue("@steps", Steps_textbox.Text);
                        cmd.Parameters.AddWithValue("@time", Time_textbox.Text);
                        cmd.Parameters.AddWithValue("@favorites", Fav_textbox.Text);
                        cmd.Parameters.AddWithValue("@feature", EditcomboBox2.Text);
                        MemoryStream memst = new MemoryStream();
                        EditpictureBox2.Image.Save(memst, EditpictureBox2.Image.RawFormat);
                        cmd.Parameters.AddWithValue("image", memst.ToArray());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Успішно збережено");
                        ClearFields();
                    }
                }
            }
        }

        private void DownloadPhoto_btn(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Select image(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                EditpictureBox2.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void Name_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void Fav_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            _ = e.KeyChar;
            if (e.KeyChar != 43 && e.KeyChar != 45 && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }

        private void Time_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void AddName_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void AddTime_textbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void Mainform_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                BtnSave_Click(btnSave, null);
            }
            if (e.KeyValue == (char)Keys.Enter)
            {
                Savereceip(button1, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Mainform_HelpRequested(object sender, HelpEventArgs hlpevent) // Display the User Information form.
        {
            using (var helpForm = new Form3())
            {
                helpForm.ShowDialog();
            }
        }
    }
} 