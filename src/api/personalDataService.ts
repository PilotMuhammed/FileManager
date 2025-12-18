import apiClient from "./axios";

export interface PersonalData {
    id: number;
    fullName: string;
    motherName: string;
    birthDate: string;
    retirementNumber: number;
    deliveredTo: string;
    ReceivedFrom: string;
}

export const getAllPersonalData = (page = 1, pageSize = 10) => {
    return apiClient.get(`/PersonalData?page=${page}&pageSize=${pageSize}`);
};

export const addPersonalData = (data: Omit<PersonalData, "id">) => {
    return apiClient.post("/PersonalData", data);
};

export const updatePersonalData = (id: number, data: Partial<PersonalData>) => {
    return apiClient.put(`/PersonalData/${id}`, data);
};

export const deletePersonalData = (id: number) => {
    return apiClient.delete(`/PersonalData/${id}`);
};
