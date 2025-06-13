using System;
using System.IO;
using System.Threading.Tasks;
using Bilgi_Güvenliği_ve_Kriptoloji.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Controllers
{
    public class SHAController : Controller
    {
        private readonly ISHAService _shaService;

        public SHAController(ISHAService shaService)
        {
            _shaService = shaService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                TempData["ErrorMessage"] = "Lütfen özeti hesaplanacak metni girin.";
                return RedirectToAction("Index");
            }

            try
            {
                string hash = _shaService.ComputeHash(input);
                ViewBag.Input = input;
                ViewBag.Hash = hash;
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Özet hesaplama sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ComputeFileHash(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["ErrorMessage"] = "Lütfen özeti hesaplanacak bir dosya seçin.";
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

                // Dosya özeti hesapla
                string fileHash = _shaService.ComputeFileHash(fileData);
                ViewBag.FileName = file.FileName;
                ViewBag.FileHash = fileHash;
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dosya özeti hesaplama sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyFileHash(IFormFile file, string expectedHash)
        {
            if (file == null || file.Length == 0 || string.IsNullOrEmpty(expectedHash))
            {
                TempData["ErrorMessage"] = "Lütfen bir dosya seçin ve beklenen özet değerini girin.";
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

                // Dosya özeti doğrula
                bool isMatch = _shaService.VerifyFileHash(fileData, expectedHash);
                ViewBag.FileName = file.FileName;
                ViewBag.ExpectedHash = expectedHash;
                ViewBag.ActualHash = _shaService.ComputeFileHash(fileData);
                ViewBag.IsMatch = isMatch;
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Dosya özeti doğrulama sırasında hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult VerifyTextHash(string verificationInput, string expectedTextHash)
        {
            if (string.IsNullOrEmpty(verificationInput) || string.IsNullOrEmpty(expectedTextHash))
            {
                TempData["ErrorMessage"] = "Lütfen doğrulanacak metni ve beklenen özet değerini girin.";
                ViewBag.VerificationInput = verificationInput; // Kullanıcının girdiği değeri koru
                ViewBag.ExpectedTextHash = expectedTextHash; // Kullanıcının girdiği değeri koru
                return View("Index"); // Hata mesajıyla birlikte formu tekrar göster
            }

            try
            {
                bool isMatch = _shaService.VerifyTextHash(verificationInput, expectedTextHash);
                string actualHash = _shaService.ComputeHash(verificationInput);

                ViewBag.VerificationInput = verificationInput;
                ViewBag.ExpectedTextHash = expectedTextHash;
                ViewBag.ActualTextHash = actualHash;
                ViewBag.IsTextMatch = isMatch;
                return View("Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                ViewBag.VerificationInput = verificationInput;
                ViewBag.ExpectedTextHash = expectedTextHash;
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Metin özeti doğrulanırken bir hata oluştu: {ex.Message}";
                ViewBag.VerificationInput = verificationInput;
                ViewBag.ExpectedTextHash = expectedTextHash;
                return View("Index");
            }
        }
    }
}
