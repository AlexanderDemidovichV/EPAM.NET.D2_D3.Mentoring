using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            var text = this.textBox1.Text;
            var httpClient = new HttpClient();
            try
            {
                var result = await httpClient.GetAsync(text);
                this.richTextBox1.Text = result.Content.ToString();
            }
            catch (Exception exception)
            {
                this.richTextBox1.Text = exception.Message;
            }
            
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var text = this.textBox2.Text;
            var httpClient = new HttpClient();
            try
            {
                var result = await httpClient.GetAsync(text);
                this.richTextBox2.Text = result.Content.ToString();
            }
            catch (Exception exception)
            {
                this.richTextBox1.Text = exception.Message;
            }
        }
    }
}
