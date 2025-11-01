# Software Development Document (SDD)

## Sport News and Scores Platform

### Version: 1.0.0
### Date: November 1, 2025

## 1. Development Environment Setup

### 1.1 Prerequisites

#### Backend Development
- **.NET 9.0 SDK** - Download from https://dotnet.microsoft.com/download
- **Visual Studio 2022** or **Visual Studio Code** with C# extension
- **SQL Server** - LocalDB, Express, or Developer Edition
- **SQL Server Management Studio (SSMS)** - Optional but recommended

#### Frontend Development
- **Node.js 20.x LTS** - Download from https://nodejs.org/
- **npm 10.x** (comes with Node.js)
- **Visual Studio Code** with React extensions

#### Tools
- **Git** for version control
- **Postman** or similar for API testing
- **Web browser** with developer tools (Chrome/Firefox)

### 1.2 Project Setup

#### Clone Repository
```bash
git clone <repository-url>
cd sportnewsandscores
```

#### Backend Setup
```bash
# Navigate to solution directory
cd sportnewsandscores

# Restore NuGet packages
dotnet restore

# Build solution
dotnet build

# Update database (when migrations are added)
cd src/SportNewsAndScores.Api
dotnet ef database update
```

#### Frontend Setup
```bash
# Navigate to client directory
cd client

# Install dependencies
npm install

# Start development server
npm run dev
```

### 1.3 Configuration

#### Backend Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SportNewsAndScores;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here"
  },
  "Gemini": {
    "ApiKey": "your-gemini-api-key-here"
  }
}
```

#### Frontend Configuration (vite.config.js)
```javascript
export default {
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true
      }
    }
  }
}
```

## 2. Development Workflow

### 2.1 Git Workflow

#### Branch Strategy
- **main**: Production-ready code
- **develop**: Integration branch
- **feature/**: New features
- **bugfix/**: Bug fixes
- **hotfix/**: Critical production fixes

#### Commit Messages
Follow conventional commits:
```
feat: Add live score refresh functionality
fix: Correct news filtering by sport
docs: Update API documentation
style: Format code according to standards
refactor: Restructure service layer
test: Add unit tests for news service
```

### 2.2 Development Process

#### Creating New Feature
1. Create feature branch from develop
2. Implement feature
3. Write tests
4. Update documentation
5. Create pull request
6. Code review
7. Merge to develop

#### Code Review Checklist
- [ ] Code follows naming conventions
- [ ] No hardcoded values
- [ ] Proper error handling
- [ ] Documentation updated
- [ ] Tests added
- [ ] No console.logs or debug code
- [ ] Performance considered

### 2.3 Testing Strategy

#### Backend Testing
```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/SportNewsAndScores.Tests

# Run with coverage
dotnet test /p:CollectCoverage=true
```

#### Frontend Testing
```bash
# Run tests
npm test

# Run with coverage
npm test -- --coverage

# Run E2E tests
npm run test:e2e
```

## 3. Database Development

### 3.1 Entity Framework Migrations

#### Create Migration
```bash
cd src/SportNewsAndScores.Api
dotnet ef migrations add InitialCreate
```

#### Update Database
```bash
dotnet ef database update
```

#### Rollback Migration
```bash
dotnet ef database update PreviousMigrationName
```

#### Remove Last Migration
```bash
dotnet ef migrations remove
```

### 3.2 Seed Data Development

Edit `DbInitializer.cs`:
```csharp
public static void Initialize(ApplicationDbContext context)
{
    context.Database.EnsureCreated();
    
    // Check if data exists
    if (context.Sports.Any()) return;
    
    // Add seed data
    var sports = new Sport[] { /* ... */ };
    context.Sports.AddRange(sports);
    context.SaveChanges();
}
```

### 3.3 Database Schema Changes

When modifying entities:
1. Update entity class
2. Create new migration
3. Review generated migration
4. Test migration (up and down)
5. Update seed data if needed
6. Commit migration files

## 4. API Development

### 4.1 Creating New Controller

```csharp
[ApiController]
[Route("api/[controller]")]
public class NewResourceController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public NewResourceController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
    {
        var resources = await _context.Resources.ToListAsync();
        return Ok(resources);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Resource>> GetResource(int id)
    {
        var resource = await _context.Resources.FindAsync(id);
        if (resource == null) return NotFound();
        return Ok(resource);
    }
    
    [HttpPost]
    public async Task<ActionResult<Resource>> CreateResource(Resource resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetResource), new { id = resource.Id }, resource);
    }
}
```

### 4.2 Adding New Service

#### Define Interface (Core/Interfaces)
```csharp
public interface INewService
{
    Task<Result> DoSomethingAsync(Parameters params);
}
```

#### Implement Service (Infrastructure/Services)
```csharp
public class NewService : INewService
{
    public async Task<Result> DoSomethingAsync(Parameters params)
    {
        // Implementation
    }
}
```

#### Register in Program.cs
```csharp
builder.Services.AddScoped<INewService, NewService>();
```

### 4.3 API Testing

#### Manual Testing with Postman

**Get All Sports**
```
GET http://localhost:5000/api/sports
```

**Get News with Filters**
```
GET http://localhost:5000/api/news?sportId=1&language=en
```

**Generate AI Comment**
```
POST http://localhost:5000/api/comments/generate
Content-Type: application/json

{
  "content": "Amazing match between two great teams!",
  "provider": "openai",
  "matchId": 1
}
```

## 5. Frontend Development

### 5.1 Creating New Component

```jsx
import { useState, useEffect } from 'react';
import { someService } from '../services/api';

function NewComponent() {
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  
  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await someService.getAll();
        setData(response.data);
      } catch (error) {
        console.error('Error:', error);
      } finally {
        setLoading(false);
      }
    };
    
    fetchData();
  }, []);
  
  if (loading) return <div>Loading...</div>;
  
  return (
    <div className="new-component">
      {data.map(item => (
        <div key={item.id}>{item.name}</div>
      ))}
    </div>
  );
}

export default NewComponent;
```

### 5.2 Adding API Service

Edit `services/api.js`:
```javascript
export const newService = {
  getAll: () => api.get('/newresource'),
  getById: (id) => api.get(`/newresource/${id}`),
  create: (data) => api.post('/newresource', data),
  update: (id, data) => api.put(`/newresource/${id}`, data),
  delete: (id) => api.delete(`/newresource/${id}`)
};
```

### 5.3 Styling Guidelines

#### CSS Structure
```css
/* Component styles */
.component-name {
  /* Container styles */
}

.component-name__element {
  /* Element styles */
}

.component-name__element--modifier {
  /* Modified element styles */
}

/* Responsive */
@media (max-width: 768px) {
  .component-name {
    /* Mobile styles */
  }
}
```

#### CSS Variables (App.css)
```css
:root {
  --primary-color: #2563eb;
  --secondary-color: #10b981;
  --text-primary: #111827;
  /* Use in components */
}
```

## 6. Debugging

### 6.1 Backend Debugging

#### Visual Studio
1. Set breakpoints in code
2. Press F5 to start debugging
3. Application runs with debugger attached

#### VS Code
1. Use C# extension
2. Set breakpoints
3. F5 to start debugging

#### Common Issues
- **Connection string errors**: Check appsettings.json
- **Migration errors**: Delete database and recreate
- **Port conflicts**: Change port in launchSettings.json

### 6.2 Frontend Debugging

#### Browser DevTools
- **Console**: Check for errors and logs
- **Network**: Inspect API calls
- **Elements**: Inspect DOM and styles
- **React DevTools**: Inspect component state

#### VS Code
1. Install Debugger for Chrome extension
2. Add debug configuration
3. Set breakpoints in JSX
4. F5 to start debugging

#### Common Issues
- **CORS errors**: Check API CORS configuration
- **404 errors**: Verify API endpoint URLs
- **State not updating**: Check useEffect dependencies

## 7. Performance Optimization

### 7.1 Backend Optimization

#### Database Queries
```csharp
// BAD: N+1 query problem
var news = await _context.News.ToListAsync();
foreach (var item in news)
{
    var sport = await _context.Sports.FindAsync(item.SportId);
}

// GOOD: Eager loading
var news = await _context.News
    .Include(n => n.Sport)
    .ToListAsync();
```

#### Async/Await
```csharp
// GOOD: Proper async usage
public async Task<ActionResult> GetDataAsync()
{
    var data = await _service.FetchDataAsync();
    return Ok(data);
}
```

### 7.2 Frontend Optimization

#### Avoid Unnecessary Re-renders
```jsx
// Use useMemo for expensive computations
const expensiveValue = useMemo(() => {
  return computeExpensiveValue(data);
}, [data]);

// Use useCallback for functions passed to children
const handleClick = useCallback(() => {
  doSomething(param);
}, [param]);
```

#### Code Splitting
```jsx
// Lazy load components
const HeavyComponent = lazy(() => import('./HeavyComponent'));

function App() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <HeavyComponent />
    </Suspense>
  );
}
```

## 8. Build and Deployment

### 8.1 Development Build

#### Backend
```bash
dotnet build
dotnet run --project src/SportNewsAndScores.Api
```

#### Frontend
```bash
npm run dev
```

### 8.2 Production Build

#### Backend
```bash
dotnet publish -c Release -o ./publish
```

#### Frontend
```bash
npm run build
# Output in dist/ directory
```

### 8.3 Deployment Steps

#### Deploy to Azure (Example)

**Backend:**
1. Create Azure App Service
2. Configure connection string
3. Deploy from Visual Studio or CLI
4. Set environment variables

**Frontend:**
1. Build production bundle
2. Deploy to Azure Static Web Apps
3. Configure custom domain
4. Set up CDN

**Database:**
1. Create Azure SQL Database
2. Run migrations
3. Seed initial data
4. Configure firewall rules

### 8.4 Environment Configuration

**Development (appsettings.Development.json):**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  }
}
```

**Production (appsettings.Production.json):**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Use Azure Key Vault"
  }
}
```

## 9. Code Quality

### 9.1 Code Analysis

#### Backend
```bash
# Run code analysis
dotnet build /p:EnableNETAnalyzers=true

# Format code
dotnet format
```

#### Frontend
```bash
# Lint code
npm run lint

# Fix linting issues
npm run lint:fix

# Format code
npm run format
```

### 9.2 Code Review Guidelines

#### What to Look For
- Clean, readable code
- Proper error handling
- No code duplication
- Consistent naming
- Adequate comments
- No security vulnerabilities
- Performance considerations

#### Review Process
1. Check code diff
2. Verify tests pass
3. Run locally
4. Check for edge cases
5. Provide constructive feedback
6. Approve or request changes

## 10. Troubleshooting Guide

### 10.1 Common Backend Issues

#### Database Connection Failed
```
Solution:
1. Check SQL Server is running
2. Verify connection string
3. Check firewall settings
4. Test connection in SSMS
```

#### Entity Framework Migration Failed
```
Solution:
1. Delete migrations folder
2. Drop database
3. Create new initial migration
4. Update database
```

### 10.2 Common Frontend Issues

#### API Calls Failing
```
Solution:
1. Check API is running (http://localhost:5000)
2. Verify CORS configuration
3. Check network tab in DevTools
4. Verify endpoint URLs
```

#### Component Not Updating
```
Solution:
1. Check useEffect dependencies
2. Verify state is being set correctly
3. Check for shallow equality issues
4. Use React DevTools to inspect state
```

## 11. Best Practices

### 11.1 Backend Best Practices
- Use async/await for all I/O operations
- Implement proper error handling
- Use dependency injection
- Follow SOLID principles
- Keep controllers thin
- Use DTOs for API responses
- Implement logging
- Validate input

### 11.2 Frontend Best Practices
- Keep components small and focused
- Use functional components with hooks
- Implement proper error boundaries
- Handle loading states
- Avoid prop drilling
- Use semantic HTML
- Implement accessibility
- Optimize performance

### 11.3 Security Best Practices
- Never commit API keys
- Use environment variables
- Validate all inputs
- Use parameterized queries
- Implement HTTPS
- Set up CORS properly
- Keep dependencies updated
- Use secure password hashing

## 12. Maintenance

### 12.1 Regular Tasks
- Update dependencies monthly
- Review and optimize database queries
- Monitor error logs
- Back up database regularly
- Review and update documentation
- Security patches

### 12.2 Dependency Updates

#### Backend
```bash
# Check for outdated packages
dotnet list package --outdated

# Update specific package
dotnet add package PackageName --version x.x.x
```

#### Frontend
```bash
# Check for outdated packages
npm outdated

# Update packages
npm update

# Update specific package
npm install package@latest
```

## 13. Resources

### 13.1 Documentation
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core)
- [React Docs](https://react.dev)
- [Vite Docs](https://vitejs.dev)

### 13.2 Learning Resources
- Microsoft Learn
- React Tutorial
- Entity Framework Tutorial
- Clean Architecture guides

### 13.3 Community
- Stack Overflow
- GitHub Issues
- .NET Community
- React Community

## 14. Appendix

### 14.1 Useful Commands

#### Backend
```bash
dotnet build                    # Build solution
dotnet run                      # Run application
dotnet test                     # Run tests
dotnet ef migrations add Name   # Add migration
dotnet ef database update       # Update database
dotnet clean                    # Clean build outputs
```

#### Frontend
```bash
npm install                     # Install dependencies
npm run dev                     # Start dev server
npm run build                   # Build for production
npm run preview                 # Preview production build
npm run lint                    # Lint code
```

### 14.2 Project Structure Reference

```
sportnewsandscores/
├── src/
│   ├── SportNewsAndScores.Api/
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── SportNewsAndScores.Core/
│   │   ├── Entities/
│   │   └── Interfaces/
│   └── SportNewsAndScores.Infrastructure/
│       ├── Data/
│       └── Services/
├── client/
│   ├── src/
│   │   ├── components/
│   │   ├── services/
│   │   ├── App.jsx
│   │   └── main.jsx
│   └── package.json
├── docs/
│   ├── PRD.md
│   ├── SRS.md
│   ├── SDS.md
│   └── SDD.md
└── SportNewsAndScores.sln
```
