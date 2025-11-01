import { useEffect, useState } from 'react';
import { matchService } from '../services/api';

function MatchesList() {
  const [matches, setMatches] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState('all');

  useEffect(() => {
    const fetchMatches = async () => {
      try {
        const status = filter === 'all' ? null : filter;
        const response = await matchService.getAll(null, status);
        setMatches(response.data);
      } catch (error) {
        console.error('Error fetching matches:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchMatches();
  }, [filter]);

  if (loading) return <div className="loading">Loading matches...</div>;

  return (
    <div className="matches-section">
      <h2>Matches</h2>
      <div className="filter-buttons">
        <button
          className={filter === 'all' ? 'active' : ''}
          onClick={() => setFilter('all')}
        >
          All
        </button>
        <button
          className={filter === 'Scheduled' ? 'active' : ''}
          onClick={() => setFilter('Scheduled')}
        >
          Upcoming
        </button>
        <button
          className={filter === 'Live' ? 'active' : ''}
          onClick={() => setFilter('Live')}
        >
          Live
        </button>
        <button
          className={filter === 'Finished' ? 'active' : ''}
          onClick={() => setFilter('Finished')}
        >
          Finished
        </button>
      </div>
      <div className="matches-list">
        {matches.map((match) => (
          <div key={match.id} className={`match-card ${match.status.toLowerCase()}`}>
            <div className="match-header">
              <span className="sport-name">{match.sport?.name}</span>
              <span className="match-status">{match.status}</span>
            </div>
            <div className="match-teams">
              <div className="team home">
                <span className="team-name">{match.homeTeam?.name}</span>
                {match.homeScore !== null && (
                  <span className="team-score">{match.homeScore}</span>
                )}
              </div>
              <div className="vs">VS</div>
              <div className="team away">
                {match.awayScore !== null && (
                  <span className="team-score">{match.awayScore}</span>
                )}
                <span className="team-name">{match.awayTeam?.name}</span>
              </div>
            </div>
            <div className="match-info">
              <span className="match-date">
                {new Date(match.matchDate).toLocaleDateString()}
              </span>
              <span className="match-venue">{match.venue}</span>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default MatchesList;
