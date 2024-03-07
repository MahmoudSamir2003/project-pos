using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // عرض شاشة المبيعات عند تحميل الفورم
            SalesForm salesForm = new SalesForm();
            salesForm.ShowDialog(); // استخدم ShowDialog بدلاً من Show لعرض الشاشة بشكل مودال
        }
    }
}
