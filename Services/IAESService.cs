using System;
using System.IO;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public interface IAESService
    {
        // Metin şifreleme
        string Encrypt(string plainText, string key, string iv);

        // Metin şifre çözme
        string Decrypt(string cipherText, string key, string iv);

        // Dosya şifreleme
        byte[] EncryptFile(byte[] fileData, string key, string iv);

        // Dosya şifre çözme
        byte[] DecryptFile(byte[] encryptedData, string key, string iv);

        // Rastgele anahtar oluşturma
        (string key, string iv) GenerateKeyAndIV();
    }
}
