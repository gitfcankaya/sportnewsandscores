import { useState } from 'react';
import './App.css';
import SportsList from './components/SportsList';
import NewsList from './components/NewsList';
import LiveScores from './components/LiveScores';
import MatchesList from './components/MatchesList';

function App() {
  const [activeTab, setActiveTab] = useState('news');

  return (
    <div className="app">
      <header className="app-header">
        <h1>🏆 Sport News & Scores</h1>
        <p className="subtitle">Your Ultimate Sports Hub</p>
      </header>

      <nav className="main-nav">
        <button
          className={activeTab === 'news' ? 'active' : ''}
          onClick={() => setActiveTab('news')}
        >
          📰 News
        </button>
        <button
          className={activeTab === 'live' ? 'active' : ''}
          onClick={() => setActiveTab('live')}
        >
          🔴 Live Scores
        </button>
        <button
          className={activeTab === 'matches' ? 'active' : ''}
          onClick={() => setActiveTab('matches')}
        >
          ⚽ Matches
        </button>
        <button
          className={activeTab === 'sports' ? 'active' : ''}
          onClick={() => setActiveTab('sports')}
        >
          🏅 Sports
        </button>
      </nav>

      <main className="main-content">
        {activeTab === 'news' && <NewsList />}
        {activeTab === 'live' && <LiveScores />}
        {activeTab === 'matches' && <MatchesList />}
        {activeTab === 'sports' && <SportsList />}
      </main>

      <footer className="app-footer">
        <p>© 2025 Sport News & Scores - Powered by AI</p>
      </footer>
    </div>
  );
}

export default App;
