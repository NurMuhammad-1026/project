using System;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FormBorrowedAndRepaid
{
    public partial class RepaidForm: Form
    {
        public Action ResetMainForm { get; set; }

        private string idRepaidLoad = null;
        private string summaUser = null;
        private string dateRepaid = null;
        private string commentLoad = null;

        private string id = null;
        private string code = null;
        private string name = null;

        public RepaidForm(string userId, string userCode, string userName)
        {
            InitializeComponent();

            id = userId;
            code = userCode;
            name = userName;
        }

        private void RepaidForm_Load(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            try
            {
                labelUserName.Text = name;
                labelUserCode.Text = code;

                Select a = MySQL.select("select * from repaid_load where user_id = " + id);

                if (a.execute && a.data != null)
                {
                    dataGridRepaidLoad.Rows.Clear();

                    foreach (var row in a.data)
                    {
                        dataGridRepaidLoad.Rows.Add(row["id"], id, row["summa"], ((DateTime)row["date"]).ToString("dd-MM-yyyy"), row["comment"]);
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
            if(idRepaidLoad != null && summaUser != null && dateRepaid != null && commentLoad != null)
            {
                ChangedRepaidDataForm changedRepaidDataForm = new ChangedRepaidDataForm(idRepaidLoad, summaUser, dateRepaid, commentLoad);
                changedRepaidDataForm.ResetRepaidForm = ResetForm;
                changedRepaidDataForm.ResetMainForm = this.ResetMainForm;
                changedRepaidDataForm.ShowDialog();
            }
        }

        private void dataGridRepaidLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["comment"].Value == null)
                {
                    idRepaidLoad = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
                    summaUser = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
                    dateRepaid = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["date"].Value.ToString();
                    commentLoad = "";

                    return;
                }

                idRepaidLoad = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString();
                summaUser = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["summa"].Value.ToString();
                dateRepaid = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["date"].Value.ToString();
                commentLoad = dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["comment"].Value.ToString();
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
                    Delete a = MySQL.delete("DELETE FROM repaid_load WHERE id = " + dataGridRepaidLoad.Rows[dataGridRepaidLoad.CurrentRow.Index].Cells["idRepaid"].Value.ToString());

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
