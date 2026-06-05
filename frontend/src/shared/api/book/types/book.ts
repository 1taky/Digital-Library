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

// Оновлена модель книги, яку ми отримуємо з сервера (Response)
export type Book = {
  id: number;
  title: string;
  author: string;
  description: string;
  genreName: string;
  language: string;
  publicationYear: number;
  formats: BookFormat[]; // Тепер тут масив об'єктів
  coverUrl: string | null;
  downloadUrl: string | null;
  listenUrl: string | null;
  createdAt: string; // Сервер повертає ISO-рядок. Можна змінити на Date, якщо ти робиш new Date() при отриманні
};

// Оновлений Payload для створення/редагування книги (Request)
export type BookPayload = {
  title: string;
  author: string;
  description: string;
  genreName: string;
  language: string;
  publicationYear: number;
  formats: BookFormatPayload[]; // Відправляємо масив форматів без id та isAvailable
};
