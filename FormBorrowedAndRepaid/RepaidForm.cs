using System;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;
using FormBorrowedAndRepaid.Models;
    
namespace FormBorrowedAndRepaid
{
    public partial class RepaidForm: Form
    {
        private UserModel userModel;

        public RepaidForm(UserModel userModel)
        {
            InitializeComponent();

            this.userModel = userModel;
        }

        private void RepaidForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void changedDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string idRepaidLoad = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
            string summaUser = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
            string dateRepaid = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["date"].Value.ToString();
            string commentLoad = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["comment"].Value?.ToString() ?? "";

            var v = new ChangedDataLoad { idRepaidLoad = idRepaidLoad, summaUser = summaUser, dateRepaid = dateRepaid, commentLoad = commentLoad };

            ChangedRepaidDataForm changedRepaidDataForm = new ChangedRepaidDataForm(v);
            changedRepaidDataForm.ResetRepaidForm = ResetForm;
            changedRepaidDataForm.ShowDialog();  
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridRepaidLoad.CurrentRow == null)
                {
                    MessageBox.Show("Пожалуйста, выберите строку для удаления");
                    return;
                }

                DialogResult dialogResult = MessageBox.Show("Вы уверены, что хотите удалить эту строку данных? ", "Предупреждение", MessageBoxButtons.OKCancel);

                if (dialogResult == DialogResult.OK)
                {
                    Delete a = MySQL.delete("DELETE FROM repaid_load WHERE id = " + dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString());

                    if(a.execute && a.affectedRowCount > 0)
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

                Select a = MySQL.select("select * from repaid_load where user_id = " + userModel.userId);

                if (a.execute && a.data != null)
                {
                    dataGridRepaidLoad.Rows.Clear();

                    foreach (var row in a.data)
                    {
                        dataGridRepaidLoad.Rows.Add(row["id"], row["user_id"], row["summa"], ((DateTime)row["date"]).ToString("yyyy-MM-dd"), row["comment"]);
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
