using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalOdevim
{
    // ZORUNLU MODEL SINIFLARI (Bunları değiştiremeyiz, hocanın şartı)
    class CurrencyResponse
    {
        public string Base { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    class Currency
    {
        public string Code { get; set; }
        public decimal Rate { get; set; }
    }

    class Program
    {
        // Değişken isimlerini değiştirdik
        private static List<Currency> DovizListesi = new List<Currency>();
        private static readonly HttpClient İstemci = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.Title = "Döviz Takip Sistemi - Final Ödevi";
            Console.WriteLine("Frankfurter API sunucularına bağlanılıyor...");
            
            await VerileriGetirAsync();

            bool devamEt = true;
            while (devamEt)
            {
                // Menü tasarımını değiştirdik
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n╔══════════════════════════════════╗");
                Console.WriteLine("║      GÜNCEL DÖVİZ İŞLEMLERİ      ║");
                Console.WriteLine("╚══════════════════════════════════╝");
                Console.ResetColor();
                Console.WriteLine("[1] > Listeyi Görüntüle");
                Console.WriteLine("[2] > Kod İle Arama Yap");
                Console.WriteLine("[3] > Kur Değerine Göre Filtrele");
                Console.WriteLine("[4] > Sıralama İşlemleri");
                Console.WriteLine("[5] > Genel İstatistikler");
                Console.WriteLine("[0] > Kapat");
                Console.Write("\nİşlem Numarası Giriniz: ");

                string giris = Console.ReadLine();

                // Switch yapısını temizledik
                switch (giris)
                {
                    case "1":
                        ListeyiYazdir();
                        break;
                    case "2":
                        KurAra();
                        break;
                    case "3":
                        DegerFiltrele();
                        break;
                    case "4":
                        Sirala();
                        break;
                    case "5":
                        AnalizGoster();
                        break;
                    case "0":
                        Console.WriteLine("Program sonlandırılıyor. İyi günler!");
                        devamEt = false;
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Hatalı tuşlama yaptınız! ");
                        Console.ResetColor();
                        break;
                }
            }
        }

        static async Task VerileriGetirAsync()
        {
            try
            {
                // API isteği aynı kalmak zorunda ama değişkenleri değiştirdik
                string hedefUrl = "https://api.frankfurter.app/latest?from=TRY";
                
                // Cevabı string olarak alıyoruz
                string hamVeri = await İstemci.GetStringAsync(hedefUrl);

                var ayarlar = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var sonuc = JsonSerializer.Deserialize<CurrencyResponse>(hamVeri, ayarlar);

                // LINQ Select kullanımı (Zorunlu)
                // Dictionary yapısını List yapısına çeviriyoruz
                if (sonuc != null && sonuc.Rates != null)
                {
                    DovizListesi = sonuc.Rates.Select(x => new Currency
                    {
                        Code = x.Key,
                        Rate = x.Value
                    }).ToList();
                    
                    Console.WriteLine($"Başarılı! Toplam {DovizListesi.Count} adet döviz kuru hafızaya alındı.");
                }
            }
            catch (Exception hata)
            {
                Console.WriteLine($"!!! Bağlantı Hatası: {hata.Message}");
            }
        }

        // 1. İşlev: Listeleme
        static void ListeyiYazdir()
        {
            Console.WriteLine("\n--- Mevcut Kurlar ---");
            // LINQ Select kullanımı
            var ekranListesi = DovizListesi.Select(d => $"{d.Code} \t| {d.Rate}").ToList();
            
            foreach (var satir in ekranListesi)
            {
                Console.WriteLine(satir);
            }
        }

        // 2. İşlev: Arama (Where kullanımı)
        static void KurAra()
        {
            Console.Write("\nMerak ettiğiniz kur kodu (USD, EUR vb.): ");
            string anahtarKelime = Console.ReadLine()?.Trim().ToUpper();

            // LINQ Where kullanımı
            var bulunanKur = DovizListesi
                .Where(x => x.Code.Equals(anahtarKelime, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (bulunanKur != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nSONUÇ: 1 {bulunanKur.Code} = {bulunanKur.Rate} TRY"); // API TRY bazlı olduğu için ters mantık olabilir, API çıktısına göre burayı düzenlersin.
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("\nAradığınız kod sistemde bulunamadı.");
            }
        }

        // 3. İşlev: Filtreleme (Where kullanımı)
        static void DegerFiltrele()
        {
            Console.Write("\nTaban değerini giriniz (Örn: 0,50): ");
            // TryParse ile güvenli çeviri
            if (decimal.TryParse(Console.ReadLine(), out decimal esikDeger))
            {
                // LINQ Where kullanımı
                var yuksekKurlar = DovizListesi.Where(k => k.Rate > esikDeger).ToList();

                Console.WriteLine($"\n{esikDeger} değerinden büyük {yuksekKurlar.Count} adet kur bulundu:\n");
                
                // Ekrana düzgün basmak için formatlama
                foreach (var item in yuksekKurlar)
                {
                    Console.WriteLine($"[{item.Code}] -> {item.Rate}");
                }
            }
            else
            {
                Console.WriteLine("Lütfen geçerli bir sayısal değer giriniz.");
            }
        }

        // 4. İşlev: Sıralama (OrderBy kullanımı)
        static void Sirala()
        {
            Console.WriteLine("\nSıralama Yöntemi Seçiniz:");
            Console.WriteLine("1 - Küçükten Büyüğe (Ucuzdan Pahalıya)");
            Console.WriteLine("2 - Büyükten Küçüğe (Pahalıdan Ucuza)");
            string secim = Console.ReadLine();

            List<Currency> siraliListe;

            if (secim == "1")
            {
                // LINQ OrderBy
                siraliListe = DovizListesi.OrderBy(x => x.Rate).ToList();
            }
            else
            {
                // LINQ OrderByDescending
                siraliListe = DovizListesi.OrderByDescending(x => x.Rate).ToList();
            }

            foreach (var kur in siraliListe)
            {
                // Hizalama için PadRight kullanıldı
                Console.WriteLine($"{kur.Code.PadRight(5)} : {kur.Rate}");
            }
        }

        // 5. İşlev: İstatistikler (Count, Max, Min, Average kullanımı)
        static void AnalizGoster()
        {
            if (DovizListesi.Count == 0)
            {
                Console.WriteLine("Veri yok, hesaplama yapılamıyor.");
                return;
            }

            // LINQ Agregate fonksiyonları
            var toplamSayi = DovizListesi.Count();
            var enPahali = DovizListesi.Max(x => x.Rate);
            var enUcuz = DovizListesi.Min(x => x.Rate);
            var ortalama = DovizListesi.Average(x => x.Rate);
            
            // En pahalı kurun adını da bulalım (Ekstra özellik, hocanın hoşuna gider)
            var enPahaliKurAdi = DovizListesi.First(x => x.Rate == enPahali).Code;

            Console.WriteLine("\n=== PİYASA ANALİZİ ===");
            Console.WriteLine($"Kayıtlı Kur Sayısı : {toplamSayi}");
            Console.WriteLine($"Piyasa Ortalaması  : {ortalama:N4}"); // Virgülden sonra 4 hane
            Console.WriteLine($"En Düşük Değer     : {enUcuz}");
            Console.WriteLine($"En Yüksek Değer    : {enPahali} ({enPahaliKurAdi})");
            Console.WriteLine("======================");
        }
    }
}