using System;
using System.Net;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;

namespace MinecraftServerMaker
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        [DllImport("shlwapi.dll")]
        public static extern bool PathIsDirectoryEmptyA(string pszPath);

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void handleGetBtnEnable(bool state) {
            this.BeginInvoke(new Action(() => {
                getSpigotBtn.Enabled = state;
                getForgeBtn.Enabled = state;
            }));
        }

        private void getSpigotBtn_Click(object sender, EventArgs e)
        {
            new Thread(() => {
                handleGetBtnEnable(false);
                this.BeginInvoke(new Action(() => {
                    label1.Text = "스피곳 서버 버전 리스트 가져오는 중..";
                    jarList.Items.Clear();
                    jarList.Tag = "Spigot";           
                    downloadJarBtn.Enabled = false;
                }));
                WebClient wc = new WebClient();
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(wc.DownloadString("https://getbukkit.org/download/spigot"));
                
                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id='download']/div/div/div/div");
                foreach (HtmlNode node in nodes) {
                    this.BeginInvoke(new Action(() => {
                        jarList.Items.Add(node.SelectSingleNode(".//div/div[1]/h2").InnerText);
                    }));
                }
                this.BeginInvoke(new Action(() => {
                    label1.Text = "스피곳 서버 버전 리스트";
                }));
                handleGetBtnEnable(true);
            }).Start();;
        }

        private void getForgeBtn_Click(object sender, EventArgs e)
        {
            new Thread(() => {
                handleGetBtnEnable(false);
                this.BeginInvoke(new Action(() => {
                    label1.Text = "포지 서버 버전 리스트 가져오는 중..";
                    jarList.Items.Clear();
                    jarList.Tag = "Forge";
                    downloadJarBtn.Enabled = false;
                }));
                WebClient wc = new WebClient();
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(wc.DownloadString("https://files.minecraftforge.net/net/minecraftforge/forge"));

                HtmlNodeCollection verList = htmlDoc.DocumentNode.SelectNodes("/html/body/main/div[1]/aside/section/ul/li");
                foreach (HtmlNode ver in verList) {
                    ///html/body/main/div[1]/aside/section/ul/div/div/li[1]/ul
                    HtmlNodeCollection verDetailList = ver.SelectNodes(".//ul/li");
                    foreach (HtmlNode version in verDetailList) {
                        this.BeginInvoke(new Action(() => {
                            jarList.Items.Add(version.InnerText.Trim());
                        }));
                    }
                }
                this.BeginInvoke(new Action(() => {
                    label1.Text = "포지 서버 버전 리스트";
                }));
                handleGetBtnEnable(true);
            }).Start();
        }

        private void jarList_SelectedIndexChanged(object sender, EventArgs e)
        {
            downloadJarBtn.Enabled = (jarList.SelectedIndex > -1);
        }

        private void downloadJarBtn_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog cofd = new CommonOpenFileDialog();
            cofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            cofd.IsFolderPicker = true;
            cofd.Title = "서버를 생성할 폴더를 선택해줘";
            if (cofd.ShowDialog() == CommonFileDialogResult.Ok) {
                if(!PathIsDirectoryEmptyA(cofd.FileName)) {
                    DialogResult result = MessageBox.Show(
                        "선택한 폴더가 비어있지 않습니다.\n폴더를 비우시겠습니까?",
                        this.Text,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );
                    if (result == DialogResult.Yes) {
                        Directory.Delete(cofd.FileName, true);
                        Directory.CreateDirectory(cofd.FileName);
                    }
                }
                string Version = jarList.SelectedItem.ToString();
                if (jarList.Tag.ToString() == "Spigot") {
                    InstallSpigot form = new InstallSpigot(Version, cofd.FileName);
                    form.installThread = new Thread(form.runInstall);
                    form.installThread.Start();
                    form.Show();
                } else {
                    InstallForge form = new InstallForge(Version, cofd.FileName);
                    form.installThread = new Thread(form.runInstall);
                    form.installThread.Start();
                    form.Show();
                }
            }
        }
    }
}
