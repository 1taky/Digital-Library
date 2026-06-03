import { apiClient } from '../instance/instance';
import type { Book, BookPayload, UpdateBookPayload } from './types/book';

export const fetchBooks = async (): Promise<Book[]> => {
  const response = await apiClient.get<Book[]>('/books');

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
  payload: UpdateBookPayload,
): Promise<Book> => {
  const response = await apiClient.put<Book>(`/books/${id}`, payload);

  return response.data;
};

export const deleteBook = async (id: number): Promise<void> => {
  await apiClient.delete(`/books/${id}`);
};
