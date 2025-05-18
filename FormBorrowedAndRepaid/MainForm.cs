using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;
using FormBorrowedAndRepaid.Models;

namespace FormBorrowedAndRepaid
{
    public partial class MainForm: Form
    {
        private string userId = null;
        private string userCode = null;
        private string userName = null;

        public MainForm()
        {
            InitializeComponent();

            MySQL.connectionString = "server=localhost;port=3306;username=root;password=;database=dbcargo;Charset=utf8mb4";
        }

        private void MainForm_Load(object sender, EventArgs e)
        { 
            ResetForm();
        }              

        private void borrowedLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userId == null)
                return;

            var v = new UserModel { userId = userId, userName = userName, userCode = userCode};

            BorrowedForm borrowedForm = new BorrowedForm(v);
            borrowedForm.ShowDialog();
        }

        private void repaidLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userId == null)
                return;

            var v = new UserModel { userId = userId, userName = userName, userCode = userCode };

            RepaidForm repaidForm = new RepaidForm(v);
            repaidForm.ShowDialog();
        }

        private void borrowedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBorrowedForm addBorrowedForm = new AddBorrowedForm();
            addBorrowedForm.ShowDialog();
        }

        private void repaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRepaidForm addRepaidForm = new AddRepaidForm();
            addRepaidForm.ShowDialog();
        }

        private void MainForm_Activated(object sender, EventArgs e)
        {           
            ResetForm();
            Search();
        }
        private void textBoxSearch_TextChanged_1(object sender, EventArgs e)
        {
            Search();
        }

        private void dataGridViewMain_Click(object sender, EventArgs e)
        {
            userId = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["id"].Value.ToString();
            userCode = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["code"].Value.ToString();
            userName = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["name"].Value.ToString();
        }

        private void textBoxSearch_Enter_1(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Trim() == "Поиск")
            {
                textBoxSearch.Clear();
                textBoxSearch.ForeColor = Color.Black;
            }
        }

        private void textBoxSearch_Leave_1(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Trim() == "")
            {
                textBoxSearch.Text = "Поиск";
                textBoxSearch.ForeColor = Color.DimGray;

                ResetForm();
            }
        }

        private void RasxodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rasxod a = new rasxod();
            a.ShowDialog();
        }

        private void UserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users users = new Users();
            users.ShowDialog();
        }

        private void ResetForm()
        {
            try
            {
                double summa = 0;

                dataGridViewMain.Rows.Clear();

                Select v = MySQL.select("select * from users");

                if (v.execute && v.data != null)
                {
                    foreach (var row in v.data)
                    {
                        var summaBorrowedLoad = MySQL.selectValue("select sum(summa) as sum from borrowed_load where user_id = " + row["id"]);
                        var summaRepaidLoad = MySQL.selectValue("select sum(summa) as sum from repaid_load where user_id = " + row["id"]);

                        if (summaBorrowedLoad.value != null || summaRepaidLoad.value != null)
                        {
                            if ((Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)) < 0)
                            {
                                summa += (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value));
                            }

                            int rowIndex = dataGridViewMain.Rows.Add(row["id"],  " " + row["name"], row["code"], (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)), row["phone_number"]);

                            RowColoring(rowIndex, (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)));
                        }
                    }
                }

                label2.Text = summa.ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void RowColoring(int rowIndex, double balance)
        {
            if (dataGridViewMain.Rows.Count > rowIndex)
            {
                if (balance > 0)
                {
                    dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Beige;
                }
                else if (balance < 0)
                {
                    dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LavenderBlush;
                }
                else
                {
                    dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightCyan;
                }
            }
        }

        private void Search()
        {
            try
            {
                if (textBoxSearch.Text.Trim() == "" || textBoxSearch.Text.Trim() == "Поиск")
                {
                    ResetForm();
                    return;
                }

                dataGridViewMain.Rows.Clear();

                Dictionary<string, object> d = new Dictionary<string, object>
                {
                    {"@text","%" + textBoxSearch.Text.Trim() + "%"}
                };

                Select v = MySQL.select("SELECT * FROM users WHERE code LIKE @text", d);

                if (v.execute && v.data != null)
                {
                    foreach (var row in v.data)
                    {
                        var summaBorrowedLoad = MySQL.selectValue("select sum(summa) as sum from borrowed_load where user_id = " + row["id"]);
                        var summaRepaidLoad = MySQL.selectValue("select sum(summa) as sum from repaid_load where user_id = " + row["id"]);

                        if (summaBorrowedLoad.value != null || summaRepaidLoad.value != null)
                        {
                            int rowIndex = dataGridViewMain.Rows.Add(row["id"], row["name"], row["code"], (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)), row["phone_number"]);

                            RowColoring(rowIndex, (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)));
                        }
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
