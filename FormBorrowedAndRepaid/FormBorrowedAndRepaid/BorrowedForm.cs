using System;
using System.Windows.Forms;
using Bee.MySQL.Models;
using Bee.MySQL;

namespace FormBorrowedAndRepaid
{
    public partial class BorrowedForm: Form
    {
        public Action ResetMainForm { get; set; }

        private string idRepaidLoad = null;
        private string summaUser = null;
        private string dateRepaid = null;
        private string commentLoad = null;

        private string id = null;
        private string code = null;
        private string name = null;

        public BorrowedForm(string userId, string userCode, string userName)
        {
            InitializeComponent();

            id = userId;
            code = userCode;
            name = userName;
        }

        private void BorrowedForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            try
            {
                labelUserName.Text = name;
                labelUserCode.Text = code;

                Select a = MySQL.select("select * from borrowed_load where user_id = " + id);

                if (a.execute && a.data != null)
                {
                    dataGridBorrowedLoad.Rows.Clear();

                    foreach (var row in a.data)
                    {
                        dataGridBorrowedLoad.Rows.Add(row["id"], id, row["summa"], ((DateTime)row["date"]).ToString("yyyy-MM-dd"), row["comment"]);
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void changedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (idRepaidLoad != null && summaUser != null && dateRepaid != null && commentLoad != null)
            {
                ChangeBorrowedDataForm changeBorrowedDataForm = new ChangeBorrowedDataForm(idRepaidLoad, summaUser, dateRepaid, commentLoad);
                changeBorrowedDataForm.ResetRepaidForm = ResetForm;
                changeBorrowedDataForm.ResetMainForm = this.ResetMainForm;
                changeBorrowedDataForm.ShowDialog();
            }
        }

        private void dataGridBorrowedLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["comment"].Value == null)
                {
                    idRepaidLoad = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
                    summaUser = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
                    dateRepaid = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["date"].Value.ToString();
                    commentLoad = "";

                    return;
                }

                idRepaidLoad = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
                summaUser = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
                dateRepaid = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["date"].Value.ToString();
                commentLoad = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["comment"].Value.ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту строку данных? ", "Подтверждение", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    Delete a = MySQL.delete("DELETE FROM borrowed_load WHERE id = " + dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString());

                    ResetForm();
                    ResetMainForm();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
