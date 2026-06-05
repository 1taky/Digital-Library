import { apiClient } from '../instance/instance';
import type {
  Book,
  BookPayload,
  BookType,
  UploadedFileResponse,
} from './types/book';

export const fetchBooks = async (params?: BookType): Promise<Book[]> => {
  const response = await apiClient.get<Book[]>('/books', {
    params: {
      genreName: params || undefined,
    },
  });

  return response.data;
};

export const createBook = async (payload: BookPayload): Promise<Book> => {
  const response = await apiClient.post<Book>('/books', payload);

  return response.data;
};

export const fetchBookById = async (id: number): Promise<Book> => {
  const response = await apiClient.get<Book>(`/books/${id}`);

  return response.data;
};

export const updateBook = async (
  id: number,
  payload: BookPayload,
): Promise<Book> => {
  const response = await apiClient.put<Book>(`/books/${id}`, payload);

  return response.data;
};

export const deleteBook = async (id: number): Promise<void> => {
  await apiClient.delete(`/books/${id}`);
};

export const uploadBookCover = async (
  bookId: number,
  file: File,
): Promise<UploadedFileResponse> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await apiClient.post<UploadedFileResponse>(
    `/books/${bookId}/cover`,
    formData,
    {
      headers: {
        'Content-Type': 'multipart/form-data',
      },
    },
  );
  return response.data;
};

export const uploadBookFile = async (
  bookId: number,
  file: File,
): Promise<UploadedFileResponse> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await apiClient.post<UploadedFileResponse>(
    `/books/${bookId}/file`,
    formData,
    {
      headers: { 'Content-Type': 'multipart/form-data' },
    },
  );
  return response.data;
};

export const uploadBookAudio = async (
  bookId: number,
  file: File,
): Promise<UploadedFileResponse> => {
  const formData = new FormData();
  formData.append('file', file);

  const response = await apiClient.post<UploadedFileResponse>(
    `/books/${bookId}/audio`,
    formData,
    {
      headers: { 'Content-Type': 'multipart/form-data' },
    },
  );
  return response.data;
};
