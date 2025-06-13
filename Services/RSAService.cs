using System;
using System.Security.Cryptography;
using System.Text;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public class RSAService : IRSAService
    {
        private const int KeySize = 2048;

        // RSA anahtar çifti oluşturma
        public (string publicKey, string privateKey) GenerateKeyPair()
        {
            using (RSA rsa = RSA.Create(KeySize))
            {
                string publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                string privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

                return (publicKey, privateKey);
            }
        }

        // Metin şifreleme (public key ile)
        public string Encrypt(string plainText, string publicKey)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Şifrelenecek metin boş olamaz.");

            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(plainText);
            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(publicKeyBytes, out _);

                // RSA şifreleme için OAEP padding (Optimal Asymmetric Encryption Padding) kullanımı
                byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedData);
            }
        }

        // Metin şifre çözme (private key ile)
        public string Decrypt(string cipherText, string privateKey)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentException("Çözülecek şifreli metin boş olamaz.");

            byte[] dataToDecrypt = Convert.FromBase64String(cipherText);
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

                // RSA şifre çözme için OAEP padding kullanımı
                byte[] decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        // Dosya şifreleme
        public byte[] EncryptFile(byte[] fileData, string publicKey)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("Şifrelenecek dosya verisi boş olamaz.");

            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(publicKeyBytes, out _);

                // RSA şifreleme sınırlı boyutta veri şifreleyebilir, bu yüzden parçalama gerekebilir
                // Not: RSA genellikle doğrudan büyük dosyalar için kullanılmaz
                // Gerçek senaryolarda hibrit şifreleme (AES+RSA) kullanılması önerilir
                int maxBlockSize = (KeySize / 8) - 42; // OAEP padding için alan bırakılır
                int dataLength = fileData.Length;
                int blocksCount = (int)Math.Ceiling((double)dataLength / maxBlockSize);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Blok sayısını kaydet
                    byte[] blocksCountBytes = BitConverter.GetBytes(blocksCount);
                    memoryStream.Write(blocksCountBytes, 0, blocksCountBytes.Length);

                    for (int i = 0; i < blocksCount; i++)
                    {
                        int blockSize = Math.Min(maxBlockSize, dataLength - i * maxBlockSize);
                        byte[] blockData = new byte[blockSize];
                        Buffer.BlockCopy(fileData, i * maxBlockSize, blockData, 0, blockSize);

                        byte[] encryptedBlock = rsa.Encrypt(blockData, RSAEncryptionPadding.OaepSHA256);

                        // Şifreli blok boyutunu kaydet
                        byte[] blockLengthBytes = BitConverter.GetBytes(encryptedBlock.Length);
                        memoryStream.Write(blockLengthBytes, 0, blockLengthBytes.Length);

                        // Şifreli bloğu kaydet
                        memoryStream.Write(encryptedBlock, 0, encryptedBlock.Length);
                    }

                    return memoryStream.ToArray();
                }
            }
        }

        // Dosya şifre çözme
        public byte[] DecryptFile(byte[] encryptedData, string privateKey)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                throw new ArgumentException("Çözülecek şifreli dosya verisi boş olamaz.");

            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            using (RSA rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(privateKeyBytes, out _);

                using (MemoryStream inputStream = new MemoryStream(encryptedData))
                using (MemoryStream outputStream = new MemoryStream())
                {
                    // Blok sayısını oku
                    byte[] blocksCountBytes = new byte[4]; // int = 4 bytes
                    inputStream.Read(blocksCountBytes, 0, blocksCountBytes.Length);
                    int blocksCount = BitConverter.ToInt32(blocksCountBytes, 0);

                    for (int i = 0; i < blocksCount; i++)
                    {
                        // Şifreli blok boyutunu oku
                        byte[] blockLengthBytes = new byte[4]; // int = 4 bytes
                        inputStream.Read(blockLengthBytes, 0, blockLengthBytes.Length);
                        int blockLength = BitConverter.ToInt32(blockLengthBytes, 0);

                        // Şifreli bloğu oku
                        byte[] encryptedBlock = new byte[blockLength];
                        inputStream.Read(encryptedBlock, 0, blockLength);

                        // Bloğu çöz
                        byte[] decryptedBlock = rsa.Decrypt(encryptedBlock, RSAEncryptionPadding.OaepSHA256);

                        // Çözülen bloğu çıkış akışına yaz
                        outputStream.Write(decryptedBlock, 0, decryptedBlock.Length);
                    }

                    return outputStream.ToArray();
                }
            }
        }
    }
}
