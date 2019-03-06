using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2
{
    public partial class MainForm : Form
    {
        private Service _service;
        
        public MainForm()
        {
            this._service = new Service();
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await _service.GetAsync(textBox1.Text, 1);


        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await _service.GetAsync(textBox2.Text, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _service.Cancel(1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _service.Cancel(2);
        }
    }
}
