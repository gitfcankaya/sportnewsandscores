# Software Requirements Specification (SRS)

## Sport News and Scores Platform

### Version: 1.0.0
### Date: November 1, 2025

## 1. Introduction

### 1.1 Purpose
This document specifies the software requirements for the Sport News and Scores platform, a web-based application for aggregating sports news, scores, and information with AI-powered commentary.

### 1.2 Scope
The system includes:
- Backend API built with ASP.NET Core
- SQL Server database with Entity Framework Core
- React-based frontend
- Web crawling service for news aggregation
- AI integration for content generation
- Real-time live score tracking

### 1.3 Definitions, Acronyms, and Abbreviations
- **API**: Application Programming Interface
- **REST**: Representational State Transfer
- **EF Core**: Entity Framework Core
- **CORS**: Cross-Origin Resource Sharing
- **AI**: Artificial Intelligence
- **CRUD**: Create, Read, Update, Delete
- **ORM**: Object-Relational Mapping

### 1.4 References
- ASP.NET Core Documentation
- React Documentation
- Entity Framework Core Documentation
- OpenAI API Documentation
- Google Gemini API Documentation

## 2. Overall Description

### 2.1 Product Perspective
The Sport News and Scores platform is a standalone web application that aggregates data from external sources and provides a unified interface for users to access sports information.

### 2.2 Product Functions
1. Aggregate sports news from multiple sources
2. Track and display live scores
3. Provide match schedules and results
4. Display player and team information
5. Generate AI commentary on sports events
6. Support multiple sports categories
7. Provide mobile-responsive user interface

### 2.3 User Classes and Characteristics
- **End Users**: Sports enthusiasts accessing the platform via web browser
- **Administrators**: System administrators managing content and configuration
- **Developers**: API consumers integrating with the platform

### 2.4 Operating Environment
- **Server**: Windows Server 2019+ or Linux with .NET 9.0 runtime
- **Database**: Microsoft SQL Server 2019+ or Azure SQL Database
- **Client**: Modern web browsers (Chrome 90+, Firefox 88+, Safari 14+, Edge 90+)
- **Mobile**: iOS Safari 14+, Chrome Mobile 90+

### 2.5 Design and Implementation Constraints
- Must use ASP.NET Core 9.0
- Must use Entity Framework Core for data access
- Must use React for frontend
- Must support MSSQL database
- Must respect web scraping ethical guidelines
- Must handle AI API rate limits

## 3. System Features

### 3.1 News Management

#### 3.1.1 Description
System shall provide comprehensive news management including aggregation, storage, and retrieval of sports news articles.

#### 3.1.2 Functional Requirements

**FR-3.1.1**: System shall scrape news from configured sources
- Input: Sport category, language preference
- Process: HTTP request to source, HTML parsing, data extraction
- Output: List of news articles

**FR-3.1.2**: System shall store news articles in database
- Input: News article data (title, content, source URL, image URL, sport, language)
- Process: Validate and save to News table
- Output: Created news entity with ID

**FR-3.1.3**: System shall retrieve news with filtering
- Input: Sport ID (optional), language (optional), pagination parameters
- Process: Query database with filters, order by creation date
- Output: Paginated list of news articles

**FR-3.1.4**: System shall include related entities in news retrieval
- Input: News ID
- Process: Query with Include for Sport, Player, Match, Comments
- Output: News article with full related data

### 3.2 Match Management

#### 3.2.1 Description
System shall manage match information including schedules, live scores, and results.

#### 3.2.2 Functional Requirements

**FR-3.2.1**: System shall store match information
- Input: Sport, home team, away team, date, venue, status
- Process: Create match entity with relationships
- Output: Created match with ID

**FR-3.2.2**: System shall retrieve matches with filtering
- Input: Sport ID (optional), status (optional)
- Process: Query with filters and related entities
- Output: List of matches with team and sport data

**FR-3.2.3**: System shall update live scores
- Input: Match ID, home score, away score
- Process: Update match entity, set status to "Live"
- Output: Updated match entity

**FR-3.2.4**: System shall retrieve live matches only
- Input: None
- Process: Query matches where status = "Live"
- Output: List of currently live matches

### 3.3 AI Commentary Generation

#### 3.3.1 Description
System shall generate AI-powered commentary using OpenAI or Google Gemini.

#### 3.3.2 Functional Requirements

**FR-3.3.1**: System shall generate commentary via OpenAI
- Input: Content text, API key
- Process: POST request to OpenAI API with content
- Output: Generated commentary text

**FR-3.3.2**: System shall generate commentary via Gemini
- Input: Content text, API key
- Process: POST request to Gemini API with content
- Output: Generated commentary text

**FR-3.3.3**: System shall handle AI API failures gracefully
- Input: Content text, provider
- Process: Try AI generation, fall back to template message on failure
- Output: Commentary text (real or simulated)

**FR-3.3.4**: System shall store generated comments
- Input: Comment content, generator (OpenAI/Gemini), news/match ID
- Process: Create comment entity with relationships
- Output: Created comment with ID

### 3.4 Live Score Tracking

#### 3.4.1 Description
System shall provide real-time updates for live sporting events.

#### 3.4.2 Functional Requirements

**FR-3.4.1**: Frontend shall refresh live scores automatically
- Input: None
- Process: Periodic API calls every 30 seconds
- Output: Updated live match data displayed

**FR-3.4.2**: System shall mark matches as live
- Input: Match with current status
- Process: Status validation and update
- Output: Match status = "Live"

**FR-3.4.3**: System shall display live indicator
- Input: Match status
- Process: Visual indicator if status = "Live"
- Output: Animated "LIVE" badge on UI

### 3.5 Player Management

#### 3.5.1 Description
System shall manage player information and profiles.

#### 3.5.2 Functional Requirements

**FR-3.5.1**: System shall store player information
- Input: Name, position, DOB, team, country, photo
- Process: Create player entity with relationships
- Output: Created player with ID

**FR-3.5.2**: System shall retrieve player list
- Input: None
- Process: Query all players with team and country
- Output: List of players with related data

**FR-3.5.3**: System shall retrieve player details
- Input: Player ID
- Process: Query player with team, country, and related news
- Output: Complete player profile

### 3.6 Sports Categories

#### 3.6.1 Description
System shall support multiple sports categories with extensibility.

#### 3.6.2 Functional Requirements

**FR-3.6.1**: System shall store sport categories
- Input: Sport name, Turkish name, icon
- Process: Create sport entity
- Output: Created sport with ID

**FR-3.6.2**: System shall retrieve all sports
- Input: None
- Process: Query all sports
- Output: List of available sports

**FR-3.6.3**: System shall enforce unique sport names
- Input: Sport name
- Process: Database unique constraint validation
- Output: Error if duplicate, success if unique

## 4. External Interface Requirements

### 4.1 User Interfaces

#### 4.1.1 General UI Requirements
- **UI-4.1.1**: Interface shall be responsive (desktop, tablet, mobile)
- **UI-4.1.2**: Interface shall use modern card-based design
- **UI-4.1.3**: Interface shall provide visual feedback for loading states
- **UI-4.1.4**: Interface shall display error messages clearly
- **UI-4.1.5**: Interface shall use intuitive navigation

#### 4.1.2 Specific UI Components

**News List View**
- Display news cards with image, title, excerpt, sport, date
- Filter by sport category
- Responsive grid layout

**Live Scores View**
- Display live match cards with teams, scores, status
- Real-time updates every 30 seconds
- Visual "LIVE" indicator with animation

**Matches View**
- Display match cards with teams, scores, date, venue
- Filter by status (All, Upcoming, Live, Finished)
- Color-coded status indicators

**Sports View**
- Display sport category cards with icon and name
- Grid layout with hover effects

### 4.2 Hardware Interfaces
- None (web-based application)

### 4.3 Software Interfaces

#### 4.3.1 Database Interface
- **System**: Microsoft SQL Server
- **Connection**: ADO.NET via Entity Framework Core
- **Operations**: CRUD operations on all entities
- **Transactions**: Automatic transaction management by EF Core

#### 4.3.2 OpenAI API Interface
- **Endpoint**: https://api.openai.com/v1/chat/completions
- **Method**: POST
- **Authentication**: Bearer token in Authorization header
- **Request**: JSON with model and messages
- **Response**: JSON with generated content

#### 4.3.3 Google Gemini API Interface
- **Endpoint**: https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent
- **Method**: POST
- **Authentication**: API key in query parameter
- **Request**: JSON with contents
- **Response**: JSON with generated content

#### 4.3.4 Web Scraping Interface
- **Protocol**: HTTP/HTTPS
- **Library**: HtmlAgilityPack
- **Method**: GET
- **Process**: HTML parsing with XPath selectors
- **Error Handling**: Try-catch with graceful failure

### 4.4 Communications Interfaces

#### 4.4.1 HTTP REST API
- **Protocol**: HTTP/1.1, HTTPS
- **Format**: JSON
- **Methods**: GET, POST, PUT, DELETE
- **CORS**: Enabled for configured origins
- **Authentication**: None (future enhancement)

#### 4.4.2 WebSocket (Future)
- For real-time score updates
- Not implemented in version 1.0.0

## 5. Non-Functional Requirements

### 5.1 Performance Requirements

**NFR-5.1.1**: API responses shall complete within 500ms for 95% of requests

**NFR-5.1.2**: Database queries shall be optimized with proper indexing

**NFR-5.1.3**: Frontend shall load initial page within 2 seconds

**NFR-5.1.4**: Live score refresh shall occur every 30 seconds without blocking UI

**NFR-5.1.5**: System shall support minimum 100 concurrent users

### 5.2 Safety Requirements

**NFR-5.2.1**: System shall validate all user inputs

**NFR-5.2.2**: System shall prevent SQL injection via parameterized queries

**NFR-5.2.3**: System shall handle exceptions without exposing sensitive information

**NFR-5.2.4**: System shall log errors for debugging without exposing to users

### 5.3 Security Requirements

**NFR-5.3.1**: API keys shall be stored in configuration, not in code

**NFR-5.3.2**: HTTPS shall be enforced in production

**NFR-5.3.3**: CORS shall be restricted to known origins

**NFR-5.3.4**: Database connection strings shall be encrypted

**NFR-5.3.5**: Sensitive data shall not be logged

### 5.4 Software Quality Attributes

#### 5.4.1 Reliability
- System shall have 99.5% uptime
- System shall handle failures gracefully
- System shall recover from crashes automatically

#### 5.4.2 Maintainability
- Code shall follow Clean Architecture principles
- Code shall have proper separation of concerns
- Code shall include inline documentation
- Code shall follow C# and JavaScript conventions

#### 5.4.3 Usability
- UI shall be intuitive requiring no training
- UI shall provide clear feedback
- UI shall be accessible on all target devices

#### 5.4.4 Scalability
- System shall support horizontal scaling
- Database shall support scaling to 1M+ records
- API shall support increased load with minimal changes

## 6. Other Requirements

### 6.1 Database Requirements

**DR-6.1.1**: Database shall use SQL Server 2019 or later

**DR-6.1.2**: Database shall have automated backups

**DR-6.1.3**: Database shall use foreign key constraints

**DR-6.1.4**: Database shall have indexes on frequently queried columns

**DR-6.1.5**: Database shall include seed data for initial setup

### 6.2 Deployment Requirements

**DR-6.2.1**: Backend shall support deployment to Windows or Linux

**DR-6.2.2**: Frontend shall be deployable to static hosting (CDN)

**DR-6.2.3**: System shall use environment-specific configuration

**DR-6.2.4**: System shall support containerization (Docker)

### 6.3 Legal Requirements

**DR-6.3.1**: Web scraping shall respect robots.txt

**DR-6.3.2**: System shall not violate copyright laws

**DR-6.3.3**: System shall include proper attribution for scraped content

**DR-6.3.4**: System shall comply with GDPR (future, when storing user data)

## 7. Appendix

### 7.1 Entity Relationship Diagram

```
Sport (1) ----< (N) News
Sport (1) ----< (N) Match

Country (1) ----< (N) Team
Country (1) ----< (N) Player

Team (1) ----< (N) Player
Team (1) ----< (N) Match (HomeTeam)
Team (1) ----< (N) Match (AwayTeam)

Player (1) ----< (N) News

Match (1) ----< (N) News
Match (1) ----< (N) Comment

News (1) ----< (N) Comment
```

### 7.2 API Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/sports | Get all sports |
| GET | /api/sports/{id} | Get sport by ID |
| GET | /api/news | Get news with filters |
| GET | /api/news/{id} | Get news by ID |
| POST | /api/news | Create news |
| GET | /api/matches | Get matches with filters |
| GET | /api/matches/live | Get live matches |
| GET | /api/matches/{id} | Get match by ID |
| PUT | /api/matches/{id}/score | Update match score |
| GET | /api/players | Get all players |
| GET | /api/players/{id} | Get player by ID |
| POST | /api/comments/generate | Generate AI comment |

### 7.3 Technology Stack

**Backend:**
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- SQL Server
- HtmlAgilityPack

**Frontend:**
- React 18
- Vite
- Axios
- CSS3

**External Services:**
- OpenAI GPT-3.5-turbo
- Google Gemini Pro

### 7.4 Configuration Parameters

| Parameter | Description | Default |
|-----------|-------------|---------|
| ConnectionStrings:DefaultConnection | SQL Server connection | LocalDB |
| OpenAI:ApiKey | OpenAI API key | Empty (optional) |
| Gemini:ApiKey | Google Gemini API key | Empty (optional) |
| AllowedOrigins | CORS allowed origins | localhost:3000, localhost:5173 |
