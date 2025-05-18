using System;
using System.Collections.Generic;
using Bee.MySQL;
using System.Windows.Forms;
using Bee.MySQL.Models;
using FormBorrowedAndRepaid.Models;
using WindowsFormsApp1;

namespace FormBorrowedAndRepaid
{
    public partial class ChangedUserData: Form
    {
        private UserModel userModel;

        public ChangedUserData()
        {
        }

        public ChangedUserData(UserModel userModel)
        {
            InitializeComponent();

            this.userModel = userModel;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxUserCode.Text == "") 
            {
                MessageBox.Show("Заполните поля!");
                return;
            }

            insert();
        }

        private void insert()
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Действительно хотите изменит данные таким оброзом! ", "Подтверждение", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"@id", userModel.userId ?? " "},
                        {"@userName",  textBoxUserName.Text ?? " "},
                        {"@userCode",  textBoxUserCode.Text},
                        {"@phoneNumber", textBoxNumberPhone.Text ?? ""},
                        {"@nameClientCityId", (comboBoxNameCtiy.SelectedItem as ComboBoxItem).id}
                    };

                    var v = MySQL.update("UPDATE users SET name = @userName, code = @userCode, phone_number = @phoneNumber, city_id = @nameClientCityId WHERE id = @id", data);

                    if (v.execute && v.affectedRowCount > 0)
                    {
                        MessageBox.Show("Дата успешно изменена");

                        this.Close();

                        return;
                    }

                    MessageBox.Show(v.message);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ChangedUserData_Load(object sender, EventArgs e)
        {
            textBoxUserName.Text = userModel.userName;
            textBoxUserCode.Text = userModel.userCode;
            textBoxNumberPhone.Text = userModel.phoneNumber;

            getCities();
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

                        if (line["id"].ToString().Trim() == userModel.idClientCity.Trim())
                            comboBoxNameCtiy.Text = line["name"].ToString();
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
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
