import { ref } from 'vue';
import { apiClient } from '@/shared/api';

export const useBookMedia = () => {
  const isDownloading = ref(false);
  const isAudioOpening = ref(false);

  const downloadFileWithAuth = async (url: string, title: string) => {
    isDownloading.value = true;
    try {
      const response = await apiClient.get(url, { responseType: 'blob' });
      const blobUrl = window.URL.createObjectURL(new Blob([response.data]));

      const link = document.createElement('a');
      link.href = blobUrl;

      link.setAttribute('download', `${title}.pdf`);
      document.body.appendChild(link);
      link.click();
      link.remove();

      window.URL.revokeObjectURL(blobUrl);
    } catch (error) {
      console.error('Помилка завантаження файлу:', error);
      alert('Не вдалося завантажити файл. Можливо, ви не авторизовані.');
    } finally {
      isDownloading.value = false;
    }
  };

  const listenAudioWithAuth = async (url: string) => {
    isAudioOpening.value = true;
    try {
      const response = await apiClient.get(url, { responseType: 'blob' });
      const blobUrl = window.URL.createObjectURL(
        new Blob([response.data], { type: 'audio/mpeg' }),
      );
      window.open(blobUrl, '_blank');
    } catch (error) {
      console.error('Помилка відкриття аудіо:', error);
      alert('Не вдалося відкрити аудіофайл.');
    } finally {
      isAudioOpening.value = false;
    }
  };

  return {
    isDownloading,
    isAudioOpening,
    downloadFileWithAuth,
    listenAudioWithAuth,
  };
};
