import axios from "axios";

const apiClient = axios.create({
    baseURL: "https://localhost:7058/api", 
    headers: {
        "Content-Type": "application/json",
    },
});

export default apiClient;
