import { useEffect, useState } from 'react';
import { sportService } from '../services/api';

function SportsList() {
  const [sports, setSports] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSports = async () => {
      try {
        const response = await sportService.getAll();
        setSports(response.data);
      } catch (error) {
        console.error('Error fetching sports:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchSports();
  }, []);

  if (loading) return <div className="loading">Loading sports...</div>;

  return (
    <div className="sports-list">
      <h2>Sports</h2>
      <div className="sports-grid">
        {sports.map((sport) => (
          <div key={sport.id} className="sport-card">
            <span className="sport-icon">{sport.icon}</span>
            <h3>{sport.name}</h3>
            <p>{sport.nameTr}</p>
          </div>
        ))}
      </div>
    </div>
  );
}

export default SportsList;
