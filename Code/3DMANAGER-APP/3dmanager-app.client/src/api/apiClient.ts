import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || 'https://localhost:443',
    headers: {
        'Content-Type': 'application/json',
    },
})

apiClient.interceptors.request.use(config => {
    const token = localStorage.getItem("token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

apiClient.interceptors.response.use(response => response, error => {
    if (error.response?.status === 401) {
        localStorage.removeItem("user");
        localStorage.removeItem("token");
        globalThis.location.href = "/login";
    }
    return Promise.reject(error);
});
export default apiClient