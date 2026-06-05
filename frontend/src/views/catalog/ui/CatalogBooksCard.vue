<script setup lang="ts">
import type { Book } from '@/shared';
import { Typography } from '@/shared/ui/typography';

defineProps<{ book: Book }>();

const formatTypeMap: Record<string, string> = {
  Paper: 'Паперова',
  Electronic: 'Електронна',
  Audio: 'Аудіо',
};
</script>

<template>
  <div
    class="border border-gray-200 rounded-md overflow-hidden bg-white hover:shadow-md transition-shadow flex flex-col h-full"
  >
    <img
      :src="book.coverUrl || 'https://frecnuonna.s-ul.eu/rsz8SyUm'"
      alt="Обкладинка"
      class="w-full h-56 object-cover bg-gray-100"
    />
    <div class="p-4 flex flex-col grow">
      <Typography
        as="h3"
        weight="bold"
        size="md"
        class="leading-tight mb-1 line-clamp-2 text-gray-900"
      >
        {{ book.title }}
      </Typography>

      <Typography as="p" size="sm" class="text-gray-600 mb-3">
        {{ book.author }} • {{ book.publicationYear }}
      </Typography>

      <div class="mt-auto flex flex-wrap gap-1.5">
        <Typography
          as="span"
          size="xs"
          weight="medium"
          v-for="format in book.formats"
          :key="format.formatType"
          class="px-2 py-1 bg-gray-100 border border-gray-200 text-gray-700 rounded-sm"
        >
          {{ formatTypeMap[format.formatType] || format.formatType }}
        </Typography>
      </div>
    </div>
  </div>
</template>
