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
        public MainForm()
        {
            InitializeComponent();
        }

        private CancellationTokenSource cancellationTokenSource1;
        private CancellationTokenSource cancellationTokenSource2;

        private async void button1_Click(object sender, EventArgs e)
        {
            var text = this.textBox1.Text;
            var httpClient = new HttpClient();
            try
            {
                cancellationTokenSource1 = new CancellationTokenSource();
                var result = await httpClient.GetAsync(text,
                    cancellationTokenSource1.Token);
                MessageBox.Show($"{result}");
            }
            catch (Exception exception)
            {

            }
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var text = this.textBox2.Text;
            var httpClient = new HttpClient();
            try
            {
                cancellationTokenSource2 = new CancellationTokenSource();
                var result = await httpClient.GetAsync(text,
                    cancellationTokenSource2.Token);
                MessageBox.Show($"{result}");
            }
            catch (Exception exception)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cancellationTokenSource1.Cancel();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            cancellationTokenSource2.Cancel();
        }
    }
}
