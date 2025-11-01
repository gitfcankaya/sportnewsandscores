import { useEffect, useState } from 'react';
import { matchService } from '../services/api';

function LiveScores() {
  const [matches, setMatches] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchLiveMatches = async () => {
      try {
        const response = await matchService.getLive();
        setMatches(response.data);
      } catch (error) {
        console.error('Error fetching live matches:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchLiveMatches();
    const interval = setInterval(fetchLiveMatches, 30000); // Refresh every 30 seconds

    return () => clearInterval(interval);
  }, []);

  if (loading) return <div className="loading">Loading live scores...</div>;

  if (matches.length === 0) {
    return <div className="no-matches">No live matches at the moment</div>;
  }

  return (
    <div className="live-scores">
      <h2>🔴 Live Scores</h2>
      <div className="matches-list">
        {matches.map((match) => (
          <div key={match.id} className="match-card live">
            <div className="match-header">
              <span className="sport-name">{match.sport?.name}</span>
              <span className="match-status">{match.status}</span>
            </div>
            <div className="match-teams">
              <div className="team home">
                <span className="team-name">{match.homeTeam?.name}</span>
                <span className="team-score">{match.homeScore}</span>
              </div>
              <div className="vs">VS</div>
              <div className="team away">
                <span className="team-score">{match.awayScore}</span>
                <span className="team-name">{match.awayTeam?.name}</span>
              </div>
            </div>
            <div className="match-venue">{match.venue}</div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default LiveScores;
