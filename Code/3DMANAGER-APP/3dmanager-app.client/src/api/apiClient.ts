import axios from 'axios'

const apiClient = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL ?? "/api",
    withCredentials: true,
    headers: {
        'Content-Type': 'application/json',
    },
})


apiClient.interceptors.response.use(response => response, error => {
    const rawStatus = error?.response?.status;
    const status = Number(rawStatus);
    const message = error?.response?.message;
    if (error.response?.status === 401) {
        if (localStorage.getItem("user")) {
            localStorage.removeItem("user");
            globalThis.location.href = "/login";
        }
    }

    if (!status || status >= 500) {
        const errorId = crypto.randomUUID();
        sessionStorage.setItem("lastErrorId", errorId);
        globalThis.location.href = `/error?code=${message}`;
        return;
    }
    return Promise.reject(error);
});

export default apiClient