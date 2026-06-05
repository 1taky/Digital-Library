import { fetchBooks, type Book } from '@/shared';
import { onMounted, ref } from 'vue';

export const useBooks = (genreName: string) => {
  const books = ref<Book[]>([]);

  const isLoading = ref(false);
  const errorMessage = ref('');

  const loadBooks = async () => {
    isLoading.value = true;
    errorMessage.value = '';

    try {
      const response = await fetchBooks({
        PageSize: 5,
        GenreName: genreName,
      });

      books.value = response.items;
    } catch (error: any) {
      errorMessage.value = error.message || 'Не вдалося завантажити книги';
      console.error(error);
    } finally {
      isLoading.value = false;
    }
  };

  onMounted(() => {
    loadBooks();
  });

  return {
    books,
    isLoading,
    errorMessage,
  };
};