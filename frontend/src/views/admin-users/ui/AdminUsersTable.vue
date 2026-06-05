<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';
import type { User } from '@/shared';

defineProps<{
  users: User[];
  isLoading: boolean;
}>();

const emit = defineEmits<{
  (e: 'change-role', userId: number, newRole: string): void;
  (e: 'toggle-block', user: User): void;
}>();

const formatDate = (dateString: string) => {
  return new Date(dateString).toLocaleDateString('uk-UA', {
    day: '2-digit',
    month: '2-digit',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
};

// Доступні ролі в системі (можеш розширити за потреби)
const availableRoles = ['User', 'Manager', 'Admin'];
</script>

<template>
  <div
    class="overflow-x-auto bg-white border border-gray-200 rounded-md shadow-sm"
  >
    <div v-if="isLoading" class="text-center py-12">
      <Typography size="md" class="text-gray-500"
        >Завантаження користувачів...</Typography
      >
    </div>

    <div v-else-if="users.length === 0" class="text-center py-12">
      <Typography size="md" class="text-gray-500"
        >Список користувачів порожній.</Typography
      >
    </div>

    <table v-else class="w-full text-left border-collapse">
      <thead class="bg-gray-50 border-b border-gray-200">
        <tr>
          <th class="p-3">
            <Typography size="sm" weight="semibold">ID</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Користувач</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Статус</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Роль</Typography>
          </th>
          <th class="p-3">
            <Typography size="sm" weight="semibold">Реєстрація</Typography>
          </th>
          <th class="p-3 text-center">
            <Typography size="sm" weight="semibold">Дії</Typography>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="user in users"
          :key="user.id"
          class="border-b border-gray-100 hover:bg-gray-50/50"
          :class="{ 'opacity-60 bg-gray-50': !user.isActive }"
        >
          <td class="p-3">
            <Typography size="sm" weight="medium">#{{ user.id }}</Typography>
          </td>

          <td class="p-3 flex flex-col gap-0.5">
            <Typography size="sm" weight="medium">{{
              user.fullName
            }}</Typography>
            <Typography size="xs" class="text-gray-500">{{
              user.email
            }}</Typography>
          </td>

          <td class="p-3">
            <span
              class="px-2.5 py-1 rounded-full text-xs font-semibold border"
              :class="
                user.isActive
                  ? 'bg-green-100 text-green-800 border-green-200'
                  : 'bg-red-100 text-red-800 border-red-200'
              "
            >
              {{ user.isActive ? 'Активний' : 'Заблокований' }}
            </span>
          </td>

          <td class="p-3">
            <select
              :value="user.role"
              @change="
                emit(
                  'change-role',
                  user.id,
                  ($event.target as HTMLSelectElement).value,
                )
              "
              :disabled="!user.isActive"
              class="border border-gray-300 p-1.5 rounded text-sm outline-none focus:border-emerald-600 bg-white cursor-pointer disabled:cursor-not-allowed"
            >
              <option v-for="role in availableRoles" :key="role" :value="role">
                {{ role }}
              </option>
            </select>
          </td>

          <td class="p-3">
            <Typography size="xs" class="text-gray-600">
              {{ formatDate(user.createdAt) }}
            </Typography>
          </td>

          <td class="p-3">
            <div class="flex justify-center">
              <button
                @click="emit('toggle-block', user)"
                class="px-4 py-1.5 text-xs font-medium rounded transition-colors w-28"
                :class="
                  user.isActive
                    ? 'bg-red-50 text-red-700 border border-red-200 hover:bg-red-100'
                    : 'bg-emerald-50 text-emerald-700 border border-emerald-200 hover:bg-emerald-100'
                "
              >
                {{ user.isActive ? 'Заблокувати' : 'Розблокувати' }}
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
