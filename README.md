# Kriptografi Uygulaması

Bu web uygulaması, temel kriptografik algoritmaların (AES, RSA, SHA-256) nasıl çalıştığını göstermek ve kullanıcıların bu algoritmaları metin ve dosyalar üzerinde interaktif olarak deneyimlemelerini sağlamak amacıyla geliştirilmiştir. Uygulama, modern ve minimal bir tasarıma sahip olup, modüler bir CSS yapısıyla geliştirilmiştir.

## Proje Amacı 🚀

*   Kriptografi konusundaki farkındalığı artırmak.
*   Temel şifreleme ve özetleme prensiplerini pratik örneklerle açıklamak.
*   Kullanıcılara AES, RSA ve SHA-256 algoritmalarını güvenli bir şekilde deneme imkanı sunmak.

## Özellikler

### 1. AES (Advanced Encryption Standard) 🔐 
*   **Metin Şifreleme ve Çözme:**
    *   Girdiğiniz metinleri 256-bit AES anahtarı ile şifreleyin ve çözün.
    *   CBC (Cipher Block Chaining) modu ve IV (Initialization Vector) kullanımı.
    *   Anahtar, IV ve şifreli/çözülmüş metinler Base64 formatındadır.
*   **Dosya Şifreleme ve Çözme:**
    *   Dosyalarınızı AES ile güvenli bir şekilde şifreleyin ve çözün.
    *   Şifrelenmiş dosyayı indirme ve şifreli dosyayı yükleyerek çözme.
*   **Anahtar ve IV Oluşturma:**
    *   Her işlem için rastgele ve güvenli AES anahtarı ve IV oluşturma.

### 2. RSA (Rivest-Shamir-Adleman) 🔐
*   **Metin Şifreleme ve Çözme:**
    *   Genel anahtar (public key) ile metin şifreleme.
    *   Özel anahtar (private key) ile metin şifre çözme.
    *   Anahtarlar ve şifreli/çözülmüş metinler Base64 formatındadır.
*   **Dosya Şifreleme ve Çözme:**
    *   Genel anahtar ile dosya şifreleme (Küçük boyutlu dosyalar için önerilir - max 100KB).
    *   Özel anahtar ile dosya şifre çözme.
*   **Anahtar Çifti Oluşturma:**
    *   2048-bit RSA genel ve özel anahtar çifti oluşturma.

### 3. SHA-256 (Secure Hash Algorithm 256-bit)  🧬 
*   **Metin Özeti Hesaplama:**
    *   Girdiğiniz metnin SHA-256 özetini (hash) hesaplama.
*   **Dosya Özeti Hesaplama:**
    *   Yüklediğiniz bir dosyanın SHA-256 özetini hesaplama.
*   **Metin Özeti Doğrulama:**
    *   Bir metnin, verilen SHA-256 özetiyle eşleşip eşleşmediğini kontrol etme.
*   **Dosya Özeti Doğrulama:**
    *   Bir dosyanın, verilen SHA-256 özetiyle eşleşip eşleşmediğini kontrol etme.

### Kullanıcı Arayüzü ve Deneyimi ✨ 
*   **Modern ve Minimal Tasarım:** Temiz, anlaşılır ve kullanıcı dostu arayüz.
*   **Duyarlı (Responsive) Tasarım:** Farklı ekran boyutlarına uyum sağlar.
*   **Modüler CSS Yapısı:** `wwwroot/css` altında `layout`, `components`, ve `pages` klasörleri ile organize edilmiş, bakımı kolay stil dosyaları.
*   **Kolay Kullanım:** Her kriptografik algoritma için ayrı sekmeler ve net formlar.
*   **Kopyala-Yapıştır Kolaylığı:** Oluşturulan anahtarları, IV'leri ve şifreli/özet metinleri tek tıkla panoya kopyalama özelliği.
*   **Anlık Geri Bildirim:** Kopyalama işlemleri için modern ve animasyonlu tooltip ile kullanıcıya geri bildirim.
*   **Kompakt Görünüm:** Sayfaların gereksiz kaydırmaları engelleyecek şekilde tasarlanması.

## Kullanılan Teknolojiler 🛠️

### Backend
*   **ASP.NET Core MVC (.NET 8):** Güçlü ve modern web uygulamaları geliştirmek için Microsoft tarafından geliştirilen bir framework.
*   **C#:** Ana programlama dili.
*   **System.Security.Cryptography:** .NET kütüphanesi içerisinde yer alan kriptografik işlemler için kullanılan sınıflar.

### Frontend
*   **HTML5:** Web sayfalarının temel yapısı.
*   **CSS3:** Modern stil ve tasarım, modüler bir yaklaşımla (BEM benzeri bir yapıya sahip `layout`, `components`, `pages` klasörleri).
*   **JavaScript:** Dinamik kullanıcı etkileşimleri ve kopyalama gibi özellikler için.
*   **Bootstrap 5:** Hızlı ve duyarlı tasarımlar oluşturmak için popüler bir CSS framework'ü.
*   **Bootstrap Icons:** Arayüzde kullanılan ikonlar için.
*   **Google Fonts (Poppins):** Modern ve okunabilir yazı tipleri için.

## Kurulum ve Çalıştırma (Yerel Geliştirme Ortamı) 📦

1.  **Repository'yi Klonlayın:**
    ```bash
    git clone https://github.com/KULLANICI_ADINIZ/PROJE_ADINIZ.git
    ```
2.  **.NET 8 SDK:** Sisteminizde .NET 8 SDK'nın kurulu olduğundan emin olun.
3.  **Proje Dizinine Gidin:**
    ```bash
    cd PROJE_ADINIZ
    ```
4.  **Bağımlılıkları Yükleyin:**
    ```bash
    dotnet restore
    ```
5.  **Uygulamayı Çalıştırın:**
    ```bash
    dotnet run
    ```
6.  Uygulama varsayılan olarak `https://localhost:PORT` veya `http://localhost:PORT` adresinde açılacaktır. (PORT numarası konsol çıktısında belirtilir.)

## Ekran Görüntüleri 🎥
*   Ana Sayfa
    *   ![image](https://github.com/user-attachments/assets/59e66b0a-9771-46fc-8fe9-d4fa1ae1afa0)
*   AES Şifreleme Sayfası
    *   ![image](https://github.com/user-attachments/assets/01d733a6-5ba6-4143-a848-fa00e650c296)
*   RSA Şifreleme Sayfası
    *   ![image](https://github.com/user-attachments/assets/9d1a9913-31a3-4720-89b2-60ead6fd553f)
*   SHA Şifreleme Sayfası
    *   ![image](https://github.com/user-attachments/assets/ec8a0758-812f-4aa2-9cc3-5486db321981)
*   Hakkında Sayfası
    *   ![image](https://github.com/user-attachments/assets/191602b0-4a4f-4788-97c2-919829c3230e)




## Katkıda Bulunma

Katkılarınız için her zaman açığız! Lütfen bir "issue" açın veya bir "pull request" gönderin.

## Geliştirici

*   **Halil İbrahim Balık**
*   GitHub: [https://github.com/halilbalik](https://github.com/halilbalik) 
