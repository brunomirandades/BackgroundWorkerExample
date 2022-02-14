using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackgroundWorkerExample
{
    public partial class principalForm : Form
    {
        public principalForm()
        {
            InitializeComponent();
        }

        struct DataParameter
        {
            public int Process { get; set; }
            public int Delay { get; set; }
        }

        private DataParameter _inputParameter;

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int process = ((DataParameter)e.Argument).Process;
            int delay = ((DataParameter)e.Argument).Delay;
            int index = 1;

            try
            {
                for (int i = 0; i < process; i++)
                {
                    if (!backgroundWorker.CancellationPending)
                    {
                        backgroundWorker.ReportProgress(index++ * 100 / process, string.Format("Process data {0}", i));
                        Thread.Sleep(delay); //simulating processing
                        //Add your code here
                        //...
                    }
                }
            }
            catch (Exception ex)
            {
                backgroundWorker.CancelAsync();
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
                return;
            }

        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            //processingLabel.Text = string.Format("Processing... {0}%", e.ProgressPercentage);
            processingLabel.Text = $"Processing... {e.ProgressPercentage}%";
            progressBar.Update();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show("Process was cancelled", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Process has been completed", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
            progressBar.Value = 0;
            processingLabel.Text = "Processing... 0%";
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                _inputParameter.Delay = 10;
                _inputParameter.Process = 1200;
                backgroundWorker.RunWorkerAsync(_inputParameter);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
                backgroundWorker.CancelAsync();
        }
    }
}
