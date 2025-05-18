using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bee.MySQL;

namespace FormBorrowedAndRepaid
{
    public partial class ChangedRepaidDataForm: Form
    {
        public Action ResetRepaidForm { get; set; }
        public Action ResetMainForm { get; set; }

        private string idLoad = null;
        private string summa = null;
        private string date = null;
        private string comment = null;

        public ChangedRepaidDataForm(string idLoad, string summa, string date, string comment)
        {
            InitializeComponent();

            this.idLoad = idLoad;
            this.summa = summa;
            this.date = date;
            this.comment = comment;
        }

        private void ChangedRepaidDataForm_Load(object sender, EventArgs e)
        {
            textBoxSumma.Text = summa;
            dateTimePickerDateRepaid.Value = Convert.ToDateTime(date);
            textBoxComment.Text = comment;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (summa.Trim() == textBoxSumma.Text.Trim() && Convert.ToDateTime(date) == dateTimePickerDateRepaid.Value && comment.Trim() == textBoxComment.Text.Trim())
                {
                    DialogResult dialogResult = MessageBox.Show("Вы не изменили никакой информации! ", "Подтверждение", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Действительно хотите изменит данные таким оброзом! ", "Подтверждение", MessageBoxButtons.OKCancel);

                    if (dialogResult == DialogResult.OK)
                    {
                        var data = new Dictionary<string, object>
                        {
                            {"@id", idLoad},
                            {"@summa",  textBoxSumma.Text.Replace(',', '.').Trim()},
                            {"@date",  dateTimePickerDateRepaid.Value},
                            {"@comment", textBoxComment.Text.Trim()}
                        };

                        var a = MySQL.update("UPDATE repaid_load SET summa = @summa, date = @date, comment = @comment WHERE id = @id", data);

                        ResetRepaidForm();
                        ResetMainForm.Invoke();

                        this.Close();
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void textBoxSumma_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxSumma.Text.Trim() != "")
                {
                    Convert.ToDouble(textBoxSumma.Text);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);

                textBoxSumma.Clear();
            }
        }
    }
}
