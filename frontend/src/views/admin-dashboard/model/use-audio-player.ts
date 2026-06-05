import { ref, onUnmounted } from 'vue';

export const useAudioPlayer = () => {
  const audioPlayerUrl = ref<string | null>(null);
  const isAudioLoading = ref(false);

  // Передаємо URL для завантаження та саму функцію fetchAudioBlobUrl як аргументи
  const loadAudioPlayer = async (
    listenUrl: string | undefined | null,
    fetchAudioBlobUrl: (url: string) => Promise<string | null>,
  ) => {
    if (!listenUrl) return;

    isAudioLoading.value = true;
    try {
      const url = await fetchAudioBlobUrl(listenUrl);
      if (url) {
        audioPlayerUrl.value = url;
      }
    } catch (error) {
      console.error('Помилка завантаження плеєра:', error);
    } finally {
      isAudioLoading.value = false;
    }
  };

  // Важливо: автоматично очищаємо пам'ять браузера при закритті компонента
  onUnmounted(() => {
    if (audioPlayerUrl.value) {
      window.URL.revokeObjectURL(audioPlayerUrl.value);
    }
  });

  return {
    audioPlayerUrl,
    isAudioLoading,
    loadAudioPlayer,
  };
};
