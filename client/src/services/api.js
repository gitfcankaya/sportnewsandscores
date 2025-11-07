import axios from 'axios';

const API_BASE_URL = 'http://localhost:5245/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const sportService = {
  getAll: () => api.get('/sports'),
  getById: (id) => api.get(`/sports/${id}`),
};

export const newsService = {
  getAll: (sportId, language) => {
    const params = new URLSearchParams();
    if (sportId) params.append('sportId', sportId);
    if (language) params.append('language', language);
    return api.get(`/news?${params.toString()}`);
  },
  getById: (id) => api.get(`/news/${id}`),
};

export const matchService = {
  getAll: (sportId, status) => {
    const params = new URLSearchParams();
    if (sportId) params.append('sportId', sportId);
    if (status) params.append('status', status);
    return api.get(`/matches?${params.toString()}`);
  },
  getLive: () => api.get('/matches/live'),
  getById: (id) => api.get(`/matches/${id}`),
};

export const playerService = {
  getAll: () => api.get('/players'),
  getById: (id) => api.get(`/players/${id}`),
};

export const commentService = {
  generate: (content, provider, newsId, matchId) =>
    api.post('/comments/generate', { content, provider, newsId, matchId }),
};

export default api;
