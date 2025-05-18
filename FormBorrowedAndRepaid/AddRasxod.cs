using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;


namespace FormBorrowedAndRepaid
{
    public partial class AddRasxod : Form
    {
        public AddRasxod()
        {
            InitializeComponent();
        }

        //button save
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool k = false;

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Cells["info"].Value == null)
                    {
                        row.Cells["info"].Style.BackColor = Color.MistyRose;
                        k = true;
                    }

                    if (row.Cells["summa"].Value == null)
                    {
                        row.Cells["summa"].Style.BackColor = Color.MistyRose;
                        k = true;
                    }
                }

                if (k)
                {
                    MessageBox.Show("Запольните всех поля");
                    return;
                }

                insert();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "Error");
            }
        }       

        void insert()
        {
            Insert a = new Insert();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@sana", dateTimePicker1.Value.ToString("yyyy-MM-dd") },
                    { "@info", row.Cells["info"].Value},
                    { "@comment", row.Cells["comment"].Value},
                    { "@summa", row.Cells["summa"].Value},
                };

                a = MySQL.insert("INSERT INTO rasxod1 (sana, info, comment, summa) VALUES (@sana,@info,@comment,@summa);", parameters);
            }

            if (a.execute && a.affectedRowCount > 0)
            {
                MessageBox.Show("Данные успешно сохранены!");

                dataGridView1.Rows.Clear();

                return;
            }

            MessageBox.Show(a.message);
        }

        //Otmaena button
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы хотите выйти со страницы?", "Предупреждение", MessageBoxButtons.OKCancel);

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
