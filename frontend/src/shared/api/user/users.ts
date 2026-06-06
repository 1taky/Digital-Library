import { apiClient } from '../instance/instance';
import type { User, UpdateRolePayload } from './types/user';

export const fetchUsers = async (): Promise<User[]> => {
  const response = await apiClient.get<User[]>('/users');
  return response.data;
};

export const fetchUserById = async (id: number): Promise<User> => {
  const response = await apiClient.get<User>(`/users/${id}`);
  return response.data;
};

export const updateUserRole = async (
  id: number,
  payload: UpdateRolePayload,
): Promise<User> => {
  const response = await apiClient.patch<User>(`/users/${id}/role`, payload);
  return response.data;
};

export const blockUser = async (id: number): Promise<User> => {
  const response = await apiClient.patch<User>(`/users/${id}/block`);
  return response.data;
};

export const unblockUser = async (id: number): Promise<User> => {
  const response = await apiClient.patch<User>(`/users/${id}/unblock`);
  return response.data;
};
