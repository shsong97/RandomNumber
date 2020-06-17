using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace RandomNumberApp
{
    public partial class Form1 : Form
    {
        Speech spk = new Speech();
        int minNumber = 0;
        int maxNumber = 0;
        int delayTime = 3;
        bool[] valueCheck;
        List<int> valueList = new List<int>();
        bool stopFlag = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Speech spk = new Speech();
            spk.AsyncTextSpeak("빙고게임을 시작합니다.");
            Thread.Sleep(5000);
            stopFlag = false;
            delayTime = int.Parse(txtDelayTime.Text);
            initRandomNumber();
            bgWork.RunWorkerAsync(maxNumber);
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            stopFlag = true;
            initRandomNumber();
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            Thread.Sleep(2000);
            Speech spk = new Speech();
            spk.AsyncTextSpeak("빙고게임이 종료되었습니다.");

        }

        private void callRandomNumber()
        {
            var rand = new Random();
            int remainCount = maxNumber - valueList.Count();
            int randValue = rand.Next(0, remainCount);
            int count = -1;
            int index = -1;
            while(true)
            {
                index++;
                if(index>=maxNumber)
                {
                    stopFlag = true;
                    return;
                }
                if(!valueCheck[index])
                {
                    count++;
                    if(randValue==count)
                    {
                        valueCheck[index] = true;
                        valueList.Add(index+1);
                        Speech spk = new Speech();
                        spk.AsyncTextSpeak((index + 1).ToString());
                        return;
                    }
                }
            }
        }

        private void initRandomNumber()
        {
            minNumber = int.Parse(txtFromNumber.Text);
            maxNumber = int.Parse(txtToNumber.Text);
            valueCheck = new bool[maxNumber];
            valueList = new List<int>();
        }

        private string printNumber()
        {
            string message = "";
            foreach (var i in valueList)
            {
                message += i.ToString() + " ";
            }
            return message;
        }

        private void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!stopFlag)
            {
                callRandomNumber();
                bgWork.ReportProgress(valueList.Count());
                if (bgWork.CancellationPending)
                {
                    stopFlag = true;
                }
                if (stopFlag)
                {
                    e.Cancel = true;
                    bgWork.ReportProgress(0);
                    return;
                }
                Thread.Sleep(delayTime * 1000);
            }
        }

        private void bgWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lblNumber.Text = valueList.Last().ToString();
            lblValueList.Text = printNumber();
            
        }

        private void bgWork_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(bgWork.IsBusy)
            {
                bgWork.CancelAsync();
                stopFlag = true;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show(printNumber());
        }
    }
}
