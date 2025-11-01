# Sports News Platform - Technical Documentation / Teknik Dokümantasyon

## 1. Sistem Mimarisi (System Architecture)

### 1.1 Genel Bakış (Overview)
Platform, modern microservices mimarisi kullanarak ölçeklenebilir ve yüksek performanslı bir yapıya sahip olacaktır.

```
┌─────────────────────────────────────────────────────────────┐
│                     CDN Layer (CloudFlare)                   │
│              Static Assets, Images, CSS, JS                  │
└──────────────────────┬──────────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────────┐
│                   Load Balancer (Nginx)                      │
└──────────────────────┬──────────────────────────────────────┘
                       │
       ┌───────────────┼───────────────┐
       │               │               │
┌──────▼─────┐  ┌─────▼──────┐  ┌────▼─────┐
│  Frontend  │  │  API       │  │ WebSocket│
│  Next.js   │  │  Gateway   │  │ Server   │
│  Server    │  │            │  │          │
└────────────┘  └─────┬──────┘  └──────────┘
                      │
       ┌──────────────┼──────────────┐
       │              │              │
┌──────▼─────┐  ┌────▼─────┐  ┌────▼──────┐
│  News      │  │  Scores  │  │  User     │
│  Service   │  │  Service │  │  Service  │
└──────┬─────┘  └────┬─────┘  └────┬──────┘
       │             │              │
┌──────▼─────────────▼──────────────▼──────┐
│           Message Queue (RabbitMQ)        │
└──────────────────┬────────────────────────┘
                   │
       ┌───────────┼───────────┐
       │           │           │
┌──────▼────┐ ┌───▼────┐ ┌───▼────┐
│PostgreSQL │ │ Redis  │ │MongoDB │
│ (Primary) │ │(Cache) │ │(Logs)  │
└───────────┘ └────────┘ └────────┘
```

### 1.2 Teknoloji Stack

#### Frontend
- **Framework**: Next.js 14+ (React 18+)
- **UI Library**: 
  - Tailwind CSS (styling)
  - Shadcn/ui (component library)
  - Framer Motion (animations)
- **State Management**: Zustand veya Jotai
- **Data Fetching**: React Query (TanStack Query)
- **Form Management**: React Hook Form + Zod
- **Real-time**: Socket.io Client
- **PWA**: next-pwa

#### Backend
- **Runtime**: Node.js 20+ LTS
- **Framework**: NestJS (TypeScript) veya Express
- **Alternative**: Python FastAPI (microservices için)
- **API Style**: RESTful + GraphQL (Apollo Server)
- **Real-time**: Socket.io Server
- **Task Queue**: Bull (Redis based)
- **Cron Jobs**: node-cron

#### Veritabanı (Database)
- **Primary DB**: PostgreSQL 15+
  - JSONB support for flexible data
  - Full-text search
  - Partitioning for large tables
- **Cache**: Redis 7+
  - Session storage
  - API response caching
  - Rate limiting
  - Real-time data
- **Search Engine**: Elasticsearch 8+ (optional, gelişmiş arama için)
- **Logging**: MongoDB veya ELK Stack

#### Altyapı (Infrastructure)
- **Containerization**: Docker + Docker Compose
- **Orchestration**: Kubernetes (production için)
- **CI/CD**: GitHub Actions veya GitLab CI
- **Cloud Provider**: 
  - AWS (EC2, S3, CloudFront, RDS)
  - veya DigitalOcean (cost-effective alternative)
  - veya Azure
- **CDN**: CloudFlare veya AWS CloudFront
- **Monitoring**: 
  - Application: Sentry, New Relic
  - Infrastructure: Prometheus + Grafana
  - Uptime: UptimeRobot

## 2. Veritabanı Şeması (Database Schema)

### 2.1 Core Tables

```sql
-- Users Table
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email VARCHAR(255) UNIQUE NOT NULL,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    avatar_url VARCHAR(500),
    bio TEXT,
    role VARCHAR(20) DEFAULT 'user', -- user, editor, admin
    is_verified BOOLEAN DEFAULT FALSE,
    is_premium BOOLEAN DEFAULT FALSE,
    last_login TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- News/Articles Table
CREATE TABLE articles (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(500) NOT NULL,
    slug VARCHAR(500) UNIQUE NOT NULL,
    summary TEXT,
    content TEXT NOT NULL,
    featured_image VARCHAR(500),
    author_id UUID REFERENCES users(id),
    category_id UUID REFERENCES categories(id),
    status VARCHAR(20) DEFAULT 'draft', -- draft, published, archived
    is_featured BOOLEAN DEFAULT FALSE,
    is_breaking BOOLEAN DEFAULT FALSE,
    views_count INTEGER DEFAULT 0,
    published_at TIMESTAMP,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- SEO fields
    meta_title VARCHAR(255),
    meta_description TEXT,
    meta_keywords TEXT[],
    
    -- Search
    search_vector tsvector
);

-- Categories Table
CREATE TABLE categories (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    slug VARCHAR(100) UNIQUE NOT NULL,
    description TEXT,
    parent_id UUID REFERENCES categories(id),
    icon VARCHAR(100),
    order_index INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Tags Table
CREATE TABLE tags (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) UNIQUE NOT NULL,
    slug VARCHAR(100) UNIQUE NOT NULL,
    usage_count INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Article Tags (Many-to-Many)
CREATE TABLE article_tags (
    article_id UUID REFERENCES articles(id) ON DELETE CASCADE,
    tag_id UUID REFERENCES tags(id) ON DELETE CASCADE,
    PRIMARY KEY (article_id, tag_id)
);

-- Sports Leagues
CREATE TABLE leagues (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    slug VARCHAR(200) UNIQUE NOT NULL,
    sport_type VARCHAR(50) NOT NULL, -- football, basketball, cricket, etc.
    country VARCHAR(100),
    logo_url VARCHAR(500),
    season VARCHAR(20),
    api_id VARCHAR(100), -- External API reference
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Teams
CREATE TABLE teams (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    slug VARCHAR(200) UNIQUE NOT NULL,
    short_name VARCHAR(50),
    logo_url VARCHAR(500),
    founded_year INTEGER,
    stadium VARCHAR(200),
    city VARCHAR(100),
    country VARCHAR(100),
    league_id UUID REFERENCES leagues(id),
    api_id VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Players
CREATE TABLE players (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    slug VARCHAR(200) UNIQUE NOT NULL,
    photo_url VARCHAR(500),
    date_of_birth DATE,
    nationality VARCHAR(100),
    position VARCHAR(50),
    jersey_number INTEGER,
    height INTEGER, -- cm
    weight INTEGER, -- kg
    team_id UUID REFERENCES teams(id),
    market_value DECIMAL(15, 2),
    api_id VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Matches
CREATE TABLE matches (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    league_id UUID REFERENCES leagues(id),
    season VARCHAR(20),
    round VARCHAR(50),
    home_team_id UUID REFERENCES teams(id),
    away_team_id UUID REFERENCES teams(id),
    match_date TIMESTAMP NOT NULL,
    venue VARCHAR(200),
    status VARCHAR(20) DEFAULT 'scheduled', -- scheduled, live, finished, postponed
    home_score INTEGER DEFAULT 0,
    away_score INTEGER DEFAULT 0,
    half_time_home_score INTEGER,
    half_time_away_score INTEGER,
    api_id VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Match Events
CREATE TABLE match_events (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    match_id UUID REFERENCES matches(id) ON DELETE CASCADE,
    event_type VARCHAR(50) NOT NULL, -- goal, card, substitution, etc.
    team_id UUID REFERENCES teams(id),
    player_id UUID REFERENCES players(id),
    minute INTEGER NOT NULL,
    extra_time INTEGER,
    detail TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Match Statistics
CREATE TABLE match_statistics (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    match_id UUID REFERENCES matches(id) ON DELETE CASCADE,
    team_id UUID REFERENCES teams(id),
    possession INTEGER,
    shots_total INTEGER,
    shots_on_target INTEGER,
    corners INTEGER,
    fouls INTEGER,
    yellow_cards INTEGER,
    red_cards INTEGER,
    passes_total INTEGER,
    passes_accurate INTEGER,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Comments
CREATE TABLE comments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    article_id UUID REFERENCES articles(id) ON DELETE CASCADE,
    user_id UUID REFERENCES users(id),
    parent_id UUID REFERENCES comments(id), -- for threaded comments
    content TEXT NOT NULL,
    likes_count INTEGER DEFAULT 0,
    is_edited BOOLEAN DEFAULT FALSE,
    is_deleted BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- User Favorites
CREATE TABLE user_favorites (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(id) ON DELETE CASCADE,
    entity_type VARCHAR(50) NOT NULL, -- team, league, player
    entity_id UUID NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(user_id, entity_type, entity_id)
);

-- Notifications
CREATE TABLE notifications (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID REFERENCES users(id) ON DELETE CASCADE,
    type VARCHAR(50) NOT NULL, -- match_start, goal, news, comment
    title VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    link VARCHAR(500),
    is_read BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Indexes for Performance
CREATE INDEX idx_articles_status ON articles(status);
CREATE INDEX idx_articles_published_at ON articles(published_at DESC);
CREATE INDEX idx_articles_category ON articles(category_id);
CREATE INDEX idx_articles_author ON articles(author_id);
CREATE INDEX idx_articles_search ON articles USING gin(search_vector);
CREATE INDEX idx_matches_date ON matches(match_date);
CREATE INDEX idx_matches_status ON matches(status);
CREATE INDEX idx_matches_league ON matches(league_id);
CREATE INDEX idx_comments_article ON comments(article_id);
CREATE INDEX idx_comments_user ON comments(user_id);
```

## 3. API Yapısı (API Structure)

### 3.1 RESTful Endpoints

#### Authentication & Users
```
POST   /api/v1/auth/register
POST   /api/v1/auth/login
POST   /api/v1/auth/logout
POST   /api/v1/auth/refresh-token
POST   /api/v1/auth/forgot-password
POST   /api/v1/auth/reset-password
GET    /api/v1/auth/verify-email/:token

GET    /api/v1/users/me
PUT    /api/v1/users/me
GET    /api/v1/users/:id
PUT    /api/v1/users/:id/avatar
GET    /api/v1/users/:id/favorites
POST   /api/v1/users/:id/favorites
DELETE /api/v1/users/:id/favorites/:favoriteId
```

#### Articles/News
```
GET    /api/v1/articles?page=1&limit=20&category=football&sort=-published_at
GET    /api/v1/articles/featured
GET    /api/v1/articles/breaking
GET    /api/v1/articles/:slug
POST   /api/v1/articles
PUT    /api/v1/articles/:id
DELETE /api/v1/articles/:id
GET    /api/v1/articles/:id/related

GET    /api/v1/categories
GET    /api/v1/categories/:slug
GET    /api/v1/categories/:slug/articles
```

#### Matches & Scores
```
GET    /api/v1/matches/live
GET    /api/v1/matches/upcoming?date=2024-11-01
GET    /api/v1/matches/finished?date=2024-11-01
GET    /api/v1/matches/:id
GET    /api/v1/matches/:id/events
GET    /api/v1/matches/:id/statistics
GET    /api/v1/matches/:id/lineups

GET    /api/v1/leagues
GET    /api/v1/leagues/:slug
GET    /api/v1/leagues/:slug/standings
GET    /api/v1/leagues/:slug/matches
GET    /api/v1/leagues/:slug/top-scorers
```

#### Teams & Players
```
GET    /api/v1/teams
GET    /api/v1/teams/:slug
GET    /api/v1/teams/:slug/players
GET    /api/v1/teams/:slug/matches
GET    /api/v1/teams/:slug/statistics

GET    /api/v1/players
GET    /api/v1/players/:slug
GET    /api/v1/players/:slug/statistics
GET    /api/v1/players/:slug/transfer-history
```

#### Comments
```
GET    /api/v1/articles/:id/comments
POST   /api/v1/articles/:id/comments
PUT    /api/v1/comments/:id
DELETE /api/v1/comments/:id
POST   /api/v1/comments/:id/like
DELETE /api/v1/comments/:id/like
```

#### Search
```
GET    /api/v1/search?q=manchester&type=all&page=1&limit=20
GET    /api/v1/search/suggestions?q=manc
```

### 3.2 WebSocket Events

```javascript
// Client to Server
socket.emit('subscribe_match', { matchId: 'uuid' });
socket.emit('unsubscribe_match', { matchId: 'uuid' });
socket.emit('subscribe_league', { leagueId: 'uuid' });

// Server to Client
socket.on('match_update', (data) => {
  // { matchId, homeScore, awayScore, status, minute }
});

socket.on('match_event', (data) => {
  // { matchId, type: 'goal'|'card'|'substitution', player, team, minute }
});

socket.on('breaking_news', (data) => {
  // { articleId, title, summary, category }
});
```

### 3.3 GraphQL Schema (Optional)

```graphql
type Query {
  articles(page: Int, limit: Int, category: String): ArticleConnection
  article(slug: String!): Article
  matches(date: String, status: MatchStatus): [Match]
  match(id: ID!): Match
  teams(leagueId: ID): [Team]
  team(slug: String!): Team
  players(teamId: ID): [Player]
  player(slug: String!): Player
}

type Mutation {
  createArticle(input: CreateArticleInput!): Article
  updateArticle(id: ID!, input: UpdateArticleInput!): Article
  deleteArticle(id: ID!): Boolean
  createComment(articleId: ID!, content: String!): Comment
  likeComment(commentId: ID!): Comment
}

type Subscription {
  matchUpdated(matchId: ID!): Match
  newBreakingNews: Article
}
```

## 4. Güvenlik (Security)

### 4.1 Authentication & Authorization
- **JWT Tokens**: Access token (15 min) + Refresh token (7 days)
- **Password Hashing**: bcrypt (salt rounds: 10)
- **OAuth2**: Google, Facebook, Twitter login
- **2FA**: TOTP (Google Authenticator)
- **Session Management**: Redis-based sessions

### 4.2 API Security
- **Rate Limiting**: 
  - Guest: 100 req/15min
  - Authenticated: 1000 req/15min
  - Premium: 5000 req/15min
- **CORS**: Configured whitelist
- **Helmet.js**: Security headers
- **Input Validation**: Joi/Zod schemas
- **SQL Injection**: Parameterized queries (ORM)
- **XSS Protection**: Content sanitization
- **CSRF Protection**: CSRF tokens

### 4.3 Data Protection
- **Encryption at Rest**: Database encryption
- **Encryption in Transit**: TLS 1.3
- **GDPR Compliance**: 
  - Data export
  - Data deletion
  - Consent management
  - Privacy policy
- **Backup**: 
  - Daily automated backups
  - 30-day retention
  - Geo-redundant storage

## 5. Performans Optimizasyonu (Performance Optimization)

### 5.1 Caching Strategy

```javascript
// Cache Levels
1. Browser Cache (Static assets): 1 year
2. CDN Cache (Images, CSS, JS): 1 week
3. API Gateway Cache: 1 minute
4. Redis Cache:
   - Live scores: 10 seconds
   - Match details: 1 minute
   - Articles list: 5 minutes
   - Article detail: 1 hour
   - League standings: 10 minutes
   - User profile: 1 hour

// Cache Invalidation
- On content update: Invalidate specific keys
- On match event: Invalidate match and league caches
- On user update: Invalidate user-specific caches
```

### 5.2 Database Optimization
- **Indexing**: Critical queries indexed
- **Connection Pooling**: Max 20 connections
- **Query Optimization**: Explain analyze for slow queries
- **Partitioning**: Matches and events tables by date
- **Materialized Views**: League standings
- **Read Replicas**: Separate read/write databases

### 5.3 Frontend Optimization
- **Code Splitting**: Route-based splitting
- **Lazy Loading**: Images and components
- **Image Optimization**: 
  - WebP format
  - Responsive images
  - CDN delivery
- **Bundle Size**: < 200KB initial load
- **Lighthouse Score**: > 90 for all metrics

## 6. Monitoring & Logging

### 6.1 Application Monitoring
- **Error Tracking**: Sentry
- **Performance**: New Relic APM
- **Uptime**: UptimeRobot (5-minute checks)
- **Real User Monitoring (RUM)**: Google Analytics, Hotjar

### 6.2 Infrastructure Monitoring
- **Metrics**: Prometheus
- **Visualization**: Grafana dashboards
- **Alerting**: PagerDuty integration
- **Log Aggregation**: ELK Stack or CloudWatch

### 6.3 Key Metrics
- **Response Time**: P95 < 500ms
- **Error Rate**: < 0.1%
- **Uptime**: > 99.9%
- **Database Queries**: P95 < 100ms
- **Cache Hit Rate**: > 80%

## 7. Deployment Strategy

### 7.1 Environments
```
Development  → Local Docker Compose
Staging      → Kubernetes cluster (single node)
Production   → Kubernetes cluster (multi-node, auto-scaling)
```

### 7.2 CI/CD Pipeline
```yaml
# .github/workflows/deploy.yml
1. Code Push to main branch
2. Run Tests (unit, integration)
3. Run Linting (ESLint, Prettier)
4. Security Scan (Snyk, OWASP)
5. Build Docker Images
6. Push to Container Registry
7. Deploy to Staging
8. Run E2E Tests
9. Manual Approval
10. Deploy to Production (Rolling update)
11. Health Check
12. Rollback if failed
```

### 7.3 Scaling Strategy
- **Horizontal Scaling**: Auto-scale based on CPU/Memory
- **Database**: Read replicas for scaling reads
- **Cache**: Redis Cluster for distributed cache
- **CDN**: Global content delivery
- **Load Balancing**: Nginx with least connections algorithm

## 8. Third-Party Integrations

### 8.1 Sports Data APIs
- **Primary**: API-Football (https://www.api-football.com/)
- **Alternative**: TheSportsDB, SportRadar
- **Update Frequency**: 
  - Live matches: 30 seconds
  - Fixtures: Daily
  - Standings: After each match

### 8.2 Services
- **Email**: SendGrid or AWS SES
- **SMS**: Twilio
- **Push Notifications**: Firebase Cloud Messaging
- **Image Storage**: AWS S3 or Cloudinary
- **Video**: Vimeo or Mux
- **Search**: Algolia (optional, for advanced search)
- **Analytics**: Google Analytics 4, Mixpanel
- **Payment**: Stripe, PayPal

## 9. Geliştirme Standartları (Development Standards)

### 9.1 Code Style
- **Language**: TypeScript (strict mode)
- **Linting**: ESLint + Prettier
- **Naming Conventions**: 
  - camelCase for variables/functions
  - PascalCase for classes/components
  - UPPER_SNAKE_CASE for constants
- **Git Commit**: Conventional Commits
- **Git Branch**: feature/, bugfix/, hotfix/ prefixes

### 9.2 Testing
- **Unit Tests**: Jest (>80% coverage)
- **Integration Tests**: Supertest
- **E2E Tests**: Playwright or Cypress
- **Load Testing**: k6 or Artillery

### 9.3 Documentation
- **API Docs**: Swagger/OpenAPI
- **Code Comments**: JSDoc
- **README**: Comprehensive setup guide
- **Architecture Docs**: C4 model diagrams
- **Runbook**: Operations guide

## 10. Maliyet Tahmini (Cost Estimation)

### 10.1 Aylık Altyapı Maliyetleri (Monthly Infrastructure Costs)

#### Başlangıç Aşaması (10K MAU)
- **Server**: DigitalOcean Droplet 4GB - $24/mo
- **Database**: Managed PostgreSQL 2GB - $15/mo
- **Redis**: 1GB - $10/mo
- **Storage**: 100GB S3 - $3/mo
- **CDN**: CloudFlare (Free plan)
- **Domain + SSL**: $15/year
- **Total**: ~$50-60/mo

#### Büyüme Aşaması (100K MAU)
- **Server**: Multiple droplets + Load balancer - $150/mo
- **Database**: 8GB + Read replica - $100/mo
- **Redis**: 4GB - $40/mo
- **Storage**: 500GB - $15/mo
- **CDN**: CloudFlare Pro - $20/mo
- **Monitoring**: New Relic - $99/mo
- **Total**: ~$400-450/mo

#### Olgunluk Aşaması (1M+ MAU)
- **Kubernetes Cluster**: $500/mo
- **Database**: High availability - $500/mo
- **Redis Cluster**: $200/mo
- **Storage**: 2TB - $50/mo
- **CDN**: CloudFlare Business - $200/mo
- **Monitoring & Logging**: $300/mo
- **Total**: ~$1,500-2,000/mo

### 10.2 Üçüncü Parti Servisler
- **Sports API**: $50-500/mo (kullanıma göre)
- **Email (SendGrid)**: $15-100/mo
- **Push Notifications**: $0-50/mo
- **Image Optimization**: $20-100/mo
