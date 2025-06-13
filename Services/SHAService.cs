using System;
using System.Security.Cryptography;
using System.Text;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public class SHAService : ISHAService
    {
        // Metin özeti oluşturma
        public string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Özeti hesaplanacak metin boş olamaz.");

            using (SHA256 sha256 = SHA256.Create())
            {
                // Metni byte dizisine dönüştür
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);

                // SHA-256 özetini hesapla
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Byte dizisini hexadecimal string'e dönüştür
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // x2 formatı iki basamaklı hexadecimal gösterim
                }

                return sb.ToString();
            }
        }

        // Dosya özeti oluşturma
        public string ComputeFileHash(byte[] fileData)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("Özeti hesaplanacak dosya verisi boş olamaz.");

            using (SHA256 sha256 = SHA256.Create())
            {
                // Dosya verisinin SHA-256 özetini hesapla
                byte[] hashBytes = sha256.ComputeHash(fileData);

                // Byte dizisini hexadecimal string'e dönüştür
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        // Dosya özeti kontrol etme
        public bool VerifyFileHash(byte[] fileData, string expectedHash)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("Doğrulanacak dosya verisi boş olamaz.");

            if (string.IsNullOrEmpty(expectedHash))
                throw new ArgumentException("Beklenen özet değeri boş olamaz.");

            // Dosyanın özet değerini hesapla
            string actualHash = ComputeFileHash(fileData);

            // Hesaplanan özet ile beklenen özeti karşılaştır (büyük-küçük harf duyarsız)
            return string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase);
        }

        // Metin özeti kontrol etme
        public bool VerifyTextHash(string inputText, string expectedHash)
        {
            if (string.IsNullOrEmpty(inputText))
                throw new ArgumentException("Doğrulanacak metin boş olamaz.");

            if (string.IsNullOrEmpty(expectedHash))
                throw new ArgumentException("Beklenen özet değeri boş olamaz.");

            // Metnin özet değerini hesapla
            string actualHash = ComputeHash(inputText);

            // Hesaplanan özet ile beklenen özeti karşılaştır (büyük-küçük harf duyarsız)
            return string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
