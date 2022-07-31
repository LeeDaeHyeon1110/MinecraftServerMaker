using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace MinecraftServerMaker
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void getSpigotBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "스피곳 서버 버전 리스트";
            jarList.Items.Clear();
            jarList.Text = "Spigot";
            WebClient wc = new WebClient();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(wc.DownloadString("https://getbukkit.org/download/spigot"));
            
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id='download']/div/div/div/div");
            foreach (HtmlNode node in nodes) {
                jarList.Items.Add(node.SelectSingleNode(".//div/div[1]/h2").InnerText);
            }
        }

        private void getForgeBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "포지 서버 버전 리스트";
            jarList.Items.Clear();
            jarList.Text = "Forge";
            WebClient wc = new WebClient();
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.LoadHtml(wc.DownloadString("https://files.minecraftforge.net/net/minecraftforge/forge"));

            HtmlNodeCollection verList = htmlDoc.DocumentNode.SelectNodes("/html/body/main/div[1]/aside/section/ul/li");
            foreach (HtmlNode ver in verList) {
                ///html/body/main/div[1]/aside/section/ul/div/div/li[1]/ul
                HtmlNodeCollection verDetailList = ver.SelectNodes(".//ul/li");
                foreach (HtmlNode version in verDetailList) {
                    jarList.Items.Add(version.InnerText.Trim());
                }
            }
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
                if (jarList.Text == "Spigot") {
                    DownloadForm form = new DownloadForm($"https://download.getbukkit.org/spigot/spigot-{jarList.SelectedItem}.jar", cofd.FileName);
                    form.Show();
                } else {
                    WebClient wc = new WebClient();
                    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                    htmlDoc.LoadHtml(wc.DownloadString($"https://files.minecraftforge.net/net/minecraftforge/forge/index_{jarList.SelectedItem}.html"));
                    string version = htmlDoc.DocumentNode.SelectSingleNode("/html/body/main/div[2]/div[1]/div[2]/div/div/div[1]/small").InnerText;
                    DownloadForm form = new DownloadForm($"https://maven.minecraftforge.net/net/minecraftforge/forge/{version}/forge-{version}-installer.jar", cofd.FileName);
                    form.Show();
                }
            }
        }
    }
}
