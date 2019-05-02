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

namespace Decompiled
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var str = textBox1.Text;
                label1.Text = Eval(str.Split('-')) ? "Correct" : "Incorrect";
            }
            catch (Exception exception)
            {
                label1.Text = "Incorrect";
            }
            
        }

        private bool Eval(string[] A_0_1)
        {
            var eval_a = new eval_a();
            var networkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            eval_a.a = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());

            Func<byte, int, int> selector1 = new Func<byte, int, int>(eval_a.method_a);
            int[] array = ((IEnumerable<byte>)addressBytes).Select<byte, int>(selector1).Select<int, int>((Func<int, int>)(A_0_2 =>
            {
                if (A_0_2 <= 999)
                {
                    return A_0_2 * 10;
                }
                return A_0_2;
            })).ToArray<int>();

            eval_a.b = ((IEnumerable<string>)A_0_1).Select<string, int>(new Func<string, int>(int.Parse)).ToArray<int>();
            Func<int, int, int> selector2 = new Func<int, int, int>(eval_a.method_a);
            var r = ((IEnumerable<int>)array).Select<int, int>(selector2).All<int>((Func<int, bool>)(A_0_2 =>
            {

                return A_0_2 == 0;

            }));
            return r;
        }

        private sealed class eval_a
        {
            public byte[] a;

            public int[] b;

            [NonSerialized]
            private string eval_c;

            internal int method_a(byte A_0, int A_1)
            {
                return A_0 ^ a[A_1];
            }

            internal int method_a(int A_0, int A_1)
            {
                return A_0 - b[A_1];
            }
        }
    }
}
