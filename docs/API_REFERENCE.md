# Sports News Platform - API Reference / API Referansı

## Genel Bilgiler (General Information)

### Base URL
```
Development: http://localhost:3000/api/v1
Staging: https://staging-api.sportnews.com/api/v1
Production: https://api.sportnews.com/api/v1
```

### Authentication
Tüm korumalı endpoint'ler için Authorization header gereklidir:
```
Authorization: Bearer <access_token>
```

### Response Format
```json
{
  "success": true,
  "data": {},
  "message": "Success message",
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 100,
    "pages": 5
  }
}
```

### Error Response
```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input",
    "details": []
  }
}
```

### HTTP Status Codes
- `200` - Success
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `429` - Too Many Requests
- `500` - Internal Server Error

## Authentication Endpoints

### Register
```http
POST /auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "username": "johndoe",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "user": {
      "id": "uuid",
      "email": "user@example.com",
      "username": "johndoe",
      "firstName": "John",
      "lastName": "Doe"
    },
    "tokens": {
      "accessToken": "jwt_token",
      "refreshToken": "refresh_token",
      "expiresIn": 900
    }
  }
}
```

### Login
```http
POST /auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

### Refresh Token
```http
POST /auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "refresh_token"
}
```

### Logout
```http
POST /auth/logout
Authorization: Bearer <access_token>
```

## Articles Endpoints

### Get Articles
```http
GET /articles?page=1&limit=20&category=football&sort=-publishedAt&search=messi

Query Parameters:
- page: integer (default: 1)
- limit: integer (default: 20, max: 100)
- category: string (slug)
- sort: string (-publishedAt, publishedAt, -views, views)
- search: string
- tags: string[] (comma separated)
- featured: boolean
- breaking: boolean
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "uuid",
      "title": "Messi Scores Hat-trick",
      "slug": "messi-scores-hat-trick",
      "summary": "Lionel Messi scored three goals...",
      "content": "Full article content...",
      "featuredImage": "https://cdn.example.com/image.jpg",
      "author": {
        "id": "uuid",
        "username": "sports_editor",
        "firstName": "Jane",
        "lastName": "Smith"
      },
      "category": {
        "id": "uuid",
        "name": "Football",
        "slug": "football"
      },
      "tags": ["messi", "hat-trick", "la-liga"],
      "viewsCount": 15420,
      "isFeatured": true,
      "isBreaking": false,
      "publishedAt": "2024-11-01T15:30:00Z",
      "createdAt": "2024-11-01T15:00:00Z",
      "updatedAt": "2024-11-01T15:30:00Z"
    }
  ],
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 250,
    "pages": 13
  }
}
```

### Get Article by Slug
```http
GET /articles/:slug

Example: GET /articles/messi-scores-hat-trick
```

### Create Article (Admin/Editor only)
```http
POST /articles
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "title": "Breaking: Transfer News",
  "summary": "Player X moves to Team Y",
  "content": "Full article content with HTML...",
  "featuredImage": "https://cdn.example.com/image.jpg",
  "categoryId": "uuid",
  "tags": ["transfer", "premier-league"],
  "status": "published",
  "isFeatured": true,
  "isBreaking": true,
  "metaTitle": "SEO Title",
  "metaDescription": "SEO Description",
  "metaKeywords": ["keyword1", "keyword2"]
}
```

### Update Article
```http
PUT /articles/:id
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "title": "Updated Title",
  "content": "Updated content..."
}
```

### Delete Article
```http
DELETE /articles/:id
Authorization: Bearer <access_token>
```

## Matches Endpoints

### Get Live Matches
```http
GET /matches/live

Response includes all currently live matches with real-time scores
```

**Response:**
```json
{
  "success": true,
  "data": [
    {
      "id": "uuid",
      "league": {
        "id": "uuid",
        "name": "Premier League",
        "logo": "https://cdn.example.com/pl.png"
      },
      "homeTeam": {
        "id": "uuid",
        "name": "Manchester United",
        "shortName": "MUN",
        "logo": "https://cdn.example.com/mun.png"
      },
      "awayTeam": {
        "id": "uuid",
        "name": "Liverpool",
        "shortName": "LIV",
        "logo": "https://cdn.example.com/liv.png"
      },
      "homeScore": 2,
      "awayScore": 1,
      "status": "live",
      "minute": 67,
      "venue": "Old Trafford",
      "matchDate": "2024-11-01T15:00:00Z"
    }
  ]
}
```

### Get Upcoming Matches
```http
GET /matches/upcoming?date=2024-11-01&leagueId=uuid

Query Parameters:
- date: string (YYYY-MM-DD)
- leagueId: string (uuid)
- teamId: string (uuid)
```

### Get Match Details
```http
GET /matches/:id

Returns detailed match information including:
- Match info
- Scores
- Statistics
- Events timeline
- Lineups
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "league": {...},
    "homeTeam": {...},
    "awayTeam": {...},
    "homeScore": 2,
    "awayScore": 1,
    "halfTimeHomeScore": 1,
    "halfTimeAwayScore": 0,
    "status": "finished",
    "venue": "Old Trafford",
    "matchDate": "2024-11-01T15:00:00Z",
    "statistics": {
      "home": {
        "possession": 58,
        "shotsTotal": 15,
        "shotsOnTarget": 8,
        "corners": 6,
        "fouls": 12,
        "yellowCards": 2,
        "redCards": 0
      },
      "away": {...}
    },
    "events": [
      {
        "id": "uuid",
        "type": "goal",
        "team": "home",
        "player": {
          "id": "uuid",
          "name": "Marcus Rashford"
        },
        "minute": 23,
        "detail": "Right foot shot"
      }
    ],
    "lineups": {
      "home": {
        "formation": "4-3-3",
        "startXI": [...],
        "substitutes": [...]
      },
      "away": {...}
    }
  }
}
```

### Get Match Events (Real-time updates)
```http
GET /matches/:id/events

Returns chronological list of match events
```

### Get Match Statistics
```http
GET /matches/:id/statistics
```

## Leagues Endpoints

### Get All Leagues
```http
GET /leagues?sportType=football&country=England

Query Parameters:
- sportType: string (football, basketball, cricket, etc.)
- country: string
- isActive: boolean
```

### Get League Details
```http
GET /leagues/:slug

Example: GET /leagues/premier-league
```

### Get League Standings
```http
GET /leagues/:slug/standings?season=2024-2025

Returns current league table with positions, points, wins, draws, losses, etc.
```

**Response:**
```json
{
  "success": true,
  "data": {
    "league": {...},
    "season": "2024-2025",
    "standings": [
      {
        "position": 1,
        "team": {
          "id": "uuid",
          "name": "Manchester City",
          "logo": "https://cdn.example.com/mci.png"
        },
        "played": 10,
        "won": 8,
        "drawn": 1,
        "lost": 1,
        "goalsFor": 25,
        "goalsAgainst": 8,
        "goalDifference": 17,
        "points": 25,
        "form": ["W", "W", "D", "W", "W"]
      }
    ]
  }
}
```

### Get League Matches
```http
GET /leagues/:slug/matches?status=finished&round=10

Query Parameters:
- status: string (scheduled, live, finished)
- round: string
- date: string (YYYY-MM-DD)
```

### Get Top Scorers
```http
GET /leagues/:slug/top-scorers?season=2024-2025

Returns top goal scorers in the league
```

## Teams Endpoints

### Get All Teams
```http
GET /teams?leagueId=uuid&search=manchester

Query Parameters:
- leagueId: string (uuid)
- country: string
- search: string
```

### Get Team Details
```http
GET /teams/:slug

Returns comprehensive team information
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "name": "Manchester United",
    "slug": "manchester-united",
    "shortName": "MUN",
    "logo": "https://cdn.example.com/mun.png",
    "founded": 1878,
    "stadium": "Old Trafford",
    "city": "Manchester",
    "country": "England",
    "league": {...},
    "statistics": {
      "played": 10,
      "won": 6,
      "drawn": 2,
      "lost": 2,
      "goalsFor": 18,
      "goalsAgainst": 10
    }
  }
}
```

### Get Team Players
```http
GET /teams/:slug/players

Returns current squad
```

### Get Team Matches
```http
GET /teams/:slug/matches?status=finished&limit=10

Returns recent and upcoming matches
```

## Players Endpoints

### Get All Players
```http
GET /players?teamId=uuid&position=forward&search=ronaldo

Query Parameters:
- teamId: string (uuid)
- position: string (goalkeeper, defender, midfielder, forward)
- nationality: string
- search: string
```

### Get Player Details
```http
GET /players/:slug

Returns player profile and statistics
```

**Response:**
```json
{
  "success": true,
  "data": {
    "id": "uuid",
    "firstName": "Cristiano",
    "lastName": "Ronaldo",
    "slug": "cristiano-ronaldo",
    "photo": "https://cdn.example.com/ronaldo.jpg",
    "dateOfBirth": "1985-02-05",
    "nationality": "Portugal",
    "position": "Forward",
    "jerseyNumber": 7,
    "height": 187,
    "weight": 83,
    "team": {...},
    "marketValue": 35000000,
    "statistics": {
      "season": "2024-2025",
      "appearances": 10,
      "goals": 8,
      "assists": 3,
      "yellowCards": 1,
      "redCards": 0,
      "minutesPlayed": 850
    }
  }
}
```

### Get Player Statistics
```http
GET /players/:slug/statistics?season=2024-2025

Returns detailed season statistics
```

### Get Transfer History
```http
GET /players/:slug/transfer-history

Returns player's transfer history
```

## Comments Endpoints

### Get Article Comments
```http
GET /articles/:id/comments?page=1&limit=20&sort=-createdAt

Query Parameters:
- page: integer
- limit: integer
- sort: string (-createdAt, createdAt, -likes, likes)
```

### Create Comment
```http
POST /articles/:id/comments
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "content": "Great article!",
  "parentId": "uuid" // optional, for replies
}
```

### Update Comment
```http
PUT /comments/:id
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "content": "Updated comment text"
}
```

### Delete Comment
```http
DELETE /comments/:id
Authorization: Bearer <access_token>
```

### Like Comment
```http
POST /comments/:id/like
Authorization: Bearer <access_token>
```

### Unlike Comment
```http
DELETE /comments/:id/like
Authorization: Bearer <access_token>
```

## Search Endpoints

### Global Search
```http
GET /search?q=manchester&type=all&page=1&limit=20

Query Parameters:
- q: string (search query, min 2 chars)
- type: string (all, articles, teams, players, leagues)
- page: integer
- limit: integer
```

**Response:**
```json
{
  "success": true,
  "data": {
    "articles": [...],
    "teams": [...],
    "players": [...],
    "leagues": [...]
  },
  "pagination": {...}
}
```

### Search Suggestions
```http
GET /search/suggestions?q=manc

Returns autocomplete suggestions
```

## User Endpoints

### Get Current User
```http
GET /users/me
Authorization: Bearer <access_token>
```

### Update Profile
```http
PUT /users/me
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "bio": "Sports enthusiast",
  "favoriteTeams": ["uuid1", "uuid2"]
}
```

### Upload Avatar
```http
POST /users/me/avatar
Authorization: Bearer <access_token>
Content-Type: multipart/form-data

avatar: <file>
```

### Get User Favorites
```http
GET /users/me/favorites?type=team

Query Parameters:
- type: string (team, league, player)
```

### Add Favorite
```http
POST /users/me/favorites
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "entityType": "team",
  "entityId": "uuid"
}
```

### Remove Favorite
```http
DELETE /users/me/favorites/:favoriteId
Authorization: Bearer <access_token>
```

### Get Notifications
```http
GET /users/me/notifications?page=1&limit=20&unread=true

Query Parameters:
- page: integer
- limit: integer
- unread: boolean
```

### Mark Notification as Read
```http
PUT /notifications/:id/read
Authorization: Bearer <access_token>
```

### Update Notification Preferences
```http
PUT /users/me/notification-preferences
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "matchStart": true,
  "goals": true,
  "breaking news": true,
  "comments": false
}
```

## Admin Endpoints

### Get Dashboard Stats
```http
GET /admin/dashboard/stats
Authorization: Bearer <admin_access_token>

Returns platform statistics
```

### Manage Users
```http
GET /admin/users?page=1&limit=20&role=user&search=john
PUT /admin/users/:id
DELETE /admin/users/:id
```

### Moderate Comments
```http
GET /admin/comments?status=pending&page=1&limit=20
PUT /admin/comments/:id/approve
PUT /admin/comments/:id/reject
```

### Content Analytics
```http
GET /admin/analytics/articles?startDate=2024-10-01&endDate=2024-10-31

Returns article performance metrics
```

## WebSocket Events

### Connect
```javascript
import io from 'socket.io-client';

const socket = io('wss://api.sportnews.com', {
  auth: {
    token: 'jwt_access_token'
  }
});
```

### Subscribe to Match Updates
```javascript
socket.emit('subscribe_match', { matchId: 'uuid' });

socket.on('match_update', (data) => {
  console.log('Match updated:', data);
  // { matchId, homeScore, awayScore, status, minute }
});

socket.on('match_event', (data) => {
  console.log('Match event:', data);
  // { matchId, type, team, player, minute, detail }
});
```

### Subscribe to Breaking News
```javascript
socket.on('breaking_news', (data) => {
  console.log('Breaking news:', data);
  // { articleId, title, summary, category, publishedAt }
});
```

### Unsubscribe
```javascript
socket.emit('unsubscribe_match', { matchId: 'uuid' });
```

## Rate Limiting

### Rate Limits by User Type

| User Type | Requests | Time Window |
|-----------|----------|-------------|
| Guest | 100 | 15 minutes |
| Authenticated | 1000 | 15 minutes |
| Premium | 5000 | 15 minutes |
| Admin | Unlimited | - |

### Rate Limit Headers
```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1698854400
```

### Rate Limit Exceeded Response
```json
{
  "success": false,
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Too many requests. Please try again later.",
    "retryAfter": 900
  }
}
```

## Pagination

### Standard Pagination
```http
GET /articles?page=2&limit=20
```

### Cursor-based Pagination (for real-time feeds)
```http
GET /matches/live?cursor=encoded_cursor
```

## Filtering & Sorting

### Multiple Filters
```http
GET /articles?category=football&tags=messi,barcelona&featured=true
```

### Sorting
```
-fieldName: Descending
fieldName: Ascending

Example: ?sort=-publishedAt,title
```

## Error Codes

| Code | Description |
|------|-------------|
| `VALIDATION_ERROR` | Invalid input data |
| `UNAUTHORIZED` | Missing or invalid authentication |
| `FORBIDDEN` | Insufficient permissions |
| `NOT_FOUND` | Resource not found |
| `CONFLICT` | Resource already exists |
| `RATE_LIMIT_EXCEEDED` | Too many requests |
| `INTERNAL_ERROR` | Server error |
| `SERVICE_UNAVAILABLE` | Service temporarily unavailable |

## Versioning

API versioning is done via URL path:
```
/api/v1/...  (current)
/api/v2/...  (future)
```

Breaking changes will result in a new version. Non-breaking changes will be added to the current version.

## SDKs

Official SDKs available for:
- JavaScript/TypeScript (npm: @sportnews/api-client)
- Python (pip: sportnews-api)
- PHP (composer: sportnews/api-client)
- Mobile (React Native, Flutter)

## Support

- API Documentation: https://docs.sportnews.com/api
- Developer Portal: https://developers.sportnews.com
- Support Email: api-support@sportnews.com
- Status Page: https://status.sportnews.com
