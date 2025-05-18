using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;

namespace FormBorrowedAndRepaid
{
    public partial class AddRepaidForm: Form
    {
        public AddRepaidForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
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
                bool k = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Cells["code"].Value == null)
                    {
                        row.Cells["code"].Style.BackColor = Color.MistyRose;
                        k = true;
                    }

                    if (row.Cells["summa"].Value == null)
                    {
                        row.Cells["summa"].Style.BackColor = Color.MistyRose;
                        k = true;
                    }
                }

                if (k == true)
                {
                    MessageBox.Show("Запольните всех поля");
                    return;
                }

                CheckCode();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void CheckCode()
        {
            try
            {
                bool k = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (CheckUserCode(row.Cells["code"].Value.ToString()) == null)
                    {
                        row.Cells["code"].Style.BackColor = Color.MistyRose;
                        k = true;
                    }
                }

                if (k)
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
                bool k = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    Dictionary<string, object> v = new Dictionary<string, object>
                    {
                        { "@summa", row.Cells["summa"].Value},
                        { "date", dateTimePicker1.Value.ToString("yyyy-MM-dd")},
                        { "@comment", row.Cells["comment"].Value},
                        { "@code", row.Cells["code"].Value }
                    };

                    var insert = MySQL.insert("INSERT INTO repaid_load (user_id, summa, date, comment) VALUES ((SELECT id FROM users WHERE code = @code), @summa, @date, @comment)", v);

                    if (insert.execute && insert.affectedRowCount > 0)
                    {
                        dataGridView1.Rows.RemoveAt(row.Index);

                        continue;
                    }

                    k = true;
                }

                if (k == true)
                {
                    MessageBox.Show("Ошибка при записи в базу данных.\n Повторите с начала!");
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы хотите выход от страница?", "Предупреждение", MessageBoxButtons.OKCancel);

                if (result == DialogResult.OK)
                {
                    this.Close();
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
    
        private void dataGridView1_CellValueChanged_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                if (dataGridView1.Rows[e.RowIndex].Cells["summa"].Value != null)
                {
                    Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["summa"].Value);
                }

                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor == Color.MistyRose)
                {
                    if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
                    }
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);

                dataGridView1.Rows[e.RowIndex].Cells["summa"].Value = null;
            }
        }
    }
}
