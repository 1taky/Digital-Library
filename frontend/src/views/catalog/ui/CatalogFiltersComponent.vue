<script setup lang="ts">
import type { GenreType } from '@/shared/types/genre';
import { Typography } from '@/shared/ui/typography';

const props = defineProps<{
  filters: {
    search: string;
    genreName: string;
    formatType: string;
    sortBy: string;
  };
  genres: GenreType[];
}>();
</script>

<template>
  <div
    class="flex flex-col gap-5 p-4 border border-gray-200 rounded-md bg-gray-50"
  >
    <div class="flex flex-col gap-1">
      <Typography as="label" size="sm" weight="semibold">Пошук</Typography>
      <input
        v-model="filters.search"
        type="text"
        placeholder="Назва або автор..."
        class="border border-gray-300 p-2 rounded outline-none focus:border-emerald-600 bg-white"
      />
    </div>

    <div class="flex flex-col gap-1">
      <Typography as="label" size="sm" weight="semibold">Жанр</Typography>
      <select
        v-model="filters.genreName"
        class="border border-gray-300 p-2 rounded outline-none focus:border-emerald-600 bg-white cursor-pointer"
      >
        <option value="">Всі жанри</option>
        <option v-for="genre in genres" :key="genre.id" :value="genre.name">
          {{ genre.name }}
        </option>
      </select>
    </div>

    <div class="flex flex-col gap-1">
      <Typography as="label" size="sm" weight="semibold">Тип книги</Typography>
      <select
        v-model="filters.formatType"
        class="border border-gray-300 p-2 rounded outline-none focus:border-emerald-600 bg-white cursor-pointer"
      >
        <option value="">Всі формати</option>
        <option value="Paper">Паперова</option>
        <option value="Electronic">Електронна</option>
        <option value="Audio">Аудіо</option>
      </select>
    </div>

    <div class="flex flex-col gap-1">
      <Typography as="label" size="sm" weight="semibold">Сортування</Typography>
      <select
        v-model="filters.sortBy"
        class="border border-gray-300 p-2 rounded outline-none focus:border-emerald-600 bg-white cursor-pointer"
      >
        <option value="newest">Найновіші за роком</option>
        <option value="title">За алфавітом (Назва)</option>
      </select>
    </div>
  </div>
</template>
