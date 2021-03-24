using Ionic.Zip;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Guncelleme
{
    public partial class Guncelleme : Form
    {
        public Guncelleme()
        {
            InitializeComponent();
        }

        string dosyaYolu;
        private void Guncelleme_Load(object sender, EventArgs e)
        {
            //Guncelleme.Exe nin olduğu klasörden çikip ana uygulamanın klasörüne zipi kaydedik sonra açıcaz
            dosyaYolu = Application.StartupPath.Substring(0, Application.StartupPath.Length - 11) + @"\ap.zip"; // zipin nereye ineceği
            WebClient istek = new WebClient();
            istek.DownloadFileCompleted += new AsyncCompletedEventHandler(Bitti);
            istek.DownloadProgressChanged += new DownloadProgressChangedEventHandler(BarDegisim);
            istek.DownloadFileAsync(new Uri("https://karadagyazilim.com/ap/ap.zip"), dosyaYolu);
        }

        private void DosyalarıAcma()
        {
            string acilacakZip = dosyaYolu; // indirilen zipin yeri
            string nereyeCikarilacak = Application.StartupPath.Substring(0, Application.StartupPath.Length - 11); // zipin çıkacağı yer

            using (ZipFile zip = ZipFile.Read(acilacakZip)) // sadece .zip uzantıları açıyor
            {
                foreach (ZipEntry e in zip)
                {
                    e.Extract(nereyeCikarilacak, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            File.Delete(dosyaYolu); // zipi siler

            MessageBox.Show("Güncelleme Başarıyla Tamamlanmıştır.", "Güncellendi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Güncelleme bitmiştir
            string anaUygulama= Application.StartupPath.Substring(0, Application.StartupPath.Length - 11)+ @"\InstaBot.exe";
            ProcessStartInfo info = new ProcessStartInfo(anaUygulama);
            Process.Start(info);
            this.Close();
        }

        private void Bitti(object sender, AsyncCompletedEventArgs e)
        {
            label1.Text = "İndirme Tamamlandı";
            DosyalarıAcma();
        }
        private void BarDegisim(object sender, DownloadProgressChangedEventArgs e)
        {
            prgBar.Value = e.ProgressPercentage;
        }
    }
}
