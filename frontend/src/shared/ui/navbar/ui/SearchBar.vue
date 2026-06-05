<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';
import { useBooks } from '../model/use-books';
import { MagnifyingGlassIcon } from '@/shared/icons/magnifying-glass';

const { filtered, searchQuery, isLoading, errorMessage } = useBooks();
</script>

<template>
  <div class="w-full ml-6 mr-10">
    <div class="relative w-full">
      <div
        class="flex flex-row py-1 px-2 w-full bg-muted-background rounded-full gap-2"
      >
        <MagnifyingGlassIcon class="text-foreground" />
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Пошук"
          class="w-full focus:outline-none text-sm font-regular bg-transparent"
        />
      </div>

      <div
        class="absolute top-0 left-0 w-full flex bg-muted-background rounded-2xl -z-10"
        v-if="searchQuery.trim() !== ''"
      >
        <div class="pt-11 px-6 pb-4 w-full">
          <p v-if="isLoading">
            <Typography size="sm" weight="regular"> Шукаємо... </Typography>
          </p>
          <p v-if="errorMessage" class="text-red-500">{{ errorMessage }}</p>

          <ul
            class="flex flex-col gap-2"
            v-if="!isLoading && filtered.length > 0"
          >
            <li
              v-for="book in filtered.slice(0, 5)"
              :key="book.id"
              class="p-1 hover:bg-muted/20 active:bg-muted/50 transition-colors rounded-sm"
            >
              <a
                :href="`/books/${book.id}`"
                class="grid grid-cols-10 gap-4 items-center"
              >
                <img :src="`${book.coverUrl}`" class="h-12 grid-cols-1" />
                <span class="flex flex-row items-baseline gap-2">
                  <Typography size="sm" weight="medium">{{
                    book.title
                  }}</Typography>
                  <Typography size="xs" weight="regular" class="text-muted">
                    {{ book.publicationYear }}</Typography
                  >
                </span>
              </a>
            </li>
            <li
              class="p-1 py-2 hover:bg-muted/20 active:bg-muted/50 transition-colors rounded-sm"
            >
              <a href="/catalog"
                ><Typography size="sm" weight="medium">
                  > Переглянути більше в каталозі</Typography
                ></a
              >
            </li>
          </ul>

          <p v-if="!isLoading && filtered.length === 0 && !errorMessage">
            <Typography size="sm" weight="regular">
              За вашим запитом жанрів не знайдено.
            </Typography>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
