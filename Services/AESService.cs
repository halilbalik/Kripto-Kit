using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public class AESService : IAESService
    {
        // AES şifreleme için anahtar ve IV boyutları
        private const int KeySize = 256;
        private const int BlockSize = 128;

        // Metin şifreleme
        public string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Şifrelenecek metin boş olamaz.");

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] keyBytes;
            byte[] ivBytes;

            try
            {
                keyBytes = Convert.FromBase64String(key);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan anahtar geçerli bir Base64 formatında değil.");
            }

            try
            {
                ivBytes = Convert.FromBase64String(iv);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan IV geçerli bir Base64 formatında değil.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        // Metin şifre çözme
        public string Decrypt(string cipherText, string key, string iv)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Çözülecek şifreli metin boş olamaz.");

            byte[] cipherBytes;
            byte[] keyBytes;
            byte[] ivBytes;

            try
            {
                cipherBytes = Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Çözülecek şifreli metin geçerli bir Base64 formatında değil.");
            }

            try
            {
                keyBytes = Convert.FromBase64String(key);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan anahtar geçerli bir Base64 formatında değil.");
            }

            try
            {
                ivBytes = Convert.FromBase64String(iv);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan IV geçerli bir Base64 formatında değil.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Encoding.UTF8.GetString(memoryStream.ToArray());
                    }
                }
            }
        }

        // Dosya şifreleme
        public byte[] EncryptFile(byte[] fileData, string key, string iv)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("Şifrelenecek dosya verisi boş olamaz.");

            byte[] keyBytes;
            byte[] ivBytes;

            try
            {
                keyBytes = Convert.FromBase64String(key);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan anahtar geçerli bir Base64 formatında değil.");
            }

            try
            {
                ivBytes = Convert.FromBase64String(iv);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan IV geçerli bir Base64 formatında değil.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileData, 0, fileData.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        // Dosya şifre çözme
        public byte[] DecryptFile(byte[] encryptedData, string key, string iv)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                throw new ArgumentException("Çözülecek şifreli dosya verisi boş olamaz.");

            byte[] keyBytes;
            byte[] ivBytes;

            try
            {
                keyBytes = Convert.FromBase64String(key);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan anahtar geçerli bir Base64 formatında değil.");
            }

            try
            {
                ivBytes = Convert.FromBase64String(iv);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Sağlanan IV geçerli bir Base64 formatında değil.");
            }

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = keyBytes;
                aes.IV = ivBytes;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(encryptedData, 0, encryptedData.Length);
                        cryptoStream.FlushFinalBlock();
                        return memoryStream.ToArray();
                    }
                }
            }
        }

        // Rastgele anahtar ve IV oluşturma
        public (string key, string iv) GenerateKeyAndIV()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.GenerateKey();
                aes.GenerateIV();

                string key = Convert.ToBase64String(aes.Key);
                string iv = Convert.ToBase64String(aes.IV);

                return (key, iv);
            }
        }
    }
}
