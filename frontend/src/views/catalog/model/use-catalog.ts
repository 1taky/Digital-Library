// src/features/catalog/model/useCatalog.ts
import { ref, watch, onMounted } from 'vue';
import { fetchBooks, fetchGenres, type Book } from '@/shared';
import type { GenreType } from '@/shared/types/genre';

export const useCatalog = () => {
  const books = ref<Book[]>([]);
  const genres = ref<GenreType[]>([]);
  const isLoading = ref(false);
  const isFetchingMore = ref(false);
  const nextCursor = ref<number | null>(null);
  const hasMore = ref(false);

  // Стан фільтрів
  const filters = ref({
    search: '',
    genreName: '',
    formatType: '',
    sortBy: 'newest', // Сортування на фронтенді (або можна передавати на бек, якщо є такий параметр)
  });

  const loadGenres = async () => {
    try {
      genres.value = await fetchGenres();
    } catch (e) {
      console.error('Помилка завантаження жанрів', e);
    }
  };

  const loadBooks = async (reset = true) => {
    if (reset) {
      isLoading.value = true;
      nextCursor.value = null;
    } else {
      isFetchingMore.value = true;
    }

    try {
      const response = await fetchBooks({
        Search: filters.value.search || undefined,
        GenreName: filters.value.genreName || undefined,
        FormatType: filters.value.formatType || undefined,
        Cursor: nextCursor.value !== null ? nextCursor.value : undefined,
        PageSize: 12,
      });

      let fetchedBooks = response.items;

      // Локальне сортування, якщо бекенд не підтримує параметр SortBy
      if (filters.value.sortBy === 'newest') {
        fetchedBooks.sort((a, b) => b.publicationYear - a.publicationYear);
      } else if (filters.value.sortBy === 'title') {
        fetchedBooks.sort((a, b) => a.title.localeCompare(b.title));
      }

      if (reset) {
        books.value = fetchedBooks;
      } else {
        books.value.push(...fetchedBooks);
      }

      nextCursor.value = response.nextCursor;
      hasMore.value = response.hasMore;
    } catch (error) {
      console.error('Помилка завантаження каталогу', error);
    } finally {
      isLoading.value = false;
      isFetchingMore.value = false;
    }
  };

  const loadMore = () => {
    if (hasMore.value && !isFetchingMore.value) {
      loadBooks(false);
    }
  };

  // Таймер для debounce, щоб не спамити бекенд при кожному натисканні клавіші в пошуку
  let searchTimeout: ReturnType<typeof setTimeout>;

  watch(
    filters,
    () => {
      clearTimeout(searchTimeout);
      searchTimeout = setTimeout(() => {
        loadBooks(true);
      }, 400); // Запит піде через 400мс після того, як користувач перестав друкувати
    },
    { deep: true },
  );

  onMounted(() => {
    loadGenres();
    loadBooks(true);
  });

  return {
    books,
    genres,
    filters,
    isLoading,
    isFetchingMore,
    hasMore,
    loadMore,
  };
};
