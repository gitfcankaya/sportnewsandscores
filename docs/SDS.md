# Software Design Specification (SDS)

## Sport News and Scores Platform

### Version: 1.0.0
### Date: November 1, 2025

## 1. Introduction

### 1.1 Purpose
This document provides a comprehensive architectural overview of the Sport News and Scores platform, presenting design decisions, architectural patterns, and technical implementation details.

### 1.2 Scope
This document covers the design of:
- System architecture
- Component design
- Data model design
- API design
- Frontend architecture
- Integration points

## 2. System Architecture

### 2.1 Architectural Style
The system follows a **Three-Tier Architecture** pattern:
- **Presentation Layer**: React SPA
- **Application Layer**: ASP.NET Core Web API
- **Data Layer**: SQL Server with EF Core

### 2.2 Architectural Patterns

#### 2.2.1 Clean Architecture
The backend follows Clean Architecture principles:

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│      (SportNewsAndScores.Api)       │
│  Controllers, Program.cs, Config    │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│      Infrastructure Layer           │
│ (SportNewsAndScores.Infrastructure) │
│  DbContext, Services, Migrations    │
└──────────────┬──────────────────────┘
               │
┌──────────────▼──────────────────────┐
│         Domain Layer                │
│     (SportNewsAndScores.Core)       │
│   Entities, Interfaces, Business    │
└─────────────────────────────────────┘
```

**Benefits:**
- Separation of concerns
- Testability
- Maintainability
- Independence from frameworks

#### 2.2.2 Repository Pattern (via EF Core DbContext)
- DbContext acts as a repository
- Provides abstraction over data access
- Supports unit of work pattern

#### 2.2.3 Dependency Injection
- Built-in ASP.NET Core DI container
- Services registered in Program.cs
- Promotes loose coupling

### 2.3 Component Diagram

```
┌──────────────────────────────────────────┐
│            React Frontend                │
│  Components: NewsList, MatchesList,      │
│  SportsList, LiveScores                  │
└────────┬─────────────────────────────────┘
         │ HTTP/REST
         │
┌────────▼─────────────────────────────────┐
│         ASP.NET Core Web API             │
│  Controllers: News, Matches, Sports,     │
│  Players, Comments                       │
└────┬───────────────────┬─────────────────┘
     │                   │
     │ Services          │ DbContext
     │                   │
┌────▼──────────┐  ┌────▼──────────────────┐
│   External    │  │    SQL Server DB       │
│   Services    │  │  Tables: Sports, News, │
│ - OpenAI API  │  │  Matches, Players, etc.│
│ - Gemini API  │  │                        │
│ - Web Sources │  │                        │
└───────────────┘  └────────────────────────┘
```

## 3. Detailed Design

### 3.1 Backend Design

#### 3.1.1 Project Structure

**SportNewsAndScores.Core (Domain Layer)**
```
/Entities
  - BaseEntity.cs (abstract base)
  - Sport.cs
  - Country.cs
  - Team.cs
  - Player.cs
  - Match.cs
  - News.cs
  - Comment.cs
/Interfaces
  - INewsScraperService.cs
  - IAICommentService.cs
  - ILiveScoreService.cs
```

**SportNewsAndScores.Infrastructure (Data & Services Layer)**
```
/Data
  - ApplicationDbContext.cs
  - DbInitializer.cs
/Services
  - NewsScraperService.cs
  - AICommentService.cs
  - LiveScoreService.cs
```

**SportNewsAndScores.Api (Presentation Layer)**
```
/Controllers
  - NewsController.cs
  - MatchesController.cs
  - SportsController.cs
  - PlayersController.cs
  - CommentsController.cs
- Program.cs
- appsettings.json
```

#### 3.1.2 Entity Design

**Base Entity Pattern:**
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
```

**Benefits:**
- Consistent ID strategy
- Automatic timestamp management
- Reduced code duplication

**Entity Relationships:**
- One-to-Many: Sport → News, Sport → Match
- One-to-Many: Country → Team, Country → Player
- One-to-Many: Team → Player
- Many-to-One: Match → HomeTeam, Match → AwayTeam
- One-to-Many: News → Comment, Match → Comment

#### 3.1.3 Service Design

**INewsScraperService:**
```csharp
public interface INewsScraperService
{
    Task<List<News>> ScrapeNewsAsync(string sport, string language);
}
```

**Implementation Strategy:**
- Use HttpClient for web requests
- Parse HTML with HtmlAgilityPack
- Extract data using XPath selectors
- Handle errors gracefully

**IAICommentService:**
```csharp
public interface IAICommentService
{
    Task<string> GenerateCommentAsync(string content, string provider);
}
```

**Implementation Strategy:**
- Support multiple AI providers (OpenAI, Gemini)
- Fallback to simulated responses if API unavailable
- Rate limiting awareness
- Error handling

**ILiveScoreService:**
```csharp
public interface ILiveScoreService
{
    Task<List<Match>> GetLiveMatchesAsync();
    Task UpdateMatchScoreAsync(int matchId, int homeScore, int awayScore);
}
```

**Implementation Strategy:**
- Query matches with status "Live"
- Update scores atomically
- Include related entities for efficiency

#### 3.1.4 Controller Design

**RESTful Principles:**
- Use appropriate HTTP methods (GET, POST, PUT, DELETE)
- Return proper status codes (200, 201, 404, 500)
- Use resource-based URLs
- Include related entities where needed

**Example: NewsController**
```csharp
[ApiController]
[Route("api/[controller]")]
public class NewsController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<News>>> GetNews(
        [FromQuery] int? sportId, [FromQuery] string? language)
    
    [HttpGet("{id}")]
    public async Task<ActionResult<News>> GetNews(int id)
    
    [HttpPost]
    public async Task<ActionResult<News>> CreateNews(News news)
}
```

### 3.2 Database Design

#### 3.2.1 Database Schema

**Sports Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| Name | NVARCHAR(100) | NOT NULL, Unique |
| NameTr | NVARCHAR(100) | NOT NULL |
| Icon | NVARCHAR(10) | NOT NULL |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**Countries Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| Name | NVARCHAR(100) | NOT NULL |
| Code | NVARCHAR(10) | NOT NULL, Unique |
| FlagUrl | NVARCHAR(200) | NOT NULL |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**Teams Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| Name | NVARCHAR(200) | NOT NULL |
| LogoUrl | NVARCHAR(500) | NOT NULL |
| CountryId | INT | FK → Countries |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**Players Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| FirstName | NVARCHAR(100) | NOT NULL |
| LastName | NVARCHAR(100) | NOT NULL |
| Position | NVARCHAR(50) | NOT NULL |
| DateOfBirth | DATETIME2 | NOT NULL |
| PhotoUrl | NVARCHAR(500) | NOT NULL |
| TeamId | INT | FK → Teams (nullable) |
| CountryId | INT | FK → Countries |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**Matches Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| SportId | INT | FK → Sports |
| HomeTeamId | INT | FK → Teams |
| AwayTeamId | INT | FK → Teams |
| MatchDate | DATETIME2 | NOT NULL |
| HomeScore | INT | NULL |
| AwayScore | INT | NULL |
| Status | NVARCHAR(50) | NOT NULL |
| Venue | NVARCHAR(200) | NOT NULL |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**News Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| Title | NVARCHAR(500) | NOT NULL |
| Content | NVARCHAR(MAX) | NOT NULL |
| SourceUrl | NVARCHAR(1000) | NOT NULL |
| ImageUrl | NVARCHAR(1000) | NOT NULL |
| Language | NVARCHAR(10) | NOT NULL |
| SportId | INT | FK → Sports |
| PlayerId | INT | FK → Players (nullable) |
| MatchId | INT | FK → Matches (nullable) |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

**Comments Table**
| Column | Type | Constraints |
|--------|------|-------------|
| Id | INT | PK, Identity |
| Content | NVARCHAR(MAX) | NOT NULL |
| GeneratedBy | NVARCHAR(50) | NOT NULL |
| NewsId | INT | FK → News (nullable) |
| MatchId | INT | FK → Matches (nullable) |
| CreatedAt | DATETIME2 | NOT NULL |
| UpdatedAt | DATETIME2 | NOT NULL |

#### 3.2.2 Database Indexes

**Performance Optimization:**
- Unique index on Sports.Name
- Unique index on Countries.Code
- Index on News.SportId for filtering
- Index on News.CreatedAt for sorting
- Index on Matches.Status for live queries
- Index on Matches.MatchDate for sorting

#### 3.2.3 Entity Framework Configuration

**OnModelCreating:**
```csharp
modelBuilder.Entity<Match>()
    .HasOne(m => m.HomeTeam)
    .WithMany(t => t.HomeMatches)
    .HasForeignKey(m => m.HomeTeamId)
    .OnDelete(DeleteBehavior.Restrict);

modelBuilder.Entity<Match>()
    .HasOne(m => m.AwayTeam)
    .WithMany(t => t.AwayMatches)
    .HasForeignKey(m => m.AwayTeamId)
    .OnDelete(DeleteBehavior.Restrict);
```

**Automatic Timestamps:**
```csharp
public override async Task<int> SaveChangesAsync(...)
{
    var entries = ChangeTracker.Entries()
        .Where(e => e.Entity is BaseEntity && ...);
    
    foreach (var entry in entries)
    {
        if (entry.State == EntityState.Added)
            entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;
    }
    return await base.SaveChangesAsync(...);
}
```

### 3.3 Frontend Design

#### 3.3.1 Component Architecture

**Component Hierarchy:**
```
App
├── Header
├── Navigation (Tabs)
├── Main Content
│   ├── NewsList
│   ├── LiveScores
│   ├── MatchesList
│   └── SportsList
└── Footer
```

**Component Responsibilities:**

**App.jsx**
- Root component
- Tab navigation state
- Layout structure

**NewsList.jsx**
- Fetch news from API
- Display news cards
- Handle loading and errors

**LiveScores.jsx**
- Fetch live matches
- Auto-refresh every 30 seconds
- Display live indicators

**MatchesList.jsx**
- Fetch all matches
- Status filtering
- Display match cards

**SportsList.jsx**
- Fetch sports categories
- Display sport cards
- Icon and name display

#### 3.3.2 State Management

**Local State (useState):**
- Component-specific data
- Loading states
- Filter selections

**Example:**
```javascript
const [news, setNews] = useState([]);
const [loading, setLoading] = useState(true);
const [selectedSport, setSelectedSport] = useState(null);
```

**Side Effects (useEffect):**
- Data fetching
- Subscriptions (auto-refresh)
- Cleanup

**Example:**
```javascript
useEffect(() => {
  const fetchLiveMatches = async () => { ... };
  fetchLiveMatches();
  const interval = setInterval(fetchLiveMatches, 30000);
  return () => clearInterval(interval);
}, []);
```

#### 3.3.3 API Integration

**Service Layer (api.js):**
```javascript
const API_BASE_URL = 'http://localhost:5000/api';

export const sportService = {
  getAll: () => api.get('/sports'),
  getById: (id) => api.get(`/sports/${id}`)
};
```

**Benefits:**
- Centralized API configuration
- Reusable service functions
- Easy to modify endpoints

#### 3.3.4 Responsive Design Strategy

**Mobile-First Approach:**
- Base styles for mobile
- Media queries for larger screens

**Breakpoints:**
- Mobile: < 480px
- Tablet: 481px - 768px
- Desktop: > 768px

**Grid Layouts:**
```css
.news-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

@media (max-width: 768px) {
  .news-grid {
    grid-template-columns: 1fr;
  }
}
```

## 4. Integration Design

### 4.1 OpenAI Integration

**Flow:**
1. Receive content to comment on
2. Prepare request with system and user prompts
3. POST to OpenAI API with Authorization header
4. Parse response and extract content
5. Handle errors with fallback

**Configuration:**
```json
{
  "OpenAI": {
    "ApiKey": "sk-..."
  }
}
```

### 4.2 Google Gemini Integration

**Flow:**
1. Receive content to comment on
2. Prepare request with content parts
3. POST to Gemini API with key in URL
4. Parse response and extract text
5. Handle errors with fallback

**Configuration:**
```json
{
  "Gemini": {
    "ApiKey": "AIza..."
  }
}
```

### 4.3 Web Scraping Integration

**Flow:**
1. Construct URL for sport and language
2. HTTP GET request to source
3. Parse HTML with HtmlAgilityPack
4. Extract title, link, image with XPath
5. Transform to News entities
6. Handle errors gracefully

**Error Handling:**
- Network failures: Return empty list
- Parsing errors: Skip problematic nodes
- No data found: Return empty list

## 5. Security Design

### 5.1 API Security
- CORS configured for specific origins
- Input validation on all endpoints
- Parameterized queries via EF Core
- No sensitive data in logs

### 5.2 Configuration Security
- API keys in appsettings (not in code)
- Connection strings encrypted in production
- Environment-specific configurations

### 5.3 Future Security Enhancements
- Authentication (JWT tokens)
- Authorization (role-based)
- Rate limiting
- Input sanitization
- CSRF protection

## 6. Performance Design

### 6.1 Database Performance
- Indexes on frequently queried columns
- Eager loading with Include() for related entities
- Pagination for large result sets
- Connection pooling (built-in with EF Core)

### 6.2 API Performance
- Async/await throughout
- Efficient LINQ queries
- HTTP client reuse
- Response caching (future)

### 6.3 Frontend Performance
- Code splitting (via Vite)
- Lazy loading images
- Debounced search (future)
- Memoization for expensive computations

## 7. Scalability Design

### 7.1 Horizontal Scaling
- Stateless API design
- Database connection pooling
- Load balancer ready

### 7.2 Vertical Scaling
- Efficient resource usage
- Optimized queries
- Minimal memory footprint

### 7.3 Future Scalability
- Caching layer (Redis)
- Message queue (RabbitMQ)
- Microservices architecture
- CDN for static assets

## 8. Error Handling Design

### 8.1 Backend Error Handling
- Try-catch in service methods
- Return appropriate HTTP status codes
- Log errors for diagnostics
- Generic error messages to clients

### 8.2 Frontend Error Handling
- Try-catch in async functions
- Display user-friendly error messages
- Loading and error states
- Graceful degradation

## 9. Deployment Architecture

### 9.1 Development Environment
- LocalDB for database
- Vite dev server for frontend
- .NET development server for API

### 9.2 Production Environment (Proposed)
```
[ Load Balancer ]
       |
[ API Servers ] x N
       |
[ SQL Server ]

[ CDN ] → [ Static Frontend ]
```

### 9.3 Containerization (Future)
- Docker containers for API
- Docker Compose for local development
- Kubernetes for orchestration

## 10. Testing Strategy

### 10.1 Unit Testing (Future)
- Test service methods
- Test controller actions
- Mock dependencies

### 10.2 Integration Testing (Future)
- Test API endpoints
- Test database operations
- Test external integrations

### 10.3 UI Testing (Future)
- Component testing with React Testing Library
- E2E testing with Playwright

## 11. Monitoring and Logging

### 11.1 Logging Strategy
- Use built-in .NET logging
- Log levels: Information, Warning, Error
- Structured logging
- No sensitive data in logs

### 11.2 Monitoring (Future)
- Application Insights
- Health check endpoints
- Performance metrics
- Error tracking

## 12. Design Patterns Used

| Pattern | Location | Purpose |
|---------|----------|---------|
| Clean Architecture | Overall | Separation of concerns |
| Repository | DbContext | Data access abstraction |
| Dependency Injection | DI Container | Loose coupling |
| Factory | Service creation | Object creation |
| Strategy | AI providers | Interchangeable algorithms |
| Template Method | BaseEntity | Common behavior |
| Observer | React useEffect | State changes |

## 13. Technology Decisions

| Decision | Technology | Rationale |
|----------|------------|-----------|
| Backend Framework | ASP.NET Core 9.0 | Modern, high-performance, cross-platform |
| ORM | Entity Framework Core | Productivity, LINQ, migrations |
| Database | SQL Server | Reliability, ACID compliance, familiarity |
| Frontend | React 18 | Component-based, large ecosystem, popularity |
| Build Tool | Vite | Fast, modern, ESM support |
| HTTP Client | Axios | Intuitive API, interceptors, error handling |
| HTML Parser | HtmlAgilityPack | .NET native, robust, well-maintained |

## 14. Appendices

### 14.1 Naming Conventions
- Classes: PascalCase
- Methods: PascalCase
- Properties: PascalCase
- Variables: camelCase
- Constants: UPPER_CASE
- Files: Match class name
- Database: PascalCase

### 14.2 Code Style
- C#: Follow Microsoft conventions
- JavaScript: ES6+, functional components
- CSS: BEM-inspired, component-scoped
