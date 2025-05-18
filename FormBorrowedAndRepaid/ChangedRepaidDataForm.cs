using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bee.MySQL;
using FormBorrowedAndRepaid.Models;

namespace FormBorrowedAndRepaid
{
    public partial class ChangedRepaidDataForm: Form
    {
        public Action ResetRepaidForm { get; set; }
        private ChangedDataLoad changedDataLoad;

        public ChangedRepaidDataForm(ChangedDataLoad changedDataLoad)
        {
            InitializeComponent();
            this.changedDataLoad = changedDataLoad;
        }

        private void ChangedRepaidDataForm_Load(object sender, EventArgs e)
        {
            textBoxSumma.Text = changedDataLoad.summaUser;
            dateTimePickerDateRepaid.Value = Convert.ToDateTime(changedDataLoad.dateRepaid);
            textBoxComment.Text = changedDataLoad.commentLoad;
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

        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (changedDataLoad.summaUser.Trim() == textBoxSumma.Text.Trim() && Convert.ToDateTime(changedDataLoad.dateRepaid) == dateTimePickerDateRepaid.Value && changedDataLoad.commentLoad.Trim() == textBoxComment.Text.Trim())
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
                            {"@id", changedDataLoad.idRepaidLoad},
                            {"@summa",  textBoxSumma.Text.Replace(',', '.').Trim()},
                            {"@date",  dateTimePickerDateRepaid.Value},
                            {"@comment", textBoxComment.Text.Trim()}
                        };

                        var a = MySQL.update("UPDATE repaid_load SET summa = @summa, date = @date, comment = @comment WHERE id = @id", data);

                        if (a.execute && a.affectedRowCount > 0)
                        {
                            MessageBox.Show("Дата успешно изменена");

                            ResetRepaidForm();

                            this.Close();

                            return;
                        }

                        MessageBox.Show(a.message);
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
