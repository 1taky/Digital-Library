<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';
import { useCatalog } from './model/use-catalog';
import CatalogFiltersComponent from './ui/CatalogFiltersComponent.vue';
import CatalogBooksCard from './ui/CatalogBooksCard.vue';

const { books, genres, filters, isLoading, isFetchingMore, hasMore, loadMore } =
  useCatalog();
</script>

<template>
  <div class="max-w-7xl mx-auto px-4 py-8">
    <Typography
      as="h1"
      size="lg"
      weight="bold"
      class="text-3xl mb-8 text-gray-900"
    >
      Каталог книг
    </Typography>

    <div class="flex flex-col md:flex-row gap-6">
      <aside class="w-full md:w-1/4 shrink-0">
        <CatalogFiltersComponent :filters="filters" :genres="genres" />
      </aside>

      <main class="w-full md:w-3/4">
        <div v-if="isLoading" class="text-center py-20">
          <Typography size="md" class="text-gray-500">
            Завантаження каталогу...
          </Typography>
        </div>

        <div
          v-else-if="books.length === 0"
          class="text-center py-20 border border-gray-200 rounded bg-gray-50"
        >
          <Typography size="md" class="text-gray-500">
            За вашим запитом нічого не знайдено.
          </Typography>
        </div>

        <template v-else>
          <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-5">
            <RouterLink
              v-for="book in books"
              :key="book.id"
              :to="`/books/${book.id}`"
              class="block"
            >
              <CatalogBooksCard :book="book" />
            </RouterLink>
          </div>

          <div v-if="hasMore" class="mt-8 flex justify-center">
            <button
              @click="loadMore"
              :disabled="isFetchingMore"
              class="px-6 py-2 bg-white border border-gray-300 rounded-full text-gray-700 hover:bg-gray-50 transition-colors disabled:opacity-50 cursor-pointer"
            >
              <Typography as="span" size="sm" weight="medium">
                {{ isFetchingMore ? 'Завантаження...' : 'Показати ще' }}
              </Typography>
            </button>
          </div>
        </template>
      </main>
    </div>
  </div>
</template>
