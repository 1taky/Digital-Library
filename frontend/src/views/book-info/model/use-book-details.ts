import { type Book, fetchBookById } from '@/shared';
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';

export const useBookDetails = () => {
  const route = useRoute();
  const router = useRouter();

  const book = ref<Book | null>(null);
  const isLoading = ref(true);
  const errorMessage = ref('');

  const loadBook = async () => {
    const bookId = Number(route.params.id);

    if (isNaN(bookId)) {
      errorMessage.value = 'Некоректний ідентифікатор книги';
      isLoading.value = false;
      return;
    }

    try {
      book.value = await fetchBookById(bookId);
    } catch (error: any) {
      if (error.response?.status === 404) {
        errorMessage.value = 'Книгу не знайдено';
      } else {
        errorMessage.value = 'Помилка при завантаженні даних';
      }
      console.error(error);
    } finally {
      isLoading.value = false;
    }
  };

  const goBack = () => {
    router.back();
  };

  onMounted(() => {
    loadBook();
  });

  return {
    book,
    isLoading,
    errorMessage,
    goBack,
  };
};
