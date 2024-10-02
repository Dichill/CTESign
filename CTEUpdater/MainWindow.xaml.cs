using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;

namespace CTEUpdater
{
    public partial class MainWindow : Window
    {
        private string githubApiUrl = "https://api.github.com/repos/Dichill/CTESign/releases/latest";
        private string downloadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "latest_release.zip");
        private string versionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VERSION");
        private string extractionPath = AppDomain.CurrentDomain.BaseDirectory;

        public MainWindow()
        {
            InitializeComponent();
            versionTxt.Text = VersionHelper.GetLocalVersion(versionFilePath);
            StartUpdateProcess(); 
        }

        private async void StartUpdateProcess()
        {
            try
            {
                string latestReleaseUrl = await GetLatestReleaseDownloadUrl();

                if (latestReleaseUrl != null)
                {
                    processTxt.Text = "New version found! Downloading...";

                    await DownloadLatestRelease(latestReleaseUrl);
                    processTxt.Text = "Download complete! Extracting...";

                    ExtractAndReplaceFiles(downloadPath, extractionPath);
                    processTxt.Text = "Update complete! Restarting...";

                    Process.Start(Path.Combine(extractionPath, "CTESign.exe"));
                    Application.Current.Shutdown();
                }
                else
                {
                    processTxt.Text = "You already have the latest version.";
                }
            }
            catch (Exception ex)
            {
                processTxt.Text = $"Error: {ex.Message}";
            }
        }

        public async Task<string> GetLatestReleaseDownloadUrl()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "CTESign");

                var response = await client.GetStringAsync(githubApiUrl);
                dynamic releaseInfo = JObject.Parse(response);

                string downloadUrl = releaseInfo.assets[0].browser_download_url;
                return downloadUrl;
            }
        }

        public async Task DownloadLatestRelease(string downloadUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
                var canReportProgress = totalBytes != -1;

                using (var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        byte[] buffer = new byte[8192];
                        long totalRead = 0;
                        int bytesRead;

                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalRead += bytesRead;

                            if (canReportProgress)
                            {
                                progressBar.Value = (double)totalRead / totalBytes * 100;
                            }
                        }
                    }
                }
            }
        }

        public void ExtractAndReplaceFiles(string zipFilePath, string targetDirectory)
        {
            if (File.Exists(zipFilePath))
            {
                string tempExtractPath = Path.Combine(targetDirectory, "temp");
                if (Directory.Exists(tempExtractPath))
                {
                    Directory.Delete(tempExtractPath, true);
                }
                ZipFile.ExtractToDirectory(zipFilePath, tempExtractPath);

                foreach (string sourceFilePath in Directory.GetFiles(tempExtractPath, "*", SearchOption.AllDirectories))
                {
                    string relativePath = sourceFilePath.Substring(tempExtractPath.Length + 1);
                    string targetFilePath = Path.Combine(targetDirectory, relativePath);

                    string targetFileDirectory = Path.GetDirectoryName(targetFilePath);
                    if (!Directory.Exists(targetFileDirectory))
                    {
                        Directory.CreateDirectory(targetFileDirectory);
                    }

                    File.Copy(sourceFilePath, targetFilePath, true);
                }

                Directory.Delete(tempExtractPath, true);

                File.Delete(zipFilePath);
            }
        }
    }
}
