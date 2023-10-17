using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Task_Manager
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer aTimer = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                richTextBox1.Text += $"Name: {process.ProcessName} Id:{process.Id}\n";
            }
        }
        private void SetTimer()
        {
            aTimer = new System.Timers.Timer(Convert.ToInt32(textBox1.Text));
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Action safeWrite = richTextBox1.Clear;
            richTextBox1.Invoke(safeWrite);
            
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {

                Action safeWrite1 = delegate { Wr(process.ProcessName, Convert.ToString(process.Id)); };
                richTextBox1.Invoke(safeWrite1);

            }
            Thread.Sleep(Convert.ToInt32(textBox1.Text));
        }
        private void Wr(string PN, string ID)
        {
            richTextBox1.Text += $"Name: {PN} Id:{ID}\n";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aTimer.Enabled)
            {
                aTimer.Stop();
            }
            SetTimer();
        }
    }
}
