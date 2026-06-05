import { onMounted, ref, type Ref } from 'vue';
import { createOrderRequest, fetchOrder } from '@/shared';

export const useOrderBook = (bookId: Ref<number | undefined>) => {
  const isOpen = ref(false);
  const phoneNumber = ref('');
  const isSubmitting = ref(false);
  const errorMessage = ref('');
  const successMessage = ref('');

  const hasOrder = ref(false);

  const openModal = () => {
    isOpen.value = true;
    phoneNumber.value = '';
    errorMessage.value = '';
    successMessage.value = '';
  };

  const closeModal = () => {
    isOpen.value = false;
  };

  const checkBookOrder = async () => {
    if (!bookId.value) return;

    try {
      const order = await fetchOrder(bookId.value);
      hasOrder.value = !!order;
    } catch (error: any) {
      if (error.response?.status === 404) {
        hasOrder.value = false;
      } else {
        console.error('Помилка перевірки замовлення:', error.message);
        hasOrder.value = false;
      }
    }
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

  onMounted(() => {
    checkBookOrder();
  });

  return {
    hasOrder,
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
