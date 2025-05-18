using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL.Models;
using Bee.MySQL;
using static FormBorrowedAndRepaid.ChangedUserData;
using WindowsFormsApp1;

namespace FormBorrowedAndRepaid
{
    public partial class AddUser : Form
    {
        private object comboBox1;

        public AddUser()
        {
            InitializeComponent();
        }        

        private void getCities()
        {
            try
            {
                Select a = MySQL.select("SELECT * FROM city WHERE country_id = 1");

                if (a.execute && a.data != null)
                {
                    comboBoxNameCtiy.Items.Clear();

                    foreach (var line in a.data)
                    {
                        comboBoxNameCtiy.Items.Add(new ComboBoxItem(Convert.ToInt32(line["id"]), line["name"].ToString()));
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void AddUser_Activated(object sender, EventArgs e)
        {
            getCities();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            CheckData();
        }

        private void CheckData()
        {
            bool k = false;

            if (textBoxUserCode.Text.Trim() == "")
            {
                textBoxUserCode.BackColor = Color.LightBlue;
                k = true;
            }

            if (comboBoxNameCtiy.Text == "")
            {
                k = true;                
            }

            if (k == true)
            {
                MessageBox.Show("Заполните поля!");
                return;
            }

            InsertData();
        }

        void InsertData()
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@name", textBoxUserName.Text.Trim()},
                    { "code", textBoxUserCode.Text.Trim()},
                    { "@city",(comboBoxNameCtiy.SelectedItem as ComboBoxItem).id},
                    { "@phone", textBoxNumberPhone.Text.Trim()}
                };

                var insert = MySQL.insert("INSERT INTO users (city_id, code, name, phone_number) VALUES (@city, @code, @name, @phone)", parameters);

                if (insert.execute && insert.affectedRowCount > 0)
                {
                    MessageBox.Show("Данные сохранены!");

                    textBoxUserCode.Text = "";
                    textBoxUserName.Text = "";
                    textBoxNumberPhone.Text = "";
                    comboBoxNameCtiy.Text = "";
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void textBoxUserCode_Leave(object sender, EventArgs e)
        {            
            var v = MySQL.select("SELECT * FROM users WHERE code = " + textBoxUserCode.Text.Trim());

            if (v.execute && v.data.Count > 0)
            {
                MessageBox.Show("Это код уже есть в база!");

                textBoxUserCode.Text = "";

                return;
            }
        }

        private void textBoxUserCode_Enter(object sender, EventArgs e)
        {
            textBoxUserCode.BackColor = Color.White;
        }

        private void buttonClouse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы хотите выход от страница?", "Предупреждение", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    this.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}