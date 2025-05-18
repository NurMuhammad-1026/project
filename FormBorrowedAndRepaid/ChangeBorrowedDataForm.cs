using Bee.MySQL;
using FormBorrowedAndRepaid.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FormBorrowedAndRepaid
{
    public partial class ChangeBorrowedDataForm: Form
    {
        public Action ResetBorrowedForm { get; set;}
        private ChangedDataLoad changedDataLoad;

        public ChangeBorrowedDataForm(ChangedDataLoad changedDataLoad)
        {
            InitializeComponent();
            this.changedDataLoad = changedDataLoad;
        }

        private void ChangeBorrowedForm_Load(object sender, EventArgs e)
        {
            textBoxSumma.Text = changedDataLoad.summaUser;
            dateTimePickerDateBorrowed.Value = Convert.ToDateTime(changedDataLoad.dateRepaid);
          
            textBoxComment.Text = changedDataLoad.commentLoad;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (changedDataLoad.summaUser.Trim() == textBoxSumma.Text.Trim() && Convert.ToDateTime(changedDataLoad.dateRepaid) == dateTimePickerDateBorrowed.Value && changedDataLoad.commentLoad.Trim() == textBoxComment.Text.Trim())
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
                            {"@date",  dateTimePickerDateBorrowed.Value},
                            {"@comment", textBoxComment.Text.Trim()}
                        };

                        var a = MySQL.update("UPDATE borrowed_load SET summa = @summa, date = @date, comment = @comment WHERE id = @id", data);

                        if(a.execute && a.affectedRowCount > 0)
                        {
                            MessageBox.Show("Дата успешно изменена");

                            ResetBorrowedForm();

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
