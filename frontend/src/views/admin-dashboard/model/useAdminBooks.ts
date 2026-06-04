import { createBook, deleteBook, fetchBooks, updateBook } from '@/shared';
import type {
  Book,
  BookPayload,
  UpdateBookPayload,
} from '@/shared/api/book/types/book';
import { ref, onMounted } from 'vue';

export const useAdminBooks = () => {
  const books = ref<Book[]>([]);
  const isLoading = ref(false);
  const isModalOpen = ref(false);
  const isEditing = ref(false);
  const editingBookId = ref<number | null>(null);

  const initialFormState: UpdateBookPayload = {
    title: '',
    author: '',
    description: '',
    bookType: 'Paper',
    genreId: 1,
    language: 'Українська',
    publicationYear: new Date().getFullYear(),
    pagesCount: 0,
    durationMinutes: 0,
    isAvailable: true,
  };

  const form = ref<UpdateBookPayload>({ ...initialFormState });

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
      genreId: book.genreId,
      language: book.language,
      publicationYear: book.publicationYear,
      pagesCount: book.pagesCount,
      durationMinutes: book.durationMinutes,
      isAvailable: book.isAvailable,
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
        const { isAvailable, ...createData } = form.value;
        await createBook(createData as BookPayload);
      }
      closeModal();
      await loadBooks();
    } catch (error) {
      console.error('Помилка збереження:', error);
      alert('Не вдалося зберегти книгу');
    }
  };

  onMounted(() => {
    loadBooks();
  });

  return {
    books,
    isLoading,
    isModalOpen,
    isEditing,
    form,
    loadBooks,
    handleDelete,
    openAddModal,
    openEditModal,
    closeModal,
    handleSubmit,
  };
};
