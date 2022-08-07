using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using System.IO;
using System.Diagnostics;

namespace MinecraftServerMaker
{
    public partial class InstallSpigot : Form
    {
        public string Version;
        public string TargetPath;
        public Thread installThread;

        public InstallSpigot(string Version, string Path) {
            InitializeComponent();
            this.Version = Version;
            this.TargetPath = Path;
        }

        private void writeLogLbl(string text) {
            if (!string.IsNullOrEmpty(text)) {
                progressLbl.BeginInvoke(new Action(() => progressLbl.Text = text));
                logBox.BeginInvoke(new Action(() => logBox.AppendText(text + "\r\n")));
            }
        }

        private void writeLog(string text) {
            if (!string.IsNullOrEmpty(text)) {
                logBox.BeginInvoke(new Action(() => {
                    logBox.AppendText(text + "\r\n");
                }));
            }
        }

        private void InstallSpigot_Load(object sender, EventArgs e)
        {
        }

        private string getJavaVersion() {
            using (Process p = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "java.exe",
                    Arguments = "-version",
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            }) {
                p.Start();  
                string output = p.StandardError.ReadToEnd();
                int startIndex = output.IndexOf('"') + 1;
                int endIndex = output.IndexOf('"', startIndex);
                return output.Substring(startIndex, endIndex-startIndex);
            };
        }

        public void runInstall() {
            try {
                writeLogLbl($"설치할 스피곳 서버의 마인크래프트 버전 : {Version}");
                writeLogLbl($"서버를 개설할 폴더 경로 : {TargetPath}");
                writeLogLbl("자바의 설치 여부를 확인 중..");
                string javaVersion = getJavaVersion();
                if (javaVersion == null) {
                    writeLogLbl("자바가 감지가 안됨..");
                    MessageBox.Show("서버를 실행 시킬려면 자바가 필요합니다.\n자바를 다운로드하고 다시 해주세용", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
                writeLogLbl($"자바 {javaVersion} 버전 감지됨!");
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                using (WebClient wc = new WebClient()) {
                    writeLogLbl($"스피곳 {Version} 버전 jar파일을 다운로드 중..");
                    progress.BeginInvoke(new Action(() => {
                        progress.Style = ProgressBarStyle.Blocks;
                    }));
                    wc.DownloadProgressChanged += progressInstall;
                    wc.DownloadFileCompleted += completedInstall;
                    wc.DownloadFileAsync(
                        new Uri($"https://download.getbukkit.org/spigot/spigot-{Version}.jar"),
                        Path.Combine(TargetPath, $"spigot-{Version}.jar")
                    );
                }
            } catch (Exception e) {
                writeLogLbl("오류가 발생됨..");
                MessageBox.Show(e.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void progressInstall(object sender, DownloadProgressChangedEventArgs e) {
            this.BeginInvoke(new Action(() => {
                progressLbl.Text = $"{e.ProgressPercentage}% - spigot-{Version}.jar 설치 중..";
                progress.Value = e.ProgressPercentage;
            }));
        }

        private void completedInstall(object sender, AsyncCompletedEventArgs e) {
            try {
                progress.BeginInvoke(new Action(() => {
                    progress.Style = ProgressBarStyle.Marquee;
                }));
                writeLogLbl("다운로드 성공!");
                writeLogLbl("초기 설정을 위해 서버를 실행 중..");
                runServer();
            } catch (Exception except) {
                if (installThread.IsAlive) {
                    writeLogLbl("오류에 걸림..");
                    MessageBox.Show(except.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void runServer() {
            using (Process p = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "java",
                    Arguments = $"-jar spigot-{Version}.jar --nogui",
                    WorkingDirectory = TargetPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true
                }
            }) {
                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.StandardInput.WriteLine("stop");
                p.OutputDataReceived += new DataReceivedEventHandler(serverRecieve);
                p.ErrorDataReceived += new DataReceivedEventHandler(serverRecieve);
            }
        }

        private void serverRecieve(object sender, DataReceivedEventArgs e) {
            if (!string.IsNullOrEmpty(e.Data)) {
                if (InvokeRequired) {
                    logBox.BeginInvoke(new Action(() => {
                        logBox.AppendText(e.Data + "\r\n");
                    }));
                }
                if (e.Data.Contains("EULA")) {
                    writeLogLbl("eula 동의 필요함 감지");
                    try { (sender as Process).Kill(); } catch { }
                    writeLogLbl("서버 종료");

                    string eulaPath = Path.Combine(TargetPath, "eula.txt");
                    if (!File.Exists(eulaPath)) {
                        int count = 0;
                        while (true) {
                            if (count > 5) {
                                writeLogLbl("결국 오류가 발생되었습니다..");
                                MessageBox.Show(
                                    "eula 파일을 서버에서 필요로 함에도 eula파일을 생성해주지도 않네요..",
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
                                return;
                            }
                            writeLogLbl("eula 파일이 생성이 안된것 같아 0.5초 뒤에 다시 실행");
                            Thread.Sleep(500);
                            if (File.Exists(eulaPath)) break;
                            count++;
                        }
                    }
                    string eula = File.ReadAllText(eulaPath);
                    eula = eula.Replace("eula=false", "eula=true");
                    File.WriteAllText(eulaPath, eula);
                    writeLogLbl("eula 동의 완료");
                    writeLogLbl("서버 다시 시작 중..");
                    runServer();
                } else if (e.Data.ToLower().Contains("done")) {
                    writeLogLbl("서버 개설 성공!!");
                    label1.BeginInvoke(new Action(() => label1.Text = "서버 만듬!"));
                    bool existsBat = false;
                    foreach (string file in Directory.GetFiles(TargetPath)) {
                        if (new FileInfo(file).Extension == "*.bat") {
                            existsBat = true;
                            break;
                        }
                    }
                    if (!existsBat) {
                        DialogResult result = MessageBox.Show(
                            "서버를 실행시키는 bat파일이 없습니다.\n하나 만들까요?",
                            this.Text,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );
                        if (result == DialogResult.Yes) {
                            File.WriteAllText(Path.Combine(TargetPath, "start.bat"), "java -jar spigot-{Version}.jar");
                        }
                    }
                    try { (sender as Process).Kill(); } catch { }
                    MessageBox.Show(this.ParentForm, $"스피곳 {Version}버전 서버 개설이 성공 되었습니다", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.BeginInvoke(new Action(this.Close));
                    Process.Start(TargetPath);
                }
            }
        }

        private void showLogCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (showLogCheck.Checked) {
                this.Size = new Size(500, 430);
                logBox.Location = new Point(12, 106);
            } else {
                this.Size = new Size(500, 150);
                logBox.Location = new Point(12, 111);
            }
        }

        private void InstallSpigot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (installThread.IsAlive) {
                e.Cancel = true;
                DialogResult dr = MessageBox.Show("정말로 설치를 종료하시겠습니까?", this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK) {
                    installThread.Abort();
                    this.Close();
                }
            }
        }
    }
}
