import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7100/api"
});

export default api;

