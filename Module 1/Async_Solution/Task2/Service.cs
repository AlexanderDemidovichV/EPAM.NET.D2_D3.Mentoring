using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task2
{
    public class Service
    {
        private CancellationTokenSource cancellationTokenSource1;
        private CancellationTokenSource cancellationTokenSource2;

        public async Task GetAsync(string text, int buttonId)
        {
            try
            {
                if (buttonId == 1)
                {
                    cancellationTokenSource1 = new CancellationTokenSource();
                    var result = await GetAsync(text, cancellationTokenSource1);
                    MessageBox.Show($"{result}");
                }
                else
                {
                    cancellationTokenSource2 = new CancellationTokenSource();
                    var result = await GetAsync(text, cancellationTokenSource2);
                    MessageBox.Show($"{result}");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Something went wrong.");
            }
        }

        private async Task<string> GetAsync(string text, CancellationTokenSource cancellationTokenSource)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.GetAsync(text,
                    cancellationTokenSource.Token);
                return result.ToString();
            }
            
        }

        public void Cancel(int buttonId)
        {
            if (buttonId == 1)
            {
                cancellationTokenSource1.Cancel();
            }
            else
            {
                cancellationTokenSource2.Cancel();
            }
            
        }
    }
}
