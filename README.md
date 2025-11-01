# Sports News Platform / Spor Haberleri ve Skorları Platformu

Global spor haberleri, canlı skorlar, maç istatistikleri ve daha fazlasını sunan modern bir spor platformu.

## 🎯 Proje Hakkında

Bu proje, dünya çapındaki spor haberlerini ve canlı skorları takip edebileceğiniz, modern ve kullanıcı dostu bir platformdur. ESPN, LiveScore, SofaScore gibi lider spor sitelerinden esinlenerek geliştirilmiştir.

## ✨ Ana Özellikler

- 📰 **Güncel Spor Haberleri**: Tüm spor dallarından son dakika haberleri
- ⚡ **Canlı Skorlar**: Gerçek zamanlı maç skorları ve güncellemeleri
- 📊 **Detaylı İstatistikler**: Maç, takım ve oyuncu istatistikleri
- 🎯 **Kişiselleştirilmiş İçerik**: Favori takımlarınızı takip edin
- 🔔 **Anlık Bildirimler**: Önemli gelişmelerden haberdar olun
- 🌍 **Çoklu Dil Desteği**: Türkçe, İngilizce ve diğer diller
- 📱 **Mobil Uyumlu**: Her cihazda mükemmel deneyim
- 🎮 **Fantasy Sports**: Kendi hayalinizdeki takımı oluşturun

## 📚 Dokümantasyon

Detaylı dokümantasyon için `docs/` klasörüne bakınız:

- [**Özellik Listesi**](docs/FEATURE_LIST.md) - Tüm platform özellikleri
- [**Yapılacaklar Listesi**](docs/TODO_LIST.md) - Sprint planlaması ve görevler
- [**Teknik Dokümantasyon**](docs/TECHNICAL_DOCUMENTATION.md) - Mimari ve teknoloji detayları
- [**API Referansı**](docs/API_REFERENCE.md) - API endpoint'leri ve kullanımı
- [**Rakip Analizi**](docs/COMPETITOR_ANALYSIS.md) - Pazar araştırması ve strateji
- [**Proje Kurulumu**](docs/PROJECT_SETUP.md) - Geliştirme ortamı kurulumu

## 🚀 Hızlı Başlangıç

```bash
# Repository'yi klonlayın
git clone https://github.com/gitfcankaya/sportnewsandscores.git
cd sportnewsandscores

# Bağımlılıkları yükleyin
pnpm install

# Veritabanını başlatın
docker-compose up -d

# Geliştirme sunucusunu başlatın
pnpm dev
```

Detaylı kurulum talimatları için [PROJECT_SETUP.md](docs/PROJECT_SETUP.md) dosyasına bakınız.

## 🛠️ Teknoloji Stack

### Frontend
- Next.js 14+ (React)
- TypeScript
- Tailwind CSS
- Shadcn/ui
- React Query
- Zustand

### Backend
- NestJS
- PostgreSQL
- Redis
- Prisma ORM
- Socket.io
- JWT Authentication

### DevOps
- Docker
- GitHub Actions
- DigitalOcean/AWS
- CloudFlare CDN

## 📋 Proje Durumu

Proje aktif geliştirme aşamasındadır. Mevcut durum:

- [x] Proje planlaması ve dokümantasyon
- [ ] Temel altyapı kurulumu
- [ ] Frontend geliştirme
- [ ] Backend API geliştirme
- [ ] Canlı skor sistemi entegrasyonu
- [ ] Kullanıcı yönetimi
- [ ] Test ve dağıtım

Detaylı ilerleme için [TODO_LIST.md](docs/TODO_LIST.md) dosyasına bakınız.

## 🎨 Tasarım Özellikleri

- Modern ve minimal arayüz
- Dark/Light mode desteği
- Responsive tasarım (mobil, tablet, desktop)
- Hızlı yükleme süreleri
- Erişilebilir (WCAG 2.1 AA uyumlu)

## 🔐 Güvenlik

- JWT tabanlı kimlik doğrulama
- Password hashing (bcrypt)
- Rate limiting
- CORS yapılandırması
- XSS ve CSRF koruması
- GDPR uyumluluğu

## 📊 Performans Hedefleri

- Sayfa yükleme: <2 saniye
- API yanıt süresi: <200ms
- Uptime: >99.9%
- Lighthouse skoru: >90

## 🤝 Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz! Lütfen şu adımları izleyin:

1. Fork'layın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Değişikliklerinizi commit edin (`git commit -m 'feat: Add amazing feature'`)
4. Branch'inizi push edin (`git push origin feature/amazing-feature`)
5. Pull Request açın

## 📝 Lisans

[Lisans bilgisi eklenecek]

## 👥 İletişim

- GitHub: [@gitfcankaya](https://github.com/gitfcankaya)
- Email: [İletişim bilgisi eklenecek]

## 🙏 Teşekkürler

Bu proje, aşağıdaki platformlardan ilham alınarak geliştirilmiştir:
- ESPN
- LiveScore
- SofaScore
- Transfermarkt
- Goal.com

## 📈 Roadmap

### Q4 2024 (Mevcut)
- ✅ Proje planlaması ve dokümantasyon
- 🚧 Temel altyapı kurulumu
- 🚧 MVP geliştirme

### Q1 2025
- Canlı skor sistemi
- Kullanıcı hesapları
- Mobil uygulama beta
- Beta lansmanı

### Q2 2025
- Premium özellikler
- Fantasy sports
- Video içerik
- Tam lansman

### Q3 2025 ve Sonrası
- Bölgesel genişleme
- AI özellikleri
- E-ticaret entegrasyonu
- Mobil uygulama global lansman 
