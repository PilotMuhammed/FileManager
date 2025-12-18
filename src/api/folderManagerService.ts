import apiClient from "./axios";

export const uploadUserFiles = (userId: number, formData: FormData) => {
    return apiClient.post(`/PersonalDocs/${userId}/upload`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
    });
};

export const getUserFiles = (userId: number) => {
    return apiClient.get(`/PersonalDocs/user/${userId}`);
};

export const deleteUserFile = (userId: number, fileId: number) => {
    return apiClient.delete(`/PersonalDocs/${userId}/files/${fileId}`);
};
