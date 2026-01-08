# ProductHub

## Genel Mimari

Proje **Domain-Driven Design(DDD)** ile geliştirilmiştir:

### Katman Sorumlulukları

- **API**
  - Controllerlar
  - Request / Response modelleri
  - Swagger
  - Authorization attributeları

- **Application**
  - Use-case servisleri
  - Validation (FluentValidation)
  - DTO’lar
  - Mapping(manuel)
  - External API karşılaştırma

- **Domain**
  - Entity’ler
  - Domain kuralları ve davranışları
  - Repository interfaceleri

- **Infrastructure**
  - Dapper repository implementasyonları
  - Stored Procedure çağrıları
  - External API client (FakeStore)
  - Persistence altyapısı

Domain katmanı DTO, HTTP veya DB detaylarını bilmez.

---

## Authentication & Token Mantığı

- JWT tabanlı authentication kullanıldı
- Login sonrası token üretiliyor
- Token doğrulaması **merkezi olarak** JwtBearer middleware üzerinden yapılıyor
- Authentication gerekli endpoint’ler `[Authorize]` attribute’u ile işaretlendi
- Rol bazlı işlemler için `[Authorize(Roles = "Admin")]` kullanıldı

### Token İçeriği (Özet)
- `sub` → UserId
- `unique_name` → Username
- `role` → Admin / User
- `exp` → Expiration

> Swagger kullanırken Authorize alanına 'Bearer {token}' girilmelidir.

---

## 🗄️ Veri Erişimi

- Veritabanı işlemleri **Stored Procedure** üzerinden yapıldı
- Veri erişimi için **Dapper ORM** kullanıldı
- Repository pattern kullanılarak DB erişimi ayrıştırıldı

Örnek SP’ler:
- `CATALOG.sp_Product_GetAll`
- `CATALOG.sp_Product_GetById`
- `CATALOG.sp_Product_Insert`
- `CATALOG.sp_Product_Update`
- `CATALOG.sp_Product_Delete`
- `CATALOG.sp_Product_MatchExternal`
- `CATALOG.sp_Product_UnmatchExternal`

---

## ✅ Validation & Mapping

### Validation
- Request doğrulamaları **FluentValidation** ile yapılmıştır
- Validation Application katmanında konumlandırıldı

### Mapping
- DTO <- Entity dönüşümleri **manuel mapping** ile yapılmıştır
- Domain kurallarını korumak için Entity constructor ve davranış metotları tercih edilmiştir

> AutoMapper kullanılabilirdi, ancak bu projede kontrol ve okunabilirlik açısından manuel mappıng tercih edildi.

---

## 🌐 Harici API Entegrasyonu

- FakeStore API kullanılmıştır: https://fakestoreapi.com/products


GET https://fakestoreapi.com/products

### Yaklaşım
- External ürünler Infrastructure katmanında alınır
- Application katmanında local ürünler ile karşılaştırılır
- Sonuç aşağıdaki gruplarla döndürülür:
- Matched products
- Only in local
- Only in external
- Bu projede ürün eşleştirmesi `ExternalProductId` üzerinden yapıldı. Gerçek hayat senaryolarında, farklı veri kaynakları için daha gelişmiş benzerlik algoritmaları veya manuel eşleşme mekanizmaları kullanılabilir.

### Endpoint
GET /api/products/compare-external



## 🔄 Product Endpoints

Tüm product endpoint’leri authentication gerektirir.

GET /api/products
GET /api/products/{id}
POST /api/products (Admin)
PUT /api/products/{id} (Admin)
DELETE /api/products/{id} (Admin)

POST /api/products/{id}/match-external (Admin)
DELETE /api/products/{id}/match-external (Admin)


## 🧪 Hata Yönetimi

- GlobalExceptionMiddleware ile merkezi hata yönetimi yapılmıştır
- Validation hataları 400
- Yetkisiz erişimler 401 / 403
- Beklenmeyen hatalar 500 olarak döner

## Projeyi Çalıştırma

### Gereksinimler
- .NET 8 SDK
- SQL Server

### Adımlar
1. Veritabanını scriptini çalıştırın(Infrastructure -> Script -> ProductDatabaseScript.sql)
2. `appsettings.Development.json` dosyasına connection string ve JWT ayarlarını ekleyin
3. API’yi çalıştırın:

dotnet run --project ProductHub.API

4. Swagger:

https://localhost:{port}/swagger

## Authentication (Demo User)

Projenin test edilebilmesi için örnek bir kullanıcı oluşturulmuştur.

**Username:** `admin`  
**Password:** `admin.925!`  

>  Bu kullanıcı yalnızca demo/test içindir.

---

## Teknik Özeti

- Katmanlı mimari ile bağımlılıklar ayrıştırıldı
- Domain modeli temiz tutuldu
- JWT doğrulaması yapıldı
- Dapper + SP ile performans ve kontrol sağlandı
- External API entegrasyonu ayrıştırıldı edildi

---


