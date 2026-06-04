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
import { ref, onMounted } from 'vue';

export const useAdminBooks = () => {
  const books = ref<Book[]>([]);
  const genres = ref<GenreType[]>([]);

  const isLoading = ref(false);
  const isModalOpen = ref(false);
  const isEditing = ref(false);
  const editingBookId = ref<number | null>(null);

  const initialFormState: BookPayload = {
    title: '',
    author: '',
    description: '',
    bookType: 'Paper',
    genreName: '',
    language: 'Українська',
    publicationYear: new Date().getFullYear(),
    pagesCount: 1,
    durationMinutes: 1,
  };

  const form = ref<BookPayload>({ ...initialFormState });

  const loadBooks = async () => {
    isLoading.value = true;
    try {
      books.value = await fetchBooks();
    } catch (error) {
      console.error('Помилка завантаження книг:', error);
    } finally {
      isLoading.value = false;
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Ви впевнені, що хочете видалити цю книгу? Дія незворотня.'))
      return;
    try {
      await deleteBook(id);
      await loadBooks();
    } catch (error) {
      console.error('Помилка видалення:', error);
      alert('Не вдалося видалити книгу');
    }
  };

  const openAddModal = () => {
    isEditing.value = false;
    editingBookId.value = null;
    form.value = { ...initialFormState };
    isModalOpen.value = true;
  };

  const openEditModal = (book: Book) => {
    isEditing.value = true;
    editingBookId.value = book.id;
    form.value = {
      title: book.title,
      author: book.author,
      description: book.description,
      bookType: book.bookType,
      genreName: book.genreName, // Використовуємо рядок
      language: book.language,
      publicationYear: book.publicationYear,
      pagesCount: book.pagesCount,
      durationMinutes: book.durationMinutes,
    };
    isModalOpen.value = true;
  };

  const closeModal = () => {
    isModalOpen.value = false;
  };

  const handleSubmit = async () => {
    try {
      if (isEditing.value && editingBookId.value) {
        await updateBook(editingBookId.value, form.value);
      } else {
        await createBook(form.value as BookPayload);
      }
      closeModal();
      await loadBooks();
    } catch (error) {
      console.error('Помилка збереження:', error);
      alert('Не вдалося зберегти книгу');
    }
  };

  const loadGenres = async () => {
    try {
      genres.value = await fetchGenres();
    } catch (error) {
      console.error('Помилка завантаження жанрів:', error);
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
    handleDelete,
    openAddModal,
    openEditModal,
    closeModal,
    handleSubmit,
  };
};
