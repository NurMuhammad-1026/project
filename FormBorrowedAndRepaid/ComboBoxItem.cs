using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ComboBoxItem
    {
        public int id = 0;
        public string title = "";

        public ComboBoxItem(int id, string title)
        {
            this.id = id;
            this.title = title;
        }

        public override string ToString()
        {
            return title;
        }
    }
}