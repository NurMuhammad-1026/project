using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;
using FormBorrowedAndRepaid.Models;
using WindowsFormsApp1;
using static FormBorrowedAndRepaid.ChangedUserData;

namespace FormBorrowedAndRepaid
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private void Users_Activated(object sender, EventArgs e)
        {
            getCities();          
            search();            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            search();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            search();
        }

        void search()
        {
            string name = textBox1.Text.Trim();
            string code = textBox2.Text.Trim();
            string city = comboBox1.Text.Trim();

            var d = new Dictionary<string, object>();
            var l = new List<string>();

            if (!string.IsNullOrEmpty(name))
            {
                l.Add("users.name LIKE @name");
                d.Add("@name", "% " + name + " %");
            }

            if (!string.IsNullOrEmpty(code))
            {
                l.Add("users.code LIKE @code");
                d.Add("@code", "% " + name + " %");
            }

            if (!string.IsNullOrEmpty(city))
            {
                l.Add("city.name LIKE @city");
                d.Add("@city", "% " + name + " %");
            }


            string whereClause;

            if (l.Count > 0)
            {
                whereClause = "WHERE " + string.Join(" AND ", l);
            }
            else
            {
                whereClause = "";
            }

            var result = MySQL.select(@"SELECT users.id, users.name, users.code, users.phone_number, city.id AS city_id, city.name AS city_name FROM users JOIN city ON users.city_id = city.id " + whereClause, d);

            if (result.execute && result.data.Count > 0)
            {
                dataGridViewUser.Rows.Clear();

                foreach (var item in result.data)
                {
                    dataGridViewUser.Rows.Add(item["id"], item["city_id"], item["name"], item["code"], item["phone_number"], item["city_name"]);
                }
            }
        }

        private void getCities()
        {
            try
            {
                Select a = MySQL.select("SELECT * FROM city WHERE country_id = 1");

                if (a.execute && a.data != null)
                {
                    comboBox1.Items.Clear();

                    foreach (var line in a.data)
                    {
                        comboBox1.Items.Add(new ComboBoxItem(Convert.ToInt32(line["id"]), line["name"].ToString()));
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void changedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewUser.CurrentRow == null)
            {
                MessageBox.Show("Сначала выберите строку");
                return;
            }

            var userId = dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["id"].Value?.ToString() ?? "";
            var userName = dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["name"].Value?.ToString() ?? "";
            var userCode = dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["code"].Value?.ToString() ?? "";
            var phoneNumber = dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["numberPhone"].Value?.ToString() ?? "";
            string idClientCity = dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["idCitiy"].Value?.ToString() ?? "";

            var v = new UserModel { userId = userId, userName = userName, userCode = userCode, phoneNumber = phoneNumber, idClientCity = idClientCity };

            ChangedUserData changedUserData = new ChangedUserData(v);
            changedUserData.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            search();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete a = MySQL.delete("DELETE FROM users WHERE id = " + dataGridViewUser.Rows[dataGridViewUser.CurrentRow.Index].Cells["id"].Value.ToString());

            if(a.execute && a.affectedRowCount > 0)
            {
                MessageBox.Show("Данные успешно удалены");

                search();
                getCities();

                return;
            }

            MessageBox.Show(a.message);
        }

        private void AddUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddUser users = new AddUser();
            users.ShowDialog();
        }
    }
}
