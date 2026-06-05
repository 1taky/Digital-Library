<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';
import type { Book } from '@/shared';

defineProps<{
  books: Book[];
  isLoading: boolean;
}>();

defineEmits<{
  (e: 'edit', book: Book): void;
  (e: 'delete', id: number): void;
  (e: 'manage-files', book: Book): void;
}>();

const formatTypeMap: Record<string, string> = {
  Paper: 'Паперова',
  Electronic: 'Електронна',
  Audio: 'Аудіо',
};
</script>

<template>
  <div
    v-if="isLoading"
    class="text-center text-gray-500 py-10 border border-gray-200"
  >
    <Typography size="sm" weight="medium"> Завантаження даних... </Typography>
  </div>

  <div v-else class="overflow-x-auto border border-muted-background">
    <table class="w-full text-left border-collapse">
      <thead class="bg-gray-100 border-b border-gray-300">
        <tr>
          <th class="p-3 font-semibold border-r border-gray-200 w-12">ID</th>
          <th class="p-3 font-semibold border-r border-gray-200">Назва</th>
          <th class="p-3 font-semibold border-r border-gray-200">Автор</th>
          <th class="p-3 font-semibold border-r border-gray-200">Рік</th>
          <th class="p-3 font-semibold border-r border-gray-200 min-w-35">
            Типи
          </th>
          <th class="p-3 font-semibold border-r border-gray-200">Жанр</th>
          <th class="p-3 font-semibold border-r border-gray-200">Мова</th>
          <th class="p-3 font-semibold border-r border-gray-200">Створено</th>
          <th class="p-3 font-semibold text-center">Дії</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="book in books"
          :key="book.id"
          class="border-b border-gray-200 hover:bg-gray-50"
        >
          <td class="p-3 border-r border-gray-200">{{ book.id }}</td>
          <td class="p-3 border-r border-gray-200 font-medium">
            {{ book.title }}
          </td>
          <td class="p-3 border-r border-gray-200">{{ book.author }}</td>
          <td class="p-3 border-r border-gray-200">
            {{ book.publicationYear }}
          </td>
          <td class="p-3 border-r border-gray-200">
            <span
              v-if="!book.formats || book.formats.length === 0"
              class="text-gray-400"
              >-</span
            >

            <div v-else class="flex flex-wrap gap-1.5">
              <span
                v-for="format in book.formats"
                :key="format.formatType"
                class="text-xs bg-gray-200 px-2 py-1 rounded-sm border border-gray-300 whitespace-nowrap"
              >
                {{ formatTypeMap[format.formatType] || format.formatType }}
              </span>
            </div>
          </td>
          <td class="p-3 border-r border-gray-200">{{ book.genreName }}</td>
          <td class="p-3 border-r border-gray-200">{{ book.language }}</td>
          <td class="p-3 border-r border-gray-200">
            {{ new Date(book.createdAt).toLocaleString() }}
          </td>

          <td class="py-3 px-2 flex justify-center gap-2">
            <button
              @click="$emit('edit', book)"
              class="bg-blue-100 text-blue-700 hover:bg-blue-200 px-3 py-1.5 text-sm rounded-full border border-blue-300 transition-colors cursor-pointer"
            >
              Редаг.
            </button>
            <button
              @click="$emit('manage-files', book)"
              class="bg-purple-100 text-purple-700 hover:bg-purple-200 px-3 py-1.5 text-sm rounded-full border border-purple-300 transition-colors cursor-pointer"
              title="Керування файлами"
            >
              Файли
            </button>
            <button
              @click="$emit('delete', book.id)"
              class="bg-red-100 text-red-700 hover:bg-red-200 px-3 py-1.5 text-sm rounded-full border border-red-300 transition-colors cursor-pointer"
            >
              Видалити
            </button>
          </td>
        </tr>
        <tr v-if="books.length === 0">
          <td colspan="7" class="p-4 text-center text-gray-500">
            Список книг порожній.
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
