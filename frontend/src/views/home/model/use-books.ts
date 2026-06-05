import { fetchBooks, type Book } from '@/shared';
import { onMounted, ref } from 'vue';

export const useBooks = () => {
  const books = ref<Book[]>([]);

  const isLoading = ref(false);
  const errorMessage = ref('');

  const loadBooks = async () => {
    isLoading.value = true;
    errorMessage.value = '';

    try {
      const response = await fetchBooks({
        PageSize: 5,
        // GenreName: genre,
      });

      books.value = response.items;
    } catch (error: any) {
      errorMessage.value = 'Не вдалося завантажити книги';
      console.error(error);
    } finally {
      isLoading.value = false;
    }
  };

  onMounted(() => {
    loadBooks();
  });
  return { books };
};
