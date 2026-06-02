import type {
  AuthResponseType,
  LoginPayload,
  RegisterPayload,
  UserType,
} from '@/shared/types';
import { apiClient } from '@/shared/api/';

export const loginUser = async (
  payload: LoginPayload,
): Promise<AuthResponseType> => {
  const response = await apiClient.post<AuthResponseType>(
    '/auth/login',
    payload,
  );
  return response.data;
};

export const registerUser = async (
  payload: RegisterPayload,
): Promise<AuthResponseType> => {
  const response = await apiClient.post<AuthResponseType>(
    '/auth/register',
    payload,
  );
  return response.data;
};

export const fetchMe = async (): Promise<UserType> => {
  const response = await apiClient.get<UserType>('/auth/me');
  return response.data;
};
