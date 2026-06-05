import { ref, onMounted } from 'vue';
import {
  fetchAllOrders,
  fetchOverdueOrders,
  approveOrder,
  borrowOrder,
  returnOrder,
  rejectOrder,
  type Order,
} from '@/shared';

export const useOrders = () => {
  const orders = ref<Order[]>([]);
  const isLoading = ref(false);
  const errorMessage = ref('');
  const currentTab = ref<'all' | 'overdue'>('all');

  const loadOrders = async () => {
    isLoading.value = true;
    errorMessage.value = '';
    try {
      if (currentTab.value === 'all') {
        orders.value = await fetchAllOrders();
      } else {
        orders.value = await fetchOverdueOrders();
      }

      // Сортуємо: новіші зверху
      orders.value.sort(
        (a, b) =>
          new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime(),
      );
    } catch (error: any) {
      console.error('Помилка завантаження замовлень:', error);
      errorMessage.value = 'Не вдалося завантажити список замовлень.';
    } finally {
      isLoading.value = false;
    }
  };

  const setTab = (tab: 'all' | 'overdue') => {
    currentTab.value = tab;
    loadOrders();
  };

  // Універсальна функція для оновлення локального стану
  const updateLocalOrder = (updatedOrder: Order) => {
    const index = orders.value.findIndex((o) => o.id === updatedOrder.id);
    if (index !== -1) {
      orders.value[index] = updatedOrder;
    }
  };

  const handleApprove = async (id: number) => {
    try {
      const updated = await approveOrder(id);
      updateLocalOrder(updated);
    } catch (e) {
      alert('Помилка при підтвердженні');
    }
  };

  const handleReject = async (id: number) => {
    if (!confirm('Ви впевнені, що хочете відхилити цей запит?')) return;
    try {
      const updated = await rejectOrder(id);
      updateLocalOrder(updated);
    } catch (e) {
      alert('Помилка при відхиленні');
    }
  };

  const handleBorrow = async (id: number) => {
    try {
      const updated = await borrowOrder(id);
      updateLocalOrder(updated);
    } catch (e) {
      alert('Помилка при видачі книги');
    }
  };

  const handleReturn = async (id: number) => {
    try {
      const updated = await returnOrder(id);
      updateLocalOrder(updated);
    } catch (e) {
      alert('Помилка при поверненні книги');
    }
  };

  onMounted(() => {
    loadOrders();
  });

  return {
    orders,
    isLoading,
    errorMessage,
    currentTab,
    setTab,
    handleApprove,
    handleReject,
    handleBorrow,
    handleReturn,
  };
};
