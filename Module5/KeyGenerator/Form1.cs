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
            var evalA = new eval_a();
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            evalA.a = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());

            var selector1 = new Func<byte, int, int>(evalA.method_a);
            return addressBytes.Select(selector1).Select(a02 => a02 * 10).ToArray();
        }

        private sealed class eval_a
        {
            public byte[] a;

            private int[] b;

            [NonSerialized]
            private string eval_c;

            public int method_a(byte A_0, int A_1)
            {
                return A_0 ^ a[A_1];
            }
        }
    }
}
