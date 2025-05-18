using System;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;
using FormBorrowedAndRepaid.Models;

namespace FormBorrowedAndRepaid
{
    public partial class BorrowedForm: Form
    {
        private UserModel userModel;

        public BorrowedForm(UserModel userModel)
        {
            InitializeComponent();

            this.userModel = userModel;
        }

        private void BorrowedForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void changedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string idRepaidLoad = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
                string summaUser = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
                string dateRepaid = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["date"].Value.ToString();
                string commentLoad = dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["comment"].Value?.ToString() ?? "";

                var v = new ChangedDataLoad{ idRepaidLoad = idRepaidLoad, summaUser = summaUser, dateRepaid = dateRepaid, commentLoad = commentLoad };
                
                ChangeBorrowedDataForm changeBorrowedDataForm = new ChangeBorrowedDataForm(v);
                changeBorrowedDataForm.ResetBorrowedForm = ResetForm;
                changeBorrowedDataForm.ShowDialog();
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
                if (dataGridBorrowedLoad.CurrentRow == null)
                {
                    MessageBox.Show("Пожалуйста, выберите строку для удаления");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту строку данных? ", "Предупреждение", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    Delete a = MySQL.delete("DELETE FROM borrowed_load WHERE id = " + dataGridBorrowedLoad.Rows[dataGridBorrowedLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString());

                    if (a.execute && a.affectedRowCount > 0)
                    {
                        MessageBox.Show("Данные успешно удалены");

                        ResetForm();

                        return;
                    }

                    MessageBox.Show(a.message);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void ResetForm()
        {
            try
            {
                labelUserName.Text = userModel.userName;
                labelUserCode.Text = userModel.userCode;

                Select a = MySQL.select("select * from borrowed_load where user_id = " + userModel.userId);

                if (a.execute && a.data != null)
                {
                    dataGridBorrowedLoad.Rows.Clear();

                    foreach (var row in a.data)
                    {
                        dataGridBorrowedLoad.Rows.Add(row["id"], row["user_id"], row["summa"], ((DateTime)row["date"]).ToString("yyyy-MM-dd"), row["comment"]);
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
