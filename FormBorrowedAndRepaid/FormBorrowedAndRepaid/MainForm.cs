using System;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;

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

            MySQL.connectionString = "server=localhost;port=3306;username=root;password=;database=dbcargo;";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            try
            {
                dataGridViewMain.Rows.Clear();

                Select v = MySQL.select("select * from users");

                if (v.execute && v.data != null)
                {
                    foreach (var row in v.data)
                    {
                        var summaBorrowedLoad = MySQL.selectValue("select sum(summa) as sum from borrowed_load where user_id = " + row["id"]);
                        var summaRepaidLoad = MySQL.selectValue("select sum(summa) as sum from repaid_load where user_id = " + row["id"]);

                        if (summaBorrowedLoad.value != null && summaRepaidLoad.value != null)
                        {
                            int rowIndex = dataGridViewMain.Rows.Add(row["id"], row["name"], row["code"], (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value)), row["phone_number"]);

                            if (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value) > 0 && dataGridViewMain.CurrentRow != null)
                            {
                                dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Khaki;
                            }
                            else if (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value) < 0 && dataGridViewMain.CurrentRow != null)
                            {
                                dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.Salmon;
                            }
                            else if (Convert.ToDouble(summaRepaidLoad.value) - Convert.ToDouble(summaBorrowedLoad.value) == 0 && dataGridViewMain.CurrentRow != null)
                            {
                                dataGridViewMain.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                            }
                        }  
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void borrowedLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userId == null)
                return;

            BorrowedForm borrowedForm = new BorrowedForm(userId, userCode, userName);
            borrowedForm.ResetMainForm = this.ResetForm;
            borrowedForm.ShowDialog();
        }

        private void repaidLoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (userId == null)
                return;

            RepaidForm repaidForm = new RepaidForm(userId, userCode, userName);
            repaidForm.ResetMainForm = this.ResetForm;
            repaidForm.ShowDialog();
        }
        
        private void dataGridViewMain_Click_1(object sender, EventArgs e)
        {
            userId = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["id"].Value.ToString();
            userCode = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["code"].Value.ToString();
            userName = dataGridViewMain.Rows[dataGridViewMain.CurrentRow.Index].Cells["name"].Value.ToString();
        }

        private void borrowedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddBorrowedForm addBorrowedForm = new AddBorrowedForm();
            addBorrowedForm.ResetmainForm =this.ResetForm;
            addBorrowedForm.ShowDialog();
        }

        private void repaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRepaidForm addRepaidForm = new AddRepaidForm();
            addRepaidForm.ResetmainForm = this.ResetForm;
            addRepaidForm.ShowDialog();
        }
    }
}
