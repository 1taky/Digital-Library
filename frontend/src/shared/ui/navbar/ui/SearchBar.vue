<script setup lang="ts">
import { Typography } from '../../typography';
import { useGenres } from '../model/use-genres';

const { filteredGenres, genreSearchQuery, isLoading, errorMessage } =
  useGenres();
</script>
<template>
  <div>
    <div class="p-0.5 w-54 bg-muted-background rounded-full">
      <input
        v-model="genreSearchQuery"
        type="text"
        placeholder="Пошук"
        class="w-full mx-6 focus:outline-none text-sm font-normal"
      />
    </div>
    <div
      class="absolute -mt-7 min-w-54 flex bg-muted-background/50 rounded-2xl -z-10"
      v-if="genreSearchQuery.trim() !== ''"
    >
      <div class="pt-9 px-6 pb-4">
        <p v-if="isLoading">
          <Typography size="sm" weight="regular"
            >За вашим запитом жанрів не знайдено.
          </Typography>
        </p>
        <p v-if="errorMessage" style="color: red">{{ errorMessage }}</p>

        <ul
          class="flex flex-col gap-2"
          v-if="!isLoading && filteredGenres.length > 0"
        >
          <li v-for="genre in filteredGenres" :key="genre.id">
            <Typography size="sm" weight="medium">{{ genre.name }} </Typography>
          </li>
        </ul>

        <p v-if="!isLoading && filteredGenres.length === 0 && !errorMessage">
          <Typography size="sm" weight="regular"
            >За вашим запитом жанрів не знайдено.
          </Typography>
        </p>
      </div>
    </div>
  </div>
</template>
