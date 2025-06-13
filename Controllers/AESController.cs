using System;
using System.IO;
using System.Threading.Tasks;
using Bilgi_Güvenliği_ve_Kriptoloji.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Controllers
{
    public class AESController : Controller
    {
        private readonly IAESService _aesService;

        public AESController(IAESService aesService)
        {
            _aesService = aesService;
        }

        public IActionResult Index()
        {
            // Varsayılan olarak yeni bir anahtar ve IV oluştur
            var (key, iv) = _aesService.GenerateKeyAndIV();
            ViewBag.Key = key;
            ViewBag.IV = iv;

            return View();
        }

        [HttpPost]
        public IActionResult EncryptText(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                TempData["ErrorMessage"] = "Lütfen metin, anahtar ve IV değerlerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                string encryptedText = _aesService.Encrypt(plainText, key, iv);
                ViewBag.EncryptedText = encryptedText;
                ViewBag.Key = key;
                ViewBag.IV = iv;
                ViewBag.PlainText = plainText;
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Şifreleme sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult DecryptText(string cipherText, string key, string iv)
        {
            if (string.IsNullOrEmpty(cipherText) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                TempData["ErrorMessage"] = "Lütfen şifreli metin, anahtar ve IV değerlerini doldurun.";
                return RedirectToAction("Index");
            }

            try
            {
                string decryptedText = _aesService.Decrypt(cipherText, key, iv);
                ViewBag.DecryptedText = decryptedText;
                ViewBag.Key = key;
                ViewBag.IV = iv;
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
        public async Task<IActionResult> EncryptFile(IFormFile file, string key, string iv)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                TempData["ErrorMessage"] = "Lütfen bir dosya seçin ve anahtar ile IV değerlerini doldurun.";
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

                // Dosyayı şifrele
                byte[] encryptedData = _aesService.EncryptFile(fileData, key, iv);

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
        public async Task<IActionResult> DecryptFile(IFormFile file, string key, string iv)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(iv))
            {
                TempData["ErrorMessage"] = "Lütfen bir dosya seçin ve anahtar ile IV değerlerini doldurun.";
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
                byte[] decryptedData = _aesService.DecryptFile(fileData, key, iv);

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
