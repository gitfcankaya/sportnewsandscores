# Sport News and Scores Platform 🏆

A comprehensive web application for aggregating sports news, live scores, match results, and player information from around the world. Features AI-powered commentary using OpenAI and Google Gemini.

## ✨ Features

- 📰 **Multi-Sport News Aggregation** - Collect sports news from worldwide sources via web crawling
- 🔴 **Live Score Tracking** - Real-time score updates for ongoing matches (refreshes every 30 seconds)
- ⚽ **Match Management** - View schedules, results, and detailed match information
- 🤖 **AI-Powered Commentary** - Generate intelligent commentary using OpenAI GPT and Google Gemini
- 👥 **Player Profiles** - Detailed information about players, teams, and countries
- 🌍 **Multi-Language Support** - Content available in multiple languages
- 📱 **Mobile Responsive** - Modern, mobile-first design that works on all devices
- 🎨 **Modern UI** - Clean, card-based interface with smooth animations

## 🏅 Supported Sports

- ⚽ Football (Soccer)
- 🏀 Basketball
- 🎾 Tennis
- 🏐 Volleyball
- 🏎️ Formula 1

## 🛠️ Technology Stack

### Backend
- **.NET 9.0** - Modern, high-performance framework
- **ASP.NET Core Web API** - RESTful API
- **Entity Framework Core 9.0** - ORM for data access
- **SQL Server** - Relational database
- **HtmlAgilityPack** - Web scraping library
- **Clean Architecture** - Maintainable code structure

### Frontend
- **React 18** - Modern UI library
- **Vite** - Fast build tool
- **Axios** - HTTP client
- **CSS3** - Custom responsive styling

### AI Integration
- **OpenAI GPT-3.5-turbo** - Content generation
- **Google Gemini Pro** - Alternative AI provider

## 🚀 Quick Start

### Prerequisites

- .NET 9.0 SDK
- Node.js 20+ with npm
- SQL Server (LocalDB, Express, or Developer Edition)

### Installation

1. **Clone the repository**
```bash
git clone <repository-url>
cd sportnewsandscores
```

2. **Setup Backend**
```bash
# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Update database connection string in appsettings.json if needed
# Run the API
cd src/SportNewsAndScores.Api
dotnet run
```

The API will be available at `http://localhost:5000`

3. **Setup Frontend**
```bash
# Navigate to client directory
cd client

# Install dependencies
npm install

# Start development server
npm run dev
```

The frontend will be available at `http://localhost:5173`

### Configuration

#### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SportNewsAndScores;Trusted_Connection=true"
  },
  "OpenAI": {
    "ApiKey": "your-openai-key-here-optional"
  },
  "Gemini": {
    "ApiKey": "your-gemini-key-here-optional"
  }
}
```

> **Note**: AI API keys are optional. The system will use simulated responses if keys are not provided.

## 📖 Documentation

Comprehensive documentation is available in the `/docs` directory:

- **[PRD.md](docs/PRD.md)** - Product Requirements Document
- **[SRS.md](docs/SRS.md)** - Software Requirements Specification
- **[SDS.md](docs/SDS.md)** - Software Design Specification
- **[SDD.md](docs/SDD.md)** - Software Development Document
- **[TODO.md](docs/TODO.md)** - Project TODO List and Roadmap

## 🏗️ Project Structure

```
sportnewsandscores/
├── src/
│   ├── SportNewsAndScores.Api/          # Web API project
│   │   ├── Controllers/                 # API controllers
│   │   └── Program.cs                   # Application entry point
│   ├── SportNewsAndScores.Core/         # Domain layer
│   │   ├── Entities/                    # Domain entities
│   │   └── Interfaces/                  # Service interfaces
│   └── SportNewsAndScores.Infrastructure/ # Infrastructure layer
│       ├── Data/                        # DbContext and database
│       └── Services/                    # Service implementations
├── client/                              # React frontend
│   ├── src/
│   │   ├── components/                  # React components
│   │   ├── services/                    # API services
│   │   ├── App.jsx                      # Main App component
│   │   └── App.css                      # Styling
│   └── package.json
├── docs/                                # Documentation
└── SportNewsAndScores.sln               # Solution file
```

## 🎯 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/sports` | Get all sports categories |
| GET | `/api/news` | Get news (filterable by sport, language) |
| GET | `/api/news/{id}` | Get specific news article |
| GET | `/api/matches` | Get matches (filterable by sport, status) |
| GET | `/api/matches/live` | Get only live matches |
| GET | `/api/matches/{id}` | Get specific match |
| PUT | `/api/matches/{id}/score` | Update match score |
| GET | `/api/players` | Get all players |
| GET | `/api/players/{id}` | Get specific player |
| POST | `/api/comments/generate` | Generate AI commentary |

## 🎨 Screenshots

*Coming soon...*

## 🔮 Future Enhancements

- User authentication and profiles
- Favorite teams and players
- Push notifications
- Mobile apps (iOS & Android)
- Video highlights integration
- Advanced statistics and analytics
- Fantasy sports integration
- Multilingual UI

See [TODO.md](docs/TODO.md) for the complete roadmap.

## 🤝 Contributing

Contributions are welcome! Please read our contributing guidelines before submitting pull requests.

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🙏 Acknowledgments

- Sports data sources
- OpenAI for GPT API
- Google for Gemini API
- All contributors

## 📧 Contact

Project Link: [https://github.com/gitfcankaya/sportnewsandscores](https://github.com/gitfcankaya/sportnewsandscores)

---

Made with ❤️ by the Sport News & Scores Team 
