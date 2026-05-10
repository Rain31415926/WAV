using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace _1121538_徐霈綺_WAV音效檔播放器
{
    public partial class Form1 : Form
    {
        private SoundPlayer player;
        private string wavFilePath = "";

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
                    player = new SoundPlayer(wavFilePath);
                    player.Play();
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (player != null)
            {
                player.Stop();
            }
        }
    }
}
