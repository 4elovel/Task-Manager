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
                listBox1.Items.Add($"Name: {process.ProcessName} Id:{process.Id}");
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
            
            Action safeWrite = listBox1.Items.Clear;
            listBox1.Invoke(safeWrite);
            
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {

                Action safeWrite1 = delegate { Wr(process.ProcessName, Convert.ToString(process.Id)); };
                listBox1.Invoke(safeWrite1);

            }
            Thread.Sleep(Convert.ToInt32(textBox1.Text));
        }
        private void Wr(string PN, string ID)
        {
            listBox1.Items.Add($"Name: {PN} Id:{ID}");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aTimer.Enabled)
            {
                aTimer.Stop();
            }
            SetTimer();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string id = listBox1.Text;
            string name = id.Substring(id.IndexOf("Name:") + 6, id.IndexOf("Id:") - 7);
            id = id.Substring(id.IndexOf("Id:")+3);

            var currentProcess = Process.GetProcessById(Convert.ToInt32(id));
            


            System.Text.StringBuilder sb = new System.Text.StringBuilder();



            sb.AppendLine("Process information");
            sb.AppendLine("-------------------");
            sb.AppendLine("CPU time");
            sb.AppendLine(string.Format("\tTotal       {0}",
                currentProcess.TotalProcessorTime));
            sb.AppendLine(string.Format("\tUser        {0}",
                currentProcess.UserProcessorTime));
            sb.AppendLine(string.Format("\tPrivileged  {0}",
                currentProcess.PrivilegedProcessorTime));
            sb.AppendLine("Memory usage");
            sb.AppendLine(string.Format("\tCurrent     {0:N0} B", currentProcess.WorkingSet64));
            sb.AppendLine(string.Format("\tPeak        {0:N0} B", currentProcess.PeakWorkingSet64));
            sb.AppendLine(string.Format("Active threads      {0:N0}", currentProcess.Threads.Count));
            var n_processes = Process.GetProcessesByName(name);
            int counter = 0;
            foreach ( var pr in n_processes)
            {
                counter++;
            }
            sb.AppendLine(string.Format("Process name coppies      {0:N0}", counter));
            counter = 0;
            foreach (var pr in Process.GetProcesses())
            {
                counter++;
            }
            sb.AppendLine(string.Format("Total processes      {0:N0}", counter));
            label2.Text = sb.ToString();
        }
    }
}
