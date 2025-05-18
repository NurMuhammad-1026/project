using Bee.MySQL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormBorrowedAndRepaid
{
    public partial class ChangeBorrowedDataForm: Form
    {
        public Action ResetRepaidForm { get; set; }
        public Action ResetMainForm { get; set; }

        private string idLoad = null;
        private string summa = null;
        private string date = null;
        private string comment = null;

        public ChangeBorrowedDataForm(string idLoad, string summa, string date, string comment)
        {
            InitializeComponent();

            this.idLoad = idLoad;
            this.summa = summa;
            this.date = date;
            this.comment = comment;
        }

        private void ChangeBorrowedForm_Load(object sender, EventArgs e)
        {
            textBoxSumma.Text = summa;
            dateTimePickerDateBorrowed.Value = Convert.ToDateTime(date);
          
            textBoxComment.Text = comment;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (summa.Trim() == textBoxSumma.Text.Trim() && Convert.ToDateTime(date) == dateTimePickerDateBorrowed.Value && comment.Trim() == textBoxComment.Text.Trim())
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
                            {"@date",  dateTimePickerDateBorrowed.Value},
                            {"@comment", textBoxComment.Text.Trim()}
                        };

                        var a = MySQL.update("UPDATE borrowed_load SET summa = @summa, date = @date, comment = @comment WHERE id = @id", data);

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
