import { ref, onMounted } from 'vue';
import {
  type Book,
  type BookPayload,
  fetchBooks,
  deleteBook,
  updateBook,
  createBook,
  fetchGenres,
} from '@/shared';
import type { GenreType } from '@/shared/types/genre';

export const useAdminBooks = () => {
  const books = ref<Book[]>([]);
  const genres = ref<GenreType[]>([]);
  const isLoading = ref(false);
  const errorMessage = ref('');

  const isModalOpen = ref(false);
  const isEditing = ref(false);
  const editingBookId = ref<number | null>(null);

  const getInitialFormState = (): BookPayload => ({
    title: '',
    author: '',
    description: '',
    genreName: '',
    language: 'Українська',
    publicationYear: new Date().getFullYear(),
    formats: [{ formatType: 'Paper', pagesCount: 1, durationMinutes: 0 }],
  });

  const form = ref<BookPayload>(getInitialFormState());

  const loadBooks = async () => {
    isLoading.value = true;
    errorMessage.value = '';
    try {
      books.value = await fetchBooks();
    } catch (error) {
      console.error('Помилка завантаження книг:', error);
      errorMessage.value = 'Не вдалося завантажити список книг.';
    } finally {
      isLoading.value = false;
    }
  };

  const loadGenres = async () => {
    try {
      genres.value = await fetchGenres();
    } catch (error) {
      console.error('Помилка завантаження жанрів:', error);
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Ви впевнені, що хочете видалити цю книгу? Дія незворотня.'))
      return;

    try {
      await deleteBook(id);
      await loadBooks();
    } catch (error: any) {
      console.error('Помилка видалення:', error);
      errorMessage.value =
        error.response?.data?.message || 'Помилка при видаленні книги';
    }
  };

  const openAddModal = () => {
    isEditing.value = false;
    editingBookId.value = null;
    form.value = getInitialFormState();
    errorMessage.value = '';
    isModalOpen.value = true;
  };

  const openEditModal = (book: Book) => {
    isEditing.value = true;
    editingBookId.value = book.id;
    errorMessage.value = '';

    form.value = {
      title: book.title,
      author: book.author,
      description: book.description,
      genreName: book.genreName,
      language: book.language,
      publicationYear: book.publicationYear,
      formats:
        book.formats && book.formats.length > 0
          ? book.formats.map((f) => ({
              formatType: f.formatType,
              pagesCount: f.pagesCount || 1,
              durationMinutes: f.durationMinutes || 0,
            }))
          : [{ formatType: 'Paper', pagesCount: 1, durationMinutes: 0 }],
    };

    isModalOpen.value = true;
  };

  const closeModal = () => {
    isModalOpen.value = false;
    errorMessage.value = '';
  };

  const handleSubmit = async () => {
    try {
      if (isEditing.value && editingBookId.value) {
        await updateBook(editingBookId.value, form.value);
      } else {
        await createBook(form.value);
      }
      closeModal();
      await loadBooks();
    } catch (error: any) {
      console.error('Помилка збереження:', error);
      errorMessage.value =
        error.response?.data?.message || 'Виникла помилка при збереженні форми';
    }
  };

  onMounted(() => {
    loadBooks();
    loadGenres();
  });

  return {
    books,
    genres,
    isLoading,
    isModalOpen,
    isEditing,
    form,
    errorMessage,
    loadBooks,
    handleDelete,
    openAddModal,
    openEditModal,
    closeModal,
    handleSubmit,
  };
};
