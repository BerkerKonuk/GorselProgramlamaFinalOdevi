# C# CurrencyTracker - Döviz Analiz ve Takip Sistemi

Bu proje, **Frankfurter API** altyapısını kullanarak anlık döviz kurlarını çeken, bu veriler üzerinde filtreleme, sıralama ve istatistiksel hesaplamalar yapan bir konsol uygulamasıdır. Veri manipülasyonu için **LINQ** teknolojisi aktif olarak kullanılmıştır.

---

## 🆔 Öğrenci ve Ders Bilgileri

| **Alan** | **Detay** |
| :--- | :--- |
| **Öğrenci Adı** | Berker Konuk |
| **Öğrenci No** | 20230108038 |
| **Bölüm** | Bilgisayar Programcılığı |
| **Ders** | Görsel Programlama (BIP2033) |
| **Ders Sorumlusu** | Öğr. Gör. Emrah SARIÇİÇEK |
| **Teslim Tarihi** | 14.01.2026 |

---

## 📝 Proje Özeti ve Teknik Altyapı

Uygulama, `System.Net.Http` kütüphanesi aracılığıyla **HTTP GET** istekleri göndererek **Türk Lirası (TRY)** tabanlı güncel kur bilgilerini JSON formatında elde eder.

Elde edilen veriler:
1.  `System.Text.Json` kütüphanesi ile nesneye (Deserialize) çevrilir.
2.  Bellekte `List<Currency>` yapısında saklanır.
3.  Kullanıcı taleplerine göre **LINQ (Language Integrated Query)** sorguları ile işlenir.

**Kullanılan Servis:** [Frankfurter API](https://api.frankfurter.app/latest?from=TRY)

---

## 🛠️ Uygulama Fonksiyonları

Program çalıştığında kullanıcıya interaktif bir menü sunar ve aşağıdaki işlemleri gerçekleştirir:

* **📋 Genel Listeleme:** API'den dönen tüm döviz kodlarını ve karşılık gelen kur değerlerini ekrana basar.
* **🔍 Detaylı Arama:** Kullanıcının girdiği döviz koduna (örn: USD, EUR) göre arama yapar (Büyük/küçük harf duyarlılığı yoktur).
* **filtreleme:** Belirlenen bir kur değerinin üzerindeki tüm para birimlerini listeler (Örn: Değeri 10 TL'den büyük olanlar).
* **📊 Sıralama Algoritmaları:** Dövizleri değerlerine göre **Azalan (Descending)** veya **Artan (Ascending)** şekilde sıralı olarak gösterir.
* **📈 İstatistik Paneli:** Mevcut veriler üzerinden matematiksel analiz yapar:
    * Toplam işlem gören para birimi sayısı (`Count`)
    * En değerli kur (`Max`)
    * En düşük değerli kur (`Min`)
    * Kurların genel ortalaması (`Average`)

---

## 💻 Kurulum ve Çalıştırma

Projenin sorunsuz çalışması için aşağıdaki adımları izleyebilirsiniz:

1.  **Gereksinimler:** Bilgisayarınızda **.NET SDK 8.0** ve **Visual Studio 2022** kurulu olmalıdır.
2.  **Projeyi Açın:** İndirdiğiniz klasördeki `.sln` uzantılı dosyayı Visual Studio ile başlatın.
3.  **Başlangıç Ayarı:** Solution Explorer panelinde projeye sağ tıklayıp "Set as Startup Project" dediğinizden emin olun.
4.  **Çalıştırın:** Klavyeden `F5` tuşuna basarak uygulamayı başlatın. (İnternet bağlantısı zorunludur).

---

### Ekran Görüntüsü (Örnek Çıktı)

```text
Veriler sunucudan çekiliyor, lütfen bekleyiniz...

===== CurrencyTracker Ana Menü =====
1. Tüm kurları göster
2. Döviz ara
3. Filtrele (Değere göre)
4. Sırala
5. Genel İstatistikler
0. Çıkış
Seçim: 5

--- Analiz Sonuçları ---
Toplam Kur Adedi : 32
Tavan Kur (Max)  : 4.1523
Taban Kur (Min)  : 0.0268
Ortalama         : 0.8942
