using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace MinecraftServerMaker
{
    public partial class InstallForge : Form
    {
        public string Version;
        public string TargetPath;
        public string forgeVersion;
        public string installerName;
        public Thread installThread;

        public InstallForge(string Version, string Path) { 
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

        private void InstallForge_Load(object sender, EventArgs e)
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

        private string getUrlParameter(string Url, string name) {
            string[] splited = Url.Split('?');
            if (splited.Length < 2) return null;
            string[] parameters = splited[1].Split('&');
            foreach (string parameter in parameters) {
                splited = parameter.Split('=');
                if (splited.Length < 2) return null;
                if (splited[0] == name) return splited[1];
            }
            return null;
        }

        public void runInstall() {
            try {
                writeLogLbl($"설치할 포지 서버의 마인크래프트 버전 : {Version}");
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
                writeLogLbl($"포지 홈페이지에서 {Version}의 추천 버전을 가져오는 중..");
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                using (WebClient wc = new WebClient()) {
                    htmlDoc.LoadHtml(wc.DownloadString(
                        $"https://files.minecraftforge.net/net/minecraftforge/forge/index_{Version}.html"
                    ));
                }
                string installerUrl = null;
                HtmlNodeCollection versions = htmlDoc.DocumentNode.SelectNodes("/html/body/main/div[2]/div[1]/div[2]/div/div");
                HtmlNodeCollection tables = versions[(versions.Count == 2) ? 1 : 0].SelectNodes(".//div[2]/div");
                foreach (HtmlNode table in tables) {
                    HtmlNode option = table.SelectSingleNode(".//a");
                    string name = option.GetAttributeValue("title", null).ToLower();
                    if (name == "installer") {
                        string url = option.GetAttributeValue("href", null);
                        installerUrl = getUrlParameter(option.GetAttributeValue("href", null), "url");
                        string[] paths = installerUrl.Split('/');
                        forgeVersion = paths[paths.Length-2];
                        installerName = paths[paths.Length-1];
                        break;
                    }
                }
                if (string.IsNullOrWhiteSpace(installerUrl)) {
                    writeLogLbl("인스톨러 파일이 해당 버전에 없음..");
                    MessageBox.Show(
                        "해당 버전은 포지에서 인스톨러를 지원하지 않는 듯 합니다.\n미안하지만 다른 버전을 하는게..^^", 
                        this.Text, 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Error
                    );
                    return;
                }

                using (WebClient wc = new WebClient()) {
                    writeLogLbl($"포지 {forgeVersion} 버전 설치파일을 다운로드 중..");
                    progress.BeginInvoke(new Action(() => {
                        progress.Style = ProgressBarStyle.Blocks;
                    }));
                    wc.DownloadProgressChanged += progressForgeInstaller;
                    wc.DownloadFileCompleted += completedForgeInstaller;
                    wc.DownloadFileAsync(
                        new Uri(installerUrl),
                        Path.Combine(TargetPath, installerName)
                    );
                }
            } catch (Exception e) {
                writeLogLbl("오류가 발생됨..");
                MessageBox.Show(e.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void progressForgeInstaller(object sender, DownloadProgressChangedEventArgs e) {
            this.BeginInvoke(new Action(() => {
                progressLbl.Text = $"{e.ProgressPercentage}% - {installerName} 설치 중..";
                progress.Value = e.ProgressPercentage;
            }));
        }

        private void completedForgeInstaller(object sender, AsyncCompletedEventArgs e) {
            try {
                progress.BeginInvoke(new Action(() => {
                    progress.Style = ProgressBarStyle.Marquee;
                }));
                writeLogLbl("다운로드 성공!");
                writeLogLbl("타깃 폴더에 포지 서버 설치.. 잠시만 기다려 주세요!");
                using (Process p = new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = "java",
                        Arguments = $"-jar {installerName} --installServer",
                        WorkingDirectory = TargetPath,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                }) {
                    p.Start();
                    p.OutputDataReceived += (object _sender, DataReceivedEventArgs _e) => writeLog(_e.Data);
                    p.ErrorDataReceived += (object _sender, DataReceivedEventArgs _e) => writeLog(_e.Data);
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();
                }
                writeLogLbl("설치 성공!");

                writeLogLbl("포지 설치 파일은 필요없으니 삭제 진행..");
                File.Delete(Path.Combine(TargetPath, installerName));
                File.Delete(Path.Combine(TargetPath, $"{installerName}.log"));
                writeLogLbl("포지 설치 파일 관련 삭제 성공!");

                writeLogLbl("초기 설정을 위해 서버를 실행 중..");
                runServer();

            } catch (Exception except) {
                writeLogLbl("오류에 걸림..");
                MessageBox.Show(except.ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void runServer() {
            string jarArgs = (File.Exists(Path.Combine(TargetPath, $"minecraft_server.{Version}.jar")))
                ? $"-jar minecraft_server.{Version}.jar --nogui"
                : $"@libraries/net/minecraftforge/forge/{forgeVersion}/win_args.txt --nogui";

            using (Process p = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "java",
                    Arguments = jarArgs,
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
                            string batStr = (File.Exists(Path.Combine(TargetPath, $"minecraft_server.{Version}.jar")))
                                ? $"java -jar minecraft_server.{Version}.jar"
                                : $"java @libraries/net/minecraftforge/forge/{forgeVersion}/win_args.txt";
                            File.WriteAllText(Path.Combine(TargetPath, "start.bat"), batStr);
                        }
                    }
                    try { (sender as Process).Kill(); } catch { }
                    MessageBox.Show($"포지 {Version}버전 서버 개설이 성공 되었습니다", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void InstallForge_FormClosing(object sender, FormClosingEventArgs e)
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
