using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private Thread Saver;
        private bool Saving = false;
        private int Interval;
        public Form1()
        {
            InitializeComponent();
            Saver = new Thread(new ThreadStart(AutoSave));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = fb.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = fb.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel2.Text == "Running")
            {
                toolStripStatusLabel2.Text = "Not Running";
                button3.Text = "Start";
                Saving = false;
            }
            else
            {
                toolStripStatusLabel2.Text = "Running";
                button3.Text = "Stop";
                if (int.TryParse(textBox3.Text, out Interval))
                {
                    if (Directory.Exists(textBox2.Text))
                    {
                        if (Directory.Exists(textBox1.Text))
                        {
                            Saving = true;
                            Saver.Start();
                        }
                        else
                        {
                            MessageBox.Show("The source file location you specified does not exist. Please check the location and try again.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The directory path you specified to save the files to does not exist. Please check the location and try again.");
                    }
                }
                else
                {
                    MessageBox.Show("Please set a valid interval in minutes.");
                }
            }
        }

        private void AutoSave()
        {
            while (Saving)
            {
                CopyAll(new DirectoryInfo(textBox1.Text), new DirectoryInfo(textBox2.Text + "//" + DateToFilePath()));
                Thread.Sleep(Interval * 60000);
            }
        }
        private string DateToFilePath()
        {
            return DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + " " + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second;
        }
        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (Directory.Exists(target.FullName) == false) { Directory.CreateDirectory(target.FullName); }
            foreach (FileInfo fi in source.GetFiles()) { fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true); }
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
