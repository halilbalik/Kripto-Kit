using System;
using System.IO;
using System.Threading.Tasks;
using Bilgi_Güvenliği_ve_Kriptoloji.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Controllers
{
    public class RSAController : Controller
    {
        private readonly IRSAService _rsaService;

        public RSAController(IRSAService rsaService)
        {
            _rsaService = rsaService;
        }

        public IActionResult Index()
        {
            if (TempData["PublicKey"] == null || TempData["PrivateKey"] == null)
            {
                var (publicKey, privateKey) = _rsaService.GenerateKeyPair();
                ViewBag.PublicKey = publicKey;
                ViewBag.PrivateKey = privateKey;
                TempData["PublicKey"] = publicKey;
                TempData["PrivateKey"] = privateKey;
            }
            else
            {
                ViewBag.PublicKey = TempData["PublicKey"]?.ToString();
                ViewBag.PrivateKey = TempData["PrivateKey"]?.ToString();
                // TempData'yı bir sonraki istek için tekrar ayarla (keep değil, yeniden ata)
                TempData["PublicKey"] = ViewBag.PublicKey;
                TempData["PrivateKey"] = ViewBag.PrivateKey;
            }

            return View();
        }

        [HttpPost]
        public IActionResult EncryptText(string plainText, string publicKey)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(publicKey))
            {
                TempData["ErrorMessage"] = "Lütfen metin ve genel anahtar değerlerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                string encryptedText = _rsaService.Encrypt(plainText, publicKey); ViewBag.EncryptedText = encryptedText;
                ViewBag.PublicKey = publicKey;
                ViewBag.PlainText = plainText;

                // Önceki private key'i kaybet, böylece kullanıcı public key ile şifreleme yapabilir
                // ancak çözme için yeni key pair oluşturması gerekir
                if (TempData["PrivateKey"] != null && TempData["PrivateKey"]?.ToString() != null)
                {
                    ViewBag.PrivateKey = TempData["PrivateKey"]?.ToString();
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Şifreleme sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DecryptText(string cipherText, string privateKey)
        {
            if (string.IsNullOrEmpty(cipherText) || string.IsNullOrEmpty(privateKey))
            {
                TempData["ErrorMessage"] = "Lütfen şifreli metin ve özel anahtar değerlerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                string decryptedText = _rsaService.Decrypt(cipherText, privateKey);
                ViewBag.DecryptedText = decryptedText;
                ViewBag.PrivateKey = privateKey;
                ViewBag.CipherText = cipherText;

                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Şifre çözme sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EncryptFile(IFormFile file, string publicKey)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(publicKey))
            {
                TempData["ErrorMessage"] = "Lütfen bir dosya seçin ve genel anahtar değerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                // Dosya boyut kontrolü (RSA ile büyük dosyaları şifrelemek verimli değil)
                if (file.Length > 100 * 1024) // 100 KB limit
                {
                    TempData["ErrorMessage"] = "RSA şifreleme için dosya boyutu çok büyük. 100 KB'dan küçük dosyalar kullanın.";
                    return RedirectToAction("Index");
                }

                // Dosyayı oku
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                // Dosyayı şifrele
                byte[] encryptedData = _rsaService.EncryptFile(fileData, publicKey);

                // Şifreli dosyayı indir
                return File(encryptedData, "application/octet-stream", $"{file.FileName}.encrypted");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dosya şifreleme sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DecryptFile(IFormFile file, string privateKey)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(privateKey))
            {
                TempData["ErrorMessage"] = "Lütfen bir dosya seçin ve özel anahtar değerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                // Dosyayı oku
                byte[] fileData;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileData = memoryStream.ToArray();
                }

                // Dosya şifresini çöz
                byte[] decryptedData = _rsaService.DecryptFile(fileData, privateKey);

                // Şifresi çözülen dosyayı indir
                string fileName = file.FileName;
                if (fileName.EndsWith(".encrypted"))
                {
                    fileName = fileName.Substring(0, fileName.Length - 10); // ".encrypted" uzantısını kaldır
                }

                return File(decryptedData, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dosya şifre çözme sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
