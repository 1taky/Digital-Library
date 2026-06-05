import { ref, type Ref } from 'vue';
import { createOrderRequest } from '@/shared';

export const useOrderBook = (bookId: Ref<number | undefined>) => {
  const isOpen = ref(false);
  const phoneNumber = ref('');
  const isSubmitting = ref(false);
  const errorMessage = ref('');
  const successMessage = ref('');

  const openModal = () => {
    isOpen.value = true;
    phoneNumber.value = '';
    errorMessage.value = '';
    successMessage.value = '';
  };

  const closeModal = () => {
    isOpen.value = false;
  };

  const submitOrder = async () => {
    if (!phoneNumber.value) {
      errorMessage.value = 'Будь ласка, введіть номер телефону.';
      return;
    }
    if (!bookId.value) return;

    isSubmitting.value = true;
    errorMessage.value = '';

    try {
      await createOrderRequest({
        bookId: bookId.value,
        phoneNumber: phoneNumber.value,
      });

      successMessage.value =
        "Запит на оренду успішно відправлено! Адміністратор зв'яжеться з вами.";

      setTimeout(() => {
        closeModal();
      }, 2000);
    } catch (error: any) {
      console.error('Помилка оренди:', error);
      errorMessage.value =
        error.response?.data?.message ||
        'Не вдалося відправити запит. Спробуйте пізніше.';
    } finally {
      isSubmitting.value = false;
    }
  };

  return {
    isOpen,
    phoneNumber,
    isSubmitting,
    errorMessage,
    successMessage,
    openModal,
    closeModal,
    submitOrder,
  };
};
