using System;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public interface IRSAService
    {
        // RSA anahtar çifti oluşturma
        (string publicKey, string privateKey) GenerateKeyPair();

        // Metin şifreleme (public key ile)
        string Encrypt(string plainText, string publicKey);

        // Metin şifre çözme (private key ile)
        string Decrypt(string cipherText, string privateKey);

        // Dosya şifreleme
        byte[] EncryptFile(byte[] fileData, string publicKey);

        // Dosya şifre çözme
        byte[] DecryptFile(byte[] encryptedData, string privateKey);
    }
}
