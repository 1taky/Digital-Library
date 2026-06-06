<script setup lang="ts">
import { Container } from '@/shared/ui/container';
import { Section } from '@/shared/ui/section';
import { Typography } from '@/shared/ui/typography';

import { useBooks } from './model/use-books';

const horrorGenre = 'Horror';
const dramaGenre = 'Romance';

const {
  books: horrorBooks,
  isLoading: isHorrorLoading,
  errorMessage: horrorErrorMessage,
} = useBooks(horrorGenre);

const {
  books: dramaBooks,
  isLoading: isDramaLoading,
  errorMessage: dramaErrorMessage,
} = useBooks(dramaGenre);
</script>

<template>
  <Section class="mt-20">
    <Container class="pt-24">
      <div
        class="mx-auto mb-16 flex max-w-4xl flex-col items-center px-4 text-center"
      >
        <Typography as="h1" weight="bold" size="lg" class="text-gray-900">
          Електронна бібліотека
        </Typography>

        <span class="mt-3 h-1 w-24 rounded-full bg-accent-dark-green"></span>

        <Typography
          weight="medium"
          size="md"
          align="center"
          class="mt-8 max-w-3xl text-gray-600"
        >
          У нас зібрані книги різних жанрів для навчання, відпочинку та
          саморозвитку. Обирайте цікаві добірки та переходьте до каталогу, щоб
          знайти саме те, що вам потрібно!
        </Typography>

        <div class="mt-10 flex gap-8 text-center">
          <div class="flex flex-col items-center">
            <div class="mt-10 flex gap-8 text-center">
              <div class="flex flex-col items-center">
                <Typography
                  as="span"
                  weight="bold"
                  size="lg"
                  class="text-accent-dark-green"
                  >100+</Typography
                >
                <Typography as="span" size="sm" class="mt-1 text-gray-500"
                  >Книг у каталозі</Typography
                >
              </div>
              <div class="w-px bg-gray-200"></div>
              <div class="flex flex-col items-center">
                <Typography
                  as="span"
                  weight="bold"
                  size="lg"
                  class="text-accent-dark-green"
                  >5+</Typography
                >
                <Typography as="span" size="sm" class="mt-1 text-gray-500"
                  >Жанрів</Typography
                >
              </div>
              <div class="w-px bg-gray-200"></div>
              <div class="flex flex-col items-center">
                <Typography
                  as="span"
                  weight="bold"
                  size="lg"
                  class="text-accent-dark-green"
                  >Безкоштовно</Typography
                >
                <Typography as="span" size="sm" class="mt-1 text-gray-500"
                  >Для всіх</Typography
                >
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="mb-16">
        <div class="mb-6 flex items-center justify-between gap-4">
          <Typography as="h2" weight="bold">
            {{ horrorGenre }}
          </Typography>

          <a
            href="/catalog"
            class="rounded-full px-5 py-3 text-sm font-semibold text-background transition"
            style="background-color: #0c6038"
            @mouseover="
              (e) =>
                ((e.currentTarget as HTMLAnchorElement).style.backgroundColor =
                  '#0a5230')
            "
            @mouseleave="
              (e) =>
                ((e.currentTarget as HTMLAnchorElement).style.backgroundColor =
                  '#0C6038')
            "
          >
            Перейти до каталогу
          </a>
        </div>

        <p v-if="isHorrorLoading" class="text-gray-500">Завантаження книг...</p>

        <p v-else-if="horrorErrorMessage" class="text-red-600">
          {{ horrorErrorMessage }}
        </p>

        <div v-else class="flex gap-6 overflow-x-auto pb-4">
          <div
            v-for="book in horrorBooks"
            :key="book.id"
            class="min-w-48 rounded-2xl border border-gray-200 bg-background p-4 shadow-sm transition"
          >
            <a :href="`/books/${book.id}`" class="w-full h-full">
              <img
                :src="book.coverUrl || ''"
                :alt="book.title"
                class="h-64 rounded-xl object-cover"
              />
              <h3
                class="mt-4 line-clamp-2 text-base font-semibold text-gray-900"
              >
                {{ book.title }}
              </h3></a
            >
          </div>
        </div>
      </div>

      <div class="mb-16">
        <div class="mb-6 flex items-center justify-between gap-4">
          <Typography as="h2" weight="bold">
            {{ dramaGenre }}
          </Typography>

          <a
            href="/catalog"
            class="rounded-full px-5 py-3 text-sm font-semibold text-white transition"
            style="background-color: #0c6038"
            @mouseover="
              (e) =>
                ((e.currentTarget as HTMLAnchorElement).style.backgroundColor =
                  '#0a5230')
            "
            @mouseleave="
              (e) =>
                ((e.currentTarget as HTMLAnchorElement).style.backgroundColor =
                  '#0C6038')
            "
          >
            Перейти до каталогу
          </a>
        </div>

        <p v-if="isDramaLoading" class="text-gray-500">Завантаження книг...</p>

        <p v-else-if="dramaErrorMessage" class="text-red-600">
          {{ dramaErrorMessage }}
        </p>

        <div v-else class="flex gap-6 overflow-x-auto pb-4">
          <div
            v-for="book in dramaBooks"
            :key="book.id"
            class="min-w-48 rounded-2xl border border-gray-200 bg-white p-4 shadow-sm transition"
          >
            <a :href="`/books/${book.id}`" class="w-full h-full">
              <img
                :src="book.coverUrl || ''"
                :alt="book.title"
                class="h-64 w-full rounded-xl object-cover"
              />
              <h3
                class="mt-4 line-clamp-2 text-base font-semibold text-gray-900"
              >
                {{ book.title }}
              </h3></a
            >
          </div>
        </div>
      </div>
    </Container>
  </Section>
</template>
