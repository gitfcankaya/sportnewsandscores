import { useEffect, useState } from 'react';
import { newsService } from '../services/api';

function NewsList() {
  const [news, setNews] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedSport, setSelectedSport] = useState(null);

  useEffect(() => {
    const fetchNews = async () => {
      try {
        const response = await newsService.getAll(selectedSport);
        setNews(response.data);
      } catch (error) {
        console.error('Error fetching news:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchNews();
  }, [selectedSport]);

  if (loading) return <div className="loading">Loading news...</div>;

  return (
    <div className="news-list">
      <h2>Latest Sports News</h2>
      <div className="news-grid">
        {news.map((item) => (
          <div key={item.id} className="news-card">
            {item.imageUrl && (
              <img src={item.imageUrl} alt={item.title} className="news-image" />
            )}
            <div className="news-content">
              <h3>{item.title}</h3>
              <p className="news-excerpt">{item.content.substring(0, 150)}...</p>
              <div className="news-meta">
                <span className="news-sport">{item.sport?.name}</span>
                <span className="news-date">
                  {new Date(item.createdAt).toLocaleDateString()}
                </span>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default NewsList;
