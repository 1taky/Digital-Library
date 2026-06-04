export type BookType = 'Paper' | 'Electronic' | 'Audio' | string;

export type Book = {
  id: number;
  title: string;
  author: string;
  description: string;
  bookType: BookType;
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
  genreName: string;
  language: string;
  publicationYear: number;
  pagesCount: number;
  durationMinutes: number;
};
