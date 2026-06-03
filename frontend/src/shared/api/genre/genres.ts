import type { GenreType } from '@/shared/types/genre';
import { apiClient } from '@/shared/api/';

export const fetchGenres = async (): Promise<GenreType[]> => {
  const response = await apiClient.get<GenreType[]>('/genres');
  return response.data;
};
