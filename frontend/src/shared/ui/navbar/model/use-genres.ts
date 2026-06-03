import { fetchGenres } from '@/shared/api';
import type { GenreType } from '@/shared/types/genre';
import { ref, onMounted, computed } from 'vue';

export const useGenres = () => {
  const genres = ref<GenreType[]>([]);
  const genreSearchQuery = ref('');
  const isLoading = ref(false);
  const errorMessage = ref('');

  // Логіка пошуку жанрів
  const filteredGenres = computed(() => {
    if (!genreSearchQuery.value.trim()) return genres.value;
    const query = genreSearchQuery.value.toLowerCase();

    return genres.value.filter((genre) =>
      genre.name.toLowerCase().includes(query),
    );
  });

  const loadGenres = async () => {
    isLoading.value = true;
    errorMessage.value = '';

    try {
      genres.value = await fetchGenres();
    } catch (error: any) {
      errorMessage.value = 'Не вдалося завантажити жанри';
      console.error(error);
    } finally {
      isLoading.value = false;
    }
  };

  // Завантажуємо жанри одразу при відкритті компонента
  onMounted(() => {
    loadGenres();
  });

  return {
    filteredGenres,
    genreSearchQuery,
    isLoading,
    errorMessage,
  };
};
