using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bee.MySQL;
using Bee.MySQL.Models;

namespace FormBorrowedAndRepaid
{
    public partial class rasxod : Form
    {
        public rasxod()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            search();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker1.Enabled = checkBox1.Checked;

            if (checkBox1.Checked || checkBox2.Checked)
            {
                search();
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            search();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Enabled = checkBox2.Checked;

            if (checkBox1.Checked || checkBox2.Checked)
            {
                search();
            }
        }

        private void rasxod_Activated(object sender, EventArgs e)
        {
            if (this.Visible && !this.IsDisposed)
            {
                search();
            }
        }

        private void search()
        {
            try
            {
                dataGridView1.Rows.Clear();

                double summa = 0;
                string zapros = "SELECT * FROM rasxod1 WHERE 1 = 1";
                Dictionary<string, object> d = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(textBox2.Text.Trim()))
                {
                    zapros += " AND info LIKE @searchInfo";

                    d.Add("@searchInfo", "%" + textBox2.Text.Trim().ToUpper() + "%");
                }

                if (checkBox1.Checked && checkBox2.Checked)
                {
                    zapros += " AND sana BETWEEN @toDate AND @doDate";

                    d.Add("@toDate", dateTimePicker1.Value.ToString("yyyy-MM-dd"));
                    d.Add("@doDate", dateTimePicker2.Value.ToString("yyyy-MM-dd"));
                }

                Select a = MySQL.select(zapros, d);

                if (a.execute && a.data.Count > 0)
                {
                    foreach (var row in a.data)
                    {
                        dataGridView1.Rows.Add(row["info"], row["summa"], Convert.ToDateTime(row["sana"]).ToString("yyyy-MM-dd"), row["comment"]);

                        summa += Convert.ToDouble(row["summa"]);
                    }
                }

               label4.Text = summa.ToString();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            search();
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRasxod addRasxod = new AddRasxod();
            addRasxod.ShowDialog();
        }
    }
}
