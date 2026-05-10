using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace _1121538_徐霈綺_WAV音效檔播放器
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);

        private string wavFilePath = "";
        private bool isPaused = false;
        private int currentVolume = 1000;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "WAV 檔案 (*.wav)|*.wav|所有檔案 (*.*)|*.*";
                openFileDialog.Title = "選擇 WAV 音效檔";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    wavFilePath = openFileDialog.FileName;
                    lblFileName.Text = wavFilePath;
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(wavFilePath))
            {
                try
                {
                    if (isPaused)
                    {
                        mciSendString("resume myWav", null, 0, IntPtr.Zero);
                        isPaused = false;
                    }
                    else
                    {
                        mciSendString("close myWav", null, 0, IntPtr.Zero);
                        string command = $"open \"{wavFilePath}\" type waveaudio alias myWav";
                        mciSendString(command, null, 0, IntPtr.Zero);
                        SetVolume();
                        mciSendString("play myWav", null, 0, IntPtr.Zero);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("播放失敗: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("請先選擇 WAV 檔案。");
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            mciSendString("pause myWav", null, 0, IntPtr.Zero);
            isPaused = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mciSendString("stop myWav", null, 0, IntPtr.Zero);
            mciSendString("close myWav", null, 0, IntPtr.Zero);
            isPaused = false;
        }

        private void btnVolumeUp_Click(object sender, EventArgs e)
        {
            if (currentVolume < 1000)
            {
                currentVolume += 100;
                if (currentVolume > 1000) currentVolume = 1000;
                SetVolume();
            }
        }

        private void btnVolumeDown_Click(object sender, EventArgs e)
        {
            if (currentVolume > 0)
            {
                currentVolume -= 100;
                if (currentVolume < 0) currentVolume = 0;
                SetVolume();
            }
        }

        private void SetVolume()
        {
            uint v = (uint)((currentVolume / 1000.0) * 0xFFFF);
            uint newVolume = (v & 0xFFFF) | (v << 16);
            waveOutSetVolume(IntPtr.Zero, newVolume);
        }
    }
}
