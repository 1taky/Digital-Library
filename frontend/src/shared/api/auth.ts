import type {
  AuthResponseType,
  LoginPayload,
  RegisterPayload,
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
