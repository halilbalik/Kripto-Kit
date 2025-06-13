using System;
using System.IO;

namespace Bilgi_Güvenliği_ve_Kriptoloji.Services
{
    public interface ISHAService
    {
        // Metin özeti oluşturma
        string ComputeHash(string input);

        // Dosya özeti oluşturma
        string ComputeFileHash(byte[] fileData);

        // Dosya özeti kontrol etme
        bool VerifyFileHash(byte[] fileData, string expectedHash);

        // Metin özeti kontrol etme
        bool VerifyTextHash(string inputText, string expectedHash);
    }
}
