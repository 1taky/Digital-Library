import { fetchBooks, type Book } from '@/shared/api';
import { ref, onMounted, computed } from 'vue';

export const useBooks = () => {
  const books = ref<Book[]>([]);
  const searchQuery = ref('');
  const isLoading = ref(false);
  const errorMessage = ref('');

  const filtered = computed(() => {
    if (!searchQuery.value.trim()) return books.value;
    const query = searchQuery.value.toLowerCase();

    return books.value.filter((genre) =>
      genre.title.toLowerCase().includes(query),
    );
  });

  const loadGenres = async () => {
    isLoading.value = true;
    errorMessage.value = '';

    try {
      const response = await fetchBooks({ PageSize: 50 });

      books.value = response.items;
    } catch (error: any) {
      errorMessage.value = error.message;
      console.error(error);
    } finally {
      isLoading.value = false;
    }
  };

  onMounted(() => {
    loadGenres();
  });

  return {
    filtered,
    searchQuery,
    isLoading,
    errorMessage,
  };
};
