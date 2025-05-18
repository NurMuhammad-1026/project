using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;

namespace FormBorrowedAndRepaid
{
    public partial class AddRepaidForm: Form
    {
        public Action ResetmainForm { get; set; }

        public AddRepaidForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы хотите сохранить эту информацию?", "Предупреждение", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    CheckData();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void CheckData()
        {
            try
            {
                var label = false;
                var codeUserTest = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Cells["code"].Value == null)
                    {
                        row.Cells["code"].Style.BackColor = Color.MistyRose;
                        label = true;
                    }
                    else
                    {
                        if (CheckUserCode(row.Cells["code"].Value.ToString()) == null)
                        {
                            row.Cells["code"].Style.BackColor = Color.DarkRed;
                            codeUserTest = true;
                        }
                    }

                    if (row.Cells["summa"].Value == null)
                    {
                        row.Cells["summa"].Style.BackColor = Color.MistyRose;
                        label = true;
                    }
                }

                if (label)
                {
                    MessageBox.Show("Запольните всех поля");
                    return;
                }

                if (codeUserTest)
                {
                    MessageBox.Show("Код не найден");
                    return;
                }

                InsertData();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private string CheckUserCode(string codeUser)
        {
            try
            {
                var v = MySQL.selectValue("SELECT code FROM users WHERE code = @codeUser", new Dictionary<string, object> { { "@codeUser", codeUser } });

                if (v.execute && v.value != null)
                {
                    return v.value.ToString();
                }

                return null;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                return null;
            }
        }

        private void InsertData()
        {
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    var user_id = MySQL.selectValue("SELECT id FROM users WHERE code = @code", new Dictionary<string, object> { { "@code", row.Cells["code"].Value } });

                    if (user_id.execute && user_id.value != null)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>
                        {
                            { "@userId", user_id.value},
                            { "@summa", row.Cells["summa"].Value},
                            { "date", dateTimePicker1.Value.ToString("yyyy-MM-dd")},
                            { "@comment", row.Cells["comment"].Value}
                        };

                        var insert = MySQL.insert("INSERT INTO repaid_load (user_id, summa, date, comment) VALUES (@userId, @summa, @date, @comment)", parameters);
                    }
                }

                ResetmainForm();

                dataGridView1.Rows.Clear();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.MistyRose || dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.DarkRed)
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;


                if (e.ColumnIndex == 1)
                {
                    try
                    {
                        if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                        {
                            Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        }
                    }
                    catch (Exception exeption)
                    {
                        MessageBox.Show(exeption.Message);

                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = null;
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
