import {
  uploadBookCover,
  uploadBookFile,
  uploadBookAudio,
  apiClient,
} from '@/shared';
import { ref } from 'vue';

export const useBookFileManager = () => {
  const isUploadingCover = ref(false);
  const isUploadingFile = ref(false);
  const isUploadingAudio = ref(false);
  const message = ref({ text: '', type: '' });

  const showMessage = (text: string, type: 'success' | 'error') => {
    message.value = { text, type };
    setTimeout(() => {
      message.value.text = '';
    }, 3000);
  };

  const handleUploadCover = async (bookId: number, event: Event) => {
    const target = event.target as HTMLInputElement;
    if (!target.files?.length) return;

    isUploadingCover.value = true;
    try {
      await uploadBookCover(bookId, target.files[0]);
      showMessage('Обкладинка успішно завантажена!', 'success');
      target.value = '';

      return true;
    } catch (error: any) {
      console.error(error);
      showMessage(error.message, 'error');

      return false;
    } finally {
      isUploadingCover.value = false;
    }
  };

  const handleUploadFile = async (bookId: number, event: Event) => {
    const target = event.target as HTMLInputElement;
    if (!target.files?.length) return;

    isUploadingFile.value = true;
    try {
      await uploadBookFile(bookId, target.files[0]);
      showMessage('Файл книги успішно завантажено!', 'success');
      target.value = '';

      return true;
    } catch (error: any) {
      console.error(error);
      showMessage(error.message, 'error');

      return false;
    } finally {
      isUploadingFile.value = false;
    }
  };

  const handleUploadAudio = async (bookId: number, event: Event) => {
    const target = event.target as HTMLInputElement;
    if (!target.files?.length) return;

    isUploadingAudio.value = true;
    try {
      await uploadBookAudio(bookId, target.files[0]);
      showMessage('Аудіофайл успішно завантажено!', 'success');
      target.value = '';

      return true;
    } catch (error: any) {
      console.error(error);
      showMessage(error.message, 'error');

      return false;
    } finally {
      isUploadingAudio.value = false;
    }
  };

  const downloadFileWithAuth = async (url: string, fileName: string) => {
    try {
      const response = await apiClient.get(url, { responseType: 'blob' });

      const blobUrl = window.URL.createObjectURL(new Blob([response.data]));

      const link = document.createElement('a');
      link.href = blobUrl;
      link.setAttribute('download', fileName);
      document.body.appendChild(link);
      link.click();
      link.remove();

      window.URL.revokeObjectURL(blobUrl);
    } catch (error) {
      console.error('Помилка завантаження файлу:', error);
      showMessage(
        'Не вдалося завантажити файл. Перевірте авторизацію.',
        'error',
      );
    }
  };

  const fetchAudioBlobUrl = async (url: string): Promise<string | null> => {
    try {
      const response = await apiClient.get(url, { responseType: 'blob' });
      return window.URL.createObjectURL(
        new Blob([response.data], { type: 'audio/mpeg' }),
      );
    } catch (error) {
      console.error('Помилка завантаження аудіо для плеєра:', error);
      showMessage('Не вдалося завантажити аудіофайл.', 'error');
      return null;
    }
  };

  return {
    isUploadingCover,
    isUploadingFile,
    isUploadingAudio,
    message,
    handleUploadCover,
    handleUploadFile,
    handleUploadAudio,
    downloadFileWithAuth,
    fetchAudioBlobUrl,
  };
};
