<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';
import type { Order } from '@/shared';

defineProps<{
  orders: Order[];
  isLoading: boolean;
}>();

const emit = defineEmits<{
  (e: 'approve', id: number): void;
  (e: 'reject', id: number): void;
  (e: 'borrow', id: number): void;
  (e: 'return', id: number): void;
}>();

// Словник для статусів (Колір та Текст)
const statusMap: Record<string, { label: string; color: string }> = {
  Requested: {
    label: 'Новий запит',
    color: 'bg-blue-100 text-blue-800 border-blue-200',
  },
  Approved: {
    label: 'Підтверджено',
    color: 'bg-yellow-100 text-yellow-800 border-yellow-200',
  },
  Borrowed: {
    label: 'Видано',
    color: 'bg-green-100 text-green-800 border-green-200',
  },
  Returned: {
    label: 'Повернуто',
    color: 'bg-gray-100 text-gray-800 border-gray-200',
  },
  Rejected: {
    label: 'Відхилено',
    color: 'bg-red-100 text-red-800 border-red-200',
  },
  Overdue: {
    label: 'Прострочено',
    color: 'bg-red-600 text-white border-red-700',
  },
};

const formatDate = (dateString: string | null) => {
  if (!dateString) return '-';
  return new Date(dateString).toLocaleDateString('uk-UA', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};
</script>

<template>
  <div
    class="overflow-x-auto bg-white border border-gray-200 rounded-md shadow-sm"
  >
    <div v-if="isLoading" class="text-center py-12">
      <Typography size="md" class="text-gray-500"
        >Завантаження замовлень...</Typography
      >
    </div>

    <div v-else-if="orders.length === 0" class="text-center py-12">
      <Typography size="md" class="text-gray-500"
        >Список замовлень порожній.</Typography
      >
    </div>

    <table v-else class="w-full text-left border-collapse">
      <thead class="bg-gray-50 border-b border-gray-200">
        <tr>
          <th class="p-3">
            <Typography size="sm" weight="semibold">ID</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold"
              >Користувач / Телефон</Typography
            >
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Книга</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Статус</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Дати</Typography>
          </th>
          <th class="p-3 text-center">
            <Typography size="sm" weight="semibold">Дії</Typography>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="order in orders"
          :key="order.id"
          class="border-b border-gray-100 hover:bg-gray-50/50"
        >
          <td class="p-3">
            <Typography size="sm" weight="medium">#{{ order.id }}</Typography>
          </td>

          <td class="p-3 flex flex-col gap-0.5">
            <Typography size="sm" weight="medium">{{
              order.userFullName
            }}</Typography>
            <Typography size="xs" class="text-gray-500">{{
              order.phoneNumber
            }}</Typography>
          </td>

          <td class="p-3">
            <Typography
              size="sm"
              weight="medium"
              class="line-clamp-2 max-w-50"
              >{{ order.bookTitle }}</Typography
            >
          </td>

          <td class="p-3">
            <span
              class="px-2.5 py-1 rounded-full text-xs font-semibold border"
              :class="
                statusMap[order.status]?.color || 'bg-gray-100 text-gray-800'
              "
            >
              {{ statusMap[order.status]?.label || order.status }}
            </span>
          </td>

          <td class="p-3 flex flex-col gap-0.5">
            <Typography size="xs" class="text-gray-600">
              Створено: {{ formatDate(order.createdAt) }}
            </Typography>
            <Typography
              v-if="order.dueDate && order.status !== 'Returned'"
              size="xs"
              class="text-amber-600 font-medium"
            >
              Повернути: {{ formatDate(order.dueDate) }}
            </Typography>
          </td>

          <td class="p-3">
            <div class="flex justify-center gap-2 flex-wrap max-w-40 mx-auto">
              <template v-if="order.status === 'Requested'">
                <button
                  @click="$emit('approve', order.id)"
                  class="px-3 py-1 bg-blue-600 text-white text-xs rounded hover:bg-blue-700 transition-colors"
                >
                  Підтвердити
                </button>
                <button
                  @click="$emit('reject', order.id)"
                  class="px-3 py-1 bg-red-100 text-red-700 border border-red-200 text-xs rounded hover:bg-red-200 transition-colors"
                >
                  Відхилити
                </button>
              </template>

              <template v-else-if="order.status === 'Approved'">
                <button
                  @click="$emit('borrow', order.id)"
                  class="px-3 py-1 bg-green-600 text-white text-xs rounded hover:bg-green-700 transition-colors w-full"
                >
                  Видати книгу
                </button>
              </template>

              <template
                v-else-if="
                  order.status === 'Borrowed' || order.status === 'Overdue'
                "
              >
                <button
                  @click="$emit('return', order.id)"
                  class="px-3 py-1 bg-gray-800 text-white text-xs rounded hover:bg-gray-900 transition-colors w-full"
                >
                  Оформити повернення
                </button>
              </template>

              <Typography v-else size="xs" class="text-gray-400">-</Typography>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
