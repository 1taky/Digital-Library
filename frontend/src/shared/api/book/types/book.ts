export type BookType = 'Paper' | 'Electronic' | 'Audio' | string;

export type Book = {
  id: number;
  title: string;
  author: string;
  description: string;
  bookType: BookType;
  genreId: number;
  genreName: string;
  language: string;
  publicationYear: number;
  pagesCount: number;
  durationMinutes: number;
  isAvailable: boolean;
  createdAt: Date;
};

export type BookPayload = {
  title: string;
  author: string;
  description: string;
  bookType: BookType;
  genreId: number;
  language: string;
  publicationYear: number;
  pagesCount: number;
  durationMinutes: number;
};

export type UpdateBookPayload = {
  title: string;
  author: string;
  description: string;
  bookType: BookType;
  genreId: number;
  language: string;
  publicationYear: number;
  pagesCount: number;
  durationMinutes: number;
  isAvailable: boolean; // Це поле додалося для PUT-запиту
};
