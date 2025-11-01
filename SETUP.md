# Setup Guide - Sport News and Scores Platform

## Quick Start (5 Minutes)

### Prerequisites Check
```bash
# Check .NET version (should be 9.0+)
dotnet --version

# Check Node.js version (should be 20+)
node --version

# Check npm version
npm --version
```

### Step 1: Start the Backend (2 minutes)

```bash
# Navigate to API directory
cd src/SportNewsAndScores.Api

# Restore packages (if needed)
dotnet restore

# Run the application
dotnet run
```

The API will start on **http://localhost:5245**

You should see:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5245
```

**Database Note:** The application uses LocalDB by default. The database will be created automatically on first run with seed data.

### Step 2: Start the Frontend (3 minutes)

Open a new terminal:

```bash
# Navigate to client directory
cd client

# Install dependencies (first time only)
npm install

# Start development server
npm run dev
```

The frontend will start on **http://localhost:5173**

You should see:
```
  ➜  Local:   http://localhost:5173/
```

### Step 3: Access the Application

Open your browser and go to: **http://localhost:5173**

You should see the Sport News & Scores platform with:
- 📰 News tab (default)
- 🔴 Live Scores tab
- ⚽ Matches tab
- 🏅 Sports tab

## Configuration

### Backend Configuration

Edit `src/SportNewsAndScores.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SportNewsAndScores;Trusted_Connection=true"
  },
  "OpenAI": {
    "ApiKey": ""  // Optional: Add your OpenAI API key
  },
  "Gemini": {
    "ApiKey": ""  // Optional: Add your Gemini API key
  }
}
```

### Frontend Configuration

The API URL is configured in `client/src/services/api.js`:
```javascript
const API_BASE_URL = 'http://localhost:5245/api';
```

## Testing the Application

### Test Backend API

Open a new terminal and test the endpoints:

```bash
# Get all sports
curl http://localhost:5245/api/sports

# Get news
curl http://localhost:5245/api/news

# Get matches
curl http://localhost:5245/api/matches

# Get live matches
curl http://localhost:5245/api/matches/live
```

### Test Frontend

1. Click on different tabs (News, Live Scores, Matches, Sports)
2. Watch for live score auto-refresh (every 30 seconds)
3. Try different filters on the Matches tab
4. Check responsive design by resizing browser window

## Seed Data

The application comes with pre-populated data:

**Sports:**
- ⚽ Football
- 🏀 Basketball
- 🎾 Tennis
- 🏐 Volleyball
- 🏎️ Formula 1

**Countries:**
- 🇹🇷 Turkey
- 🇬🇧 England
- 🇪🇸 Spain
- 🇩🇪 Germany
- 🇫🇷 France
- 🇮🇹 Italy
- 🇧🇷 Brazil
- 🇦🇷 Argentina
- 🇺🇸 USA

**Teams:**
- Galatasaray, Fenerbahçe, Beşiktaş (Turkey)
- Manchester United, Liverpool (England)
- Real Madrid, Barcelona (Spain)

**Sample Data:**
- 2 players
- 2 matches
- 2 news articles

## Troubleshooting

### Backend Issues

**Issue: Database connection failed**
```bash
# Solution: Check SQL Server LocalDB is installed
sqllocaldb info

# If not installed, download from Microsoft
# Or update connection string to use SQL Server Express
```

**Issue: Port 5245 already in use**
```bash
# Solution: Change port in launchSettings.json
# Edit: src/SportNewsAndScores.Api/Properties/launchSettings.json
```

**Issue: Build errors**
```bash
# Solution: Clean and rebuild
dotnet clean
dotnet build
```

### Frontend Issues

**Issue: Cannot connect to API**
```bash
# Solution 1: Verify API is running on port 5245
# Check terminal where you ran 'dotnet run'

# Solution 2: Check CORS configuration
# Verify Program.cs has correct origin: http://localhost:5173
```

**Issue: Port 5173 already in use**
```bash
# Solution: Kill the process or use a different port
# In vite.config.js, add:
# server: { port: 3000 }
```

**Issue: Module not found errors**
```bash
# Solution: Reinstall dependencies
rm -rf node_modules package-lock.json
npm install
```

## Development Tips

### Backend Development

**Hot Reload:**
```bash
# Use watch mode for auto-rebuild
dotnet watch run
```

**View Database:**
```bash
# Connect with SQL Server Management Studio (SSMS)
# Server: (localdb)\mssqllocaldb
# Database: SportNewsAndScores
```

**Add Migration:**
```bash
cd src/SportNewsAndScores.Api
dotnet ef migrations add MigrationName
dotnet ef database update
```

### Frontend Development

**Auto-Reload:**
Vite automatically reloads when you save files.

**Build for Production:**
```bash
cd client
npm run build
# Output in dist/ directory
```

**Lint Code:**
```bash
npm run lint
```

## Optional: AI Configuration

To enable real AI commentary generation:

### OpenAI Setup

1. Get API key from https://platform.openai.com/
2. Add to appsettings.json:
```json
"OpenAI": {
  "ApiKey": "sk-your-key-here"
}
```

### Google Gemini Setup

1. Get API key from https://makersuite.google.com/
2. Add to appsettings.json:
```json
"Gemini": {
  "ApiKey": "AIza-your-key-here"
}
```

**Note:** Without API keys, the system uses simulated AI responses.

## Next Steps

1. **Explore the Code**: Check out the Clean Architecture structure
2. **Read Documentation**: See `/docs` folder for detailed documentation
3. **Customize**: Add your favorite sports teams
4. **Extend**: Add new features from the TODO list
5. **Deploy**: Follow deployment guide in SDD.md

## Support

- Documentation: See `/docs` folder
- Issues: Check GitHub Issues
- Architecture: See `docs/SDS.md`
- API Reference: See `docs/SRS.md`

## Key Files

| File | Purpose |
|------|---------|
| `src/SportNewsAndScores.Api/Program.cs` | API startup and configuration |
| `src/SportNewsAndScores.Infrastructure/Data/DbInitializer.cs` | Seed data |
| `client/src/App.jsx` | Main React component |
| `client/src/services/api.js` | API service layer |
| `appsettings.json` | Backend configuration |

---

**Enjoy using Sport News & Scores!** 🏆
