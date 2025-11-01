# Sports News Platform - Project Setup / Proje Kurulumu

## Proje Başlangıç Rehberi

Bu dokuman, Sports News Platform projesini sıfırdan başlatmak için gerekli adımları içerir.

## Teknoloji Stack

### Frontend
- **Framework**: Next.js 14+ (App Router)
- **Language**: TypeScript
- **Styling**: Tailwind CSS
- **UI Components**: Shadcn/ui
- **State Management**: Zustand
- **Data Fetching**: React Query (TanStack Query)
- **Forms**: React Hook Form + Zod

### Backend
- **Runtime**: Node.js 20+ LTS
- **Framework**: NestJS (TypeScript)
- **Database**: PostgreSQL 15+
- **Cache**: Redis 7+
- **ORM**: Prisma or TypeORM
- **Authentication**: Passport.js + JWT
- **Real-time**: Socket.io

### DevOps
- **Containerization**: Docker + Docker Compose
- **CI/CD**: GitHub Actions
- **Hosting**: DigitalOcean / AWS
- **CDN**: CloudFlare
- **Monitoring**: Sentry

## Proje Yapısı

```
sportnewsandscores/
├── apps/
│   ├── web/                    # Next.js frontend
│   │   ├── src/
│   │   │   ├── app/           # App router pages
│   │   │   ├── components/    # React components
│   │   │   ├── lib/           # Utilities
│   │   │   ├── hooks/         # Custom hooks
│   │   │   └── types/         # TypeScript types
│   │   ├── public/            # Static assets
│   │   └── package.json
│   │
│   ├── api/                   # NestJS backend
│   │   ├── src/
│   │   │   ├── modules/       # Feature modules
│   │   │   ├── common/        # Shared code
│   │   │   ├── config/        # Configuration
│   │   │   └── main.ts        # Entry point
│   │   └── package.json
│   │
│   └── mobile/                # React Native app (optional)
│
├── packages/                   # Shared packages
│   ├── ui/                    # Shared UI components
│   ├── types/                 # Shared TypeScript types
│   ├── utils/                 # Shared utilities
│   └── config/                # Shared configs
│
├── docs/                      # Documentation
│   ├── FEATURE_LIST.md
│   ├── TODO_LIST.md
│   ├── TECHNICAL_DOCUMENTATION.md
│   ├── API_REFERENCE.md
│   ├── COMPETITOR_ANALYSIS.md
│   └── PROJECT_SETUP.md
│
├── docker/                    # Docker configurations
│   ├── development/
│   └── production/
│
├── scripts/                   # Utility scripts
├── .github/                   # GitHub workflows
├── docker-compose.yml
├── package.json               # Root package.json
├── turbo.json                 # Turborepo config
└── README.md
```

## Kurulum Adımları

### 1. Gereksinimler

Sisteminizde şunlar yüklü olmalı:
- Node.js 20+ LTS
- npm veya yarn veya pnpm
- Docker ve Docker Compose
- Git
- PostgreSQL 15+ (local development için)
- Redis 7+ (local development için)

### 2. Repository Clone

```bash
git clone https://github.com/gitfcankaya/sportnewsandscores.git
cd sportnewsandscores
```

### 3. Proje Yapısını Oluşturma

#### A. Monorepo Kurulumu (Turborepo)

```bash
# Install Turborepo globally
npm install -g turbo

# Initialize Turborepo
npx create-turbo@latest

# Or use pnpm (recommended)
pnpm dlx create-turbo@latest
```

#### B. Frontend Setup (Next.js)

```bash
cd apps
npx create-next-app@latest web --typescript --tailwind --app --src-dir
cd web

# Install dependencies
pnpm add @tanstack/react-query zustand
pnpm add react-hook-form zod @hookform/resolvers
pnpm add axios
pnpm add framer-motion
pnpm add socket.io-client
pnpm add -D @types/node

# Install Shadcn/ui
npx shadcn-ui@latest init

# Add essential components
npx shadcn-ui@latest add button card input label
npx shadcn-ui@latest add dropdown-menu avatar badge
npx shadcn-ui@latest add dialog sheet tabs
```

#### C. Backend Setup (NestJS)

```bash
cd apps
nest new api --package-manager pnpm

cd api

# Install dependencies
pnpm add @nestjs/config @nestjs/jwt @nestjs/passport
pnpm add @nestjs/typeorm typeorm pg
pnpm add passport passport-jwt bcrypt
pnpm add class-validator class-transformer
pnpm add @nestjs/websockets @nestjs/platform-socket.io
pnpm add ioredis
pnpm add helmet compression

# Install dev dependencies
pnpm add -D @types/passport-jwt @types/bcrypt
pnpm add -D prisma
pnpm add -D @nestjs/testing
```

### 4. Veritabanı Kurulumu

#### A. Docker Compose ile (Önerilen)

`docker-compose.yml` dosyası oluşturun:

```yaml
version: '3.8'

services:
  postgres:
    image: postgres:15-alpine
    container_name: sportnews_postgres
    environment:
      POSTGRES_USER: sportnews
      POSTGRES_PASSWORD: sportnews123
      POSTGRES_DB: sportnews_db
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: unless-stopped

  redis:
    image: redis:7-alpine
    container_name: sportnews_redis
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
    restart: unless-stopped

  postgres_test:
    image: postgres:15-alpine
    container_name: sportnews_postgres_test
    environment:
      POSTGRES_USER: sportnews_test
      POSTGRES_PASSWORD: test123
      POSTGRES_DB: sportnews_test_db
    ports:
      - "5433:5432"
    volumes:
      - postgres_test_data:/var/lib/postgresql/data

volumes:
  postgres_data:
  redis_data:
  postgres_test_data:
```

Başlatma:
```bash
docker-compose up -d
```

#### B. Prisma Setup

```bash
cd apps/api

# Initialize Prisma
npx prisma init

# Update schema.prisma with your models
# Then run:
npx prisma migrate dev --name init
npx prisma generate
```

### 5. Environment Variables

#### Frontend (.env.local)
```env
# API
NEXT_PUBLIC_API_URL=http://localhost:3001/api/v1
NEXT_PUBLIC_WS_URL=ws://localhost:3001

# Auth
NEXT_PUBLIC_JWT_EXPIRATION=15m

# External APIs
NEXT_PUBLIC_GOOGLE_ANALYTICS_ID=G-XXXXXXXXXX

# Feature Flags
NEXT_PUBLIC_ENABLE_FANTASY=false
NEXT_PUBLIC_ENABLE_PREMIUM=false
```

#### Backend (.env)
```env
# Server
NODE_ENV=development
PORT=3001
API_PREFIX=/api/v1

# Database
DATABASE_URL=postgresql://sportnews:sportnews123@localhost:5432/sportnews_db
DATABASE_HOST=localhost
DATABASE_PORT=5432
DATABASE_USER=sportnews
DATABASE_PASSWORD=sportnews123
DATABASE_NAME=sportnews_db

# Redis
REDIS_HOST=localhost
REDIS_PORT=6379
REDIS_PASSWORD=

# JWT
JWT_SECRET=your-super-secret-jwt-key-change-this-in-production
JWT_EXPIRATION=15m
JWT_REFRESH_SECRET=your-refresh-token-secret
JWT_REFRESH_EXPIRATION=7d

# Sports API
SPORTS_API_KEY=your-api-football-key
SPORTS_API_URL=https://api-football-v1.p.rapidapi.com/v3

# Email
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USER=your-email@gmail.com
SMTP_PASSWORD=your-app-password

# Storage
AWS_S3_BUCKET=sportnews-media
AWS_REGION=us-east-1
AWS_ACCESS_KEY_ID=your-access-key
AWS_SECRET_ACCESS_KEY=your-secret-key

# CDN
CDN_URL=https://cdn.sportnews.com

# Monitoring
SENTRY_DSN=your-sentry-dsn
```

### 6. Development

#### Start All Services

```bash
# Using Docker Compose
docker-compose up -d

# Start backend
cd apps/api
pnpm run start:dev

# Start frontend (in another terminal)
cd apps/web
pnpm run dev
```

Tarayıcınızda açın:
- Frontend: http://localhost:3000
- Backend API: http://localhost:3001/api/v1
- API Documentation: http://localhost:3001/api/docs

### 7. Database Migration

```bash
cd apps/api

# Create migration
npx prisma migrate dev --name add_users_table

# Apply migrations
npx prisma migrate deploy

# Seed database (optional)
npx prisma db seed
```

### 8. Testing

```bash
# Backend tests
cd apps/api
pnpm run test              # Unit tests
pnpm run test:e2e          # E2E tests
pnpm run test:cov          # Coverage

# Frontend tests
cd apps/web
pnpm run test
pnpm run test:e2e
```

### 9. Linting & Formatting

```bash
# Lint all projects
turbo lint

# Format all projects
turbo format

# Type check
turbo type-check
```

### 10. Build

```bash
# Build all projects
turbo build

# Build specific app
cd apps/web
pnpm run build

# Build backend
cd apps/api
pnpm run build
```

## Scripts

Root package.json'a eklenecek scripts:

```json
{
  "scripts": {
    "dev": "turbo dev",
    "build": "turbo build",
    "test": "turbo test",
    "lint": "turbo lint",
    "format": "prettier --write \"**/*.{ts,tsx,md}\"",
    "clean": "turbo clean",
    "docker:up": "docker-compose up -d",
    "docker:down": "docker-compose down",
    "db:migrate": "cd apps/api && npx prisma migrate dev",
    "db:seed": "cd apps/api && npx prisma db seed",
    "db:studio": "cd apps/api && npx prisma studio"
  }
}
```

## Deployment

### Production Build

```bash
# Build all
pnpm run build

# Test production build locally
cd apps/web
pnpm start

cd apps/api
node dist/main.js
```

### Docker Production Build

```bash
# Build images
docker build -t sportnews-web -f docker/web/Dockerfile .
docker build -t sportnews-api -f docker/api/Dockerfile .

# Run containers
docker-compose -f docker-compose.prod.yml up -d
```

### Deploy to DigitalOcean

1. Create Droplet (Ubuntu 22.04)
2. Install Docker and Docker Compose
3. Clone repository
4. Set environment variables
5. Run docker-compose

```bash
# On server
git clone https://github.com/gitfcankaya/sportnewsandscores.git
cd sportnewsandscores
cp .env.example .env
# Edit .env with production values
docker-compose -f docker-compose.prod.yml up -d
```

## Monitoring

### Sentry Setup

```bash
# Install Sentry
pnpm add @sentry/node @sentry/nextjs

# Configure Sentry
# Add SENTRY_DSN to .env
```

### Logging

```bash
# Backend logging with Winston
cd apps/api
pnpm add winston
```

## Troubleshooting

### Database Connection Issues
```bash
# Check if PostgreSQL is running
docker ps | grep postgres

# Connect to database
docker exec -it sportnews_postgres psql -U sportnews -d sportnews_db
```

### Port Already in Use
```bash
# Kill process on port 3000
lsof -ti:3000 | xargs kill -9

# Or change port in package.json
```

### Node Modules Issues
```bash
# Clean install
rm -rf node_modules
rm pnpm-lock.yaml
pnpm install
```

## Useful Commands

```bash
# Database commands
pnpm db:migrate          # Run migrations
pnpm db:seed            # Seed database
pnpm db:studio          # Open Prisma Studio

# Docker commands
pnpm docker:up          # Start Docker services
pnpm docker:down        # Stop Docker services
docker-compose logs -f  # View logs

# Development
pnpm dev                # Start all services
pnpm build              # Build all projects
pnpm test               # Run all tests
pnpm lint               # Lint all projects
```

## Next Steps

1. ✅ Set up project structure
2. ✅ Configure databases
3. ✅ Implement authentication
4. ✅ Create basic API endpoints
5. ✅ Design UI components
6. ✅ Integrate sports data API
7. ✅ Implement live score system
8. ✅ Add real-time features
9. ✅ Set up CI/CD pipeline
10. ✅ Deploy to staging

## Resources

- [Next.js Documentation](https://nextjs.org/docs)
- [NestJS Documentation](https://docs.nestjs.com)
- [Prisma Documentation](https://www.prisma.io/docs)
- [Turborepo Documentation](https://turbo.build/repo/docs)
- [Tailwind CSS](https://tailwindcss.com/docs)
- [Shadcn/ui](https://ui.shadcn.com)

## Support

For questions and support:
- GitHub Issues: https://github.com/gitfcankaya/sportnewsandscores/issues
- Email: support@sportnews.com

## License

[Your License Here]
