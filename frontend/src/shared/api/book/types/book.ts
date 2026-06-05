export type BookType = 'Paper' | 'Electronic' | 'Audio' | string;

export type BookFormat = {
  id: number;
  formatType: BookType;
  isAvailable: boolean;
  pagesCount: number;
  durationMinutes: number;
};

export type BookFormatPayload = {
  formatType: BookType;
  pagesCount: number;
  durationMinutes: number;
};

export type Book = {
  id: number;
  title: string;
  author: string;
  description: string;
  genreName: string;
  language: string;
  publicationYear: number;
  formats: BookFormat[];
  coverUrl: string | null;
  downloadUrl: string | null;
  listenUrl: string | null;
  createdAt: string;
};

export type BookPayload = {
  title: string;
  author: string;
  description: string;
  genreName: string;
  language: string;
  publicationYear: number;
  formats: BookFormatPayload[];
};

export type UploadedFileResponse = {
  id: number;
  bookId: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  fileCategory: string;
  uploadedAt: string;
};
