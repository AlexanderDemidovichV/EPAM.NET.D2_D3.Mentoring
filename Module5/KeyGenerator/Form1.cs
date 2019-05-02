using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Join("-", hello().Select(number => number.ToString()));
        }

        private int[] hello()
        {
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            var binaryDate = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            
            return addressBytes.Select((b, a) => b ^ binaryDate[a]).Select(i => i * 10).ToArray();
        }
    }
}
