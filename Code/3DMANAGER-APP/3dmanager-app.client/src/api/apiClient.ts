import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL ?? "/api",
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
    const rawStatus = error?.response?.status;
    const status = Number(rawStatus);

    if (error.response?.status === 401) {
        if (localStorage.getItem("token")) {
            localStorage.removeItem("user");
            localStorage.removeItem("token");
            globalThis.location.href = "/login";
        }
    }

    if (!status || status >= 500) {
        const errorId = crypto.randomUUID();
        sessionStorage.setItem("lastErrorId", errorId);
        globalThis.location.href = `/error?code=${errorId}`;
        return;
    }
    return Promise.reject(error);
});

export default apiClient