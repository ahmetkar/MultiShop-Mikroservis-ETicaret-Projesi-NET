[🇹🇷 Türkçe README](README.tr.md) | [🇬🇧 English README](README.en.md)




# MultiShop E-TİCARET Sitesi Projesi

.NET Core ile admin paneli ve e-ticaret sitesi arayüzü olan ,frontend olarak .net core MVC kullanan, backend olarak 8 mikroservis içeren web projesidir. Sepet sistemi ve ödeme sistemi ,kupon uygulama,kampanya ekranları,sipariş ve kargo süreci yönetimi gibi özellikler içeriyor.

Bu proje Murat Yücedağın Multishop E-ticaret eğitim serisinden faydalanılarak yazılmış ve benim tarafımdan düzeltmeler,eklemeler yapılmıştır.

# Benim Eklediklerim

- Cargo mikroservisi eklendi
- Sepet yapısı güncellendi baştan yazıldı,giriş yapmayan ve yapanlar için cookieyle ekleme ve redise ekleme istisnaları eklendi. Ayrıca ürünlerinn filtreleriyle birlikte ayrıca sepete eklenmesi ve sipariş detaylarına eklenmesi sağlandı.
- Payment frontend ve backend düzeltmeleri yapıldı.
- Order servisi yeniden düzenlendi.
- Kafka ile Order-Payment-Cargo arası asenkron kuyruk mesajlaşma yapısı choereography saga pattern ile kuruldu
- Ürün filtreleme ve admin panelinden filtre ekleme,filtreleri ürünler ve kategorilerle ilişkilendirme özellikleri eklendi.
- Ürün kampanya sayfaları eklendi ve ürünlerin admin panelinden bunlarla ilişkilendirilmesi , seçilen ürünlerin bu kampanya sayfalarında görüntülenmesi sağlandı.
- Arama özellği eklendi.
- Sayfalama özelliği eklendi.
- Kullanıcı için profil sayfası eklendi. Bilgilerinin yönetimi eklendi. Sipariş,kargo takibi eklendi. 
- Admin panelde istatistik sayfası,sipariş yönetimi,kasa bilgileri,kargo yönetimi eklendi.
- Admin panelde indirim ve kupon yönetimi ve bunların ilgili yerlere yansıtılması eklendi.
- Admin panelde kargo şirketleri ve fiyatları yönetimi ve bunların ilgili yerlere yansıtılması eklendi.


# Kafka ile Mikroservis Arası Mesajlaşma Yapısı

- Kullanıcı sepete ürün ekler devam eder adres seçip Order oluşturur. Order oluşunca OrderCreated eventi kafkaya yayınlanır.Ve Payment servisi OrderCreatedi dinler
ve veritabanında PaymentOrderSnapshot kaydı oluşturur .
 - Kullanıcı devam eder ödeme yapar Payment oluşturur.Payment oluşturulurken PaymentOrderSnapshot tablosundaki bilgiler kontrol edilir. Ve payment oluşturulup payment işlemi taklit edilip PaymentCompleted veya PaymentFailed Eventi yayınlanır.
 - Order PaymentCompleted veya PaymentFailed eventini dinler ve ona göre Ordering tablosundaki Status durumunu değiştirir
 - Cargo PaymentCompleted eventini dinler ve gerçekleşince kargo müşterisi,kargo detayı ve kargo operasyonu oluşturma işlemini tamamlar.CargoCreated veya CargoFailed olayını yayınlar
 - Order bu olayları dinler ve ona göre tablosundaki Status durumunu değiştirir.
 - Cargo teslim edildi olarak işaretlenirse CargoDelivered olayı Cargo servisi tarafından yayınlanır.
 - Order bu CargoDelivered olayını dinler ve Ordering tablosundaki Status durumunu Completed olarak değiştirir.

 Her mikroservis alakalı olduğu olayı dinlediği berirli bir orkestratör olmadığı için buna choereography saga pattern deniyor. Ve patterni bu uygullamaya bu şekilde uyarladım.



# İçerdiği Mikroservisler

Basket
Cargo
Catalog
Comment
Discount
Order
Payment
IdentityService

# Veritabanları Bilgileri
- Basket için Docker üzerinde çalışan Redis veritabanı
- Payment için Docker üzerinde çalışan MSSQL veritabanı
- Identity için Docker üzerinde çalışan MSSQL veritabanı
- Cargo için Docker üzerinde çalışan MSSQL veritabanı
- Order için Docker üzerinde çalışan MSSQL veritabanı
- Comment için Docker üzerinde çalışan MSSQL veritabanı
- Catalog için mongodb veritabanı
- Discount için  Docker üzerinde çalışan MSSQL veritabanı

# Kullanılan Teknolojiler
 • Asp.Net Core 9.0 Web API ve MVC
 • Entity Framework Core
 • Dapper Orm
 • Ocelot Gateway
 • Json Web Token / Identity Service
 • Kafka
 • Docker
 • Saga apattern
 • Onion Mimarisi
 • N-tier Mimarisi
 • Monolitik Mimari
 • CQRS Design Pattern
 • Generic Repository Design Pattern
 • Mediator Design Pattern
 • SOLID ve Clean Code Prensipleri

# Kullanılan Veritabanı Teknolojileri
   • MSSQL
   • MongoDb
   • Redis

# Resimler

Alttaki resimler eskidir. Yeni resimler yakın zamanda eklenecek.

# Web Sitesi Ana Sayfa Ekran Görüntüleri

![resim1](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203429.png)

![resim2](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203442.png)

![resim3](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203507.png)


# Sepet Ekran Görüntüsü

![resim4](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20203939.png)

![resim5](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20204113.png)

# Sipariş Detayları Ekranı

![resim6](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20205552.png)

# Ödeme Ekranı 

![resim7](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20210256.png)

# Ürün Listesi Ekranı

![resim8](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20210431.png)

![resim81](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213811.png)


# Ürün Detayı Ekranı

![resim8](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213637.png)

![resim9](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20213647.png)

# Admin Paneli Ekranı

![resim9](https://github.com/ahmetkar/MultiShop-Mikroservis-ETicaret-Projesi-NET/blob/3a5b9d65de7d5791fddb98457b3909840cafbb30/ekrangoruntuleri/Screenshot%202025-08-07%20214411.png)





