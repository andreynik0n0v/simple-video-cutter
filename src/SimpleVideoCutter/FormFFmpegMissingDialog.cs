using SimpleVideoCutter.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleVideoCutter
{
    public partial class FormFFmpegMissingDialog : Form
    {
        private bool stopDownloadRequest = false; 

        public FormFFmpegMissingDialog()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://www.gyan.dev/ffmpeg/builds/") { UseShellExecute = true });
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            await DownloadLastestFFmpegVersion();
        }

public async Task<bool> DownloadLastestFFmpegVersion()
{
    try
    {
        var downloadSucceeded = await DownloadFFmpegFile();
        if (downloadSucceeded)
        {
            var extractionSucceeded = ExtractFFmpegFile();
            if (extractionSucceeded)
            {
                UpdateFFmpegPath();
                labelDownloadSuccessful.Visible = true;
                VideoCutterSettings.Instance.StoreSettings();
                return true;
            }
        }
    }
    catch (Exception)
    {
        labelError.Visible = true; 
    }

    return false; // Возвращаем false, если операция завершилась неудачно
}

    private async Task<bool> DownloadFFmpegFile()
    {
        var url = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";
        var filename = "ffmpeg-latest-static.zip";

        stopDownloadRequest = false;
        labelError.Visible = false;
        progressBarDownload.Value = 0;
        progressBarDownload.Visible = true;
        buttonClose.Text = GlobalStrings.GlobalCancel;

        using (var client = new CustomWebClient { TimeoutMs = 10000 })
        {
            try
            {
                var uri = new Uri(url);
                await client.DownloadFileTaskAsync(uri, filename);
                return !client.IsBusy && !client.Cancelled && client.Error == null;
            }
            finally
            {
                progressBarDownload.Visible = false;
                buttonClose.Text = GlobalStrings.GlobalClose;
            }
        }
    }

    private bool ExtractFFmpegFile()
    {
        var filename = "ffmpeg-latest-static.zip";
        var folderName = $"ffmpeg.{DateTime.Now:yyyyMMddHHmmss}";

        try
        {
            ZipFile.ExtractToDirectory(filename, folderName);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void UpdateFFmpegPath()
    {
        var folderName = $"ffmpeg.{DateTime.Now:yyyyMMddHHmmss}";
        var dir = Directory.GetDirectories(folderName).FirstOrDefault();
        if (dir != null)
        {
            VideoCutterSettings.Instance.FFmpegPath =
                Path.Combine(Path.GetFullPath(dir), "bin", "ffmpeg.exe");
        }
    }


        private void buttonClose_Click(object sender, EventArgs e)
        {
            if (progressBarDownload.Visible)
            {
                stopDownloadRequest = true; 
            }
            else
            {
                Close();
            }
        }
    }
}



namespace SimpleVideoCutter
{
    public partial class FormFFmpegMissingDialog : Form
    {
        private FFmpegDownloader ffmpegDownloader;

        public FormFFmpegMissingDialog()
        {
            InitializeComponent();
            ffmpegDownloader = new FFmpegDownloader();
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            bool downloadSuccess = ffmpegDownloader.DownloadLatestVersion();

            if (downloadSuccess)
            {
                // Обработка успешной загрузки
            }
            else
            {
                // Обработка неудачной загрузки
            }
        }
    }

}



