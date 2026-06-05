<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { Container } from '@/shared/ui/container';
import { Typography } from '@/shared/ui/typography';
import { useAdminBooks } from '../model/use-admin-books.ts';
import type { Book } from '@/shared';
import BooksTable from './BooksTable.vue';
import BookFormModal from './BookFormModal.vue';
import BookFileManagerModal from './BookFileManagerModal.vue';

const {
  books,
  genres,
  isLoading,
  isModalOpen,
  isEditing,
  form,
  errorMessage,

  hasMore,
  isFetchingMore,
  loadMore,

  loadBooks,
  handleDelete,
  openAddModal,
  openEditModal,
  closeModal,
  handleSubmit,
} = useAdminBooks();

const fileManagerBookId = ref<number | null>(null);

const openFileManager = (book: Book) => {
  fileManagerBookId.value = book.id;
};

const closeFileManager = () => {
  fileManagerBookId.value = null;
};
</script>

<template>
  <Section class="mt-20">
    <Container>
      <div class="max-w-6xl mx-auto p-4 md:p-6 font-sans text-foreground">
        <header class="flex justify-between items-center mb-6">
          <Typography as="h1" weight="bold" size="lg">
            Панель адміністратора
          </Typography>
          <button
            @click="openAddModal"
            class="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-full transition-colors cursor-pointer"
          >
            + Додати книгу
          </button>
        </header>

        <Typography
          size="sm"
          weight="medium"
          v-if="errorMessage && !isModalOpen"
          class="text-red-500 p-3 mb-4"
        >
          {{ errorMessage }}
        </Typography>

        <BooksTable
          :books="books"
          :isLoading="isLoading"
          @edit="openEditModal"
          @manage-files="openFileManager"
          @delete="handleDelete"
        />

        <div v-if="hasMore" class="flex justify-center mt-6 mb-4">
          <button
            @click="loadMore"
            :disabled="isFetchingMore"
            class="cursor-pointer bg-background border border-gray-300 text-gray-700 hover:bg-gray-50 px-6 py-2 rounded-full font-medium transition-colors disabled:opacity-50 disabled:cursor-wait flex items-center gap-2"
          >
            {{ isFetchingMore ? 'Завантаження...' : 'Завантажити ще' }}
          </button>
        </div>

        <BookFormModal
          :isOpen="isModalOpen"
          :isEditing="isEditing"
          :form="form"
          :genres="genres"
          :errorMessage="errorMessage"
          @close="closeModal"
          @submit="handleSubmit"
        />

        <BookFileManagerModal
          v-if="fileManagerBookId"
          :book="books.find((b) => b.id === fileManagerBookId)!"
          @close="closeFileManager"
          @uploaded="loadBooks"
        />
      </div>
    </Container>
  </Section>
</template>
