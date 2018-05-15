using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWinform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
         
            
        }

        //声明 并发队列处理

        ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void ConcurrenTest()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
         
        }
    }

    class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public int SellPrice { get; set; }
    }

}
