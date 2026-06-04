<script setup lang="ts">
import { Section } from '@/shared/ui/section';
import { useAdminBooks } from '../model/useAdminBooks';
import { Container } from '@/shared/ui/container';

const {
  books,
  genres,
  isLoading,
  isModalOpen,
  isEditing,
  form,
  handleDelete,
  openAddModal,
  openEditModal,
  closeModal,
  handleSubmit,
} = useAdminBooks();
</script>

<template>
  <Section class="mt-20"
    ><Container>
      <div class="max-w-6xl mx-auto p-4 md:p-6 font-sans text-foreground">
        <header class="flex justify-between items-center mb-6">
          <h1 class="text-2xl font-bold">Панель адміністратора</h1>
          <button
            @click="openAddModal"
            class="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-none transition-colors"
          >
            + Додати книгу
          </button>
        </header>

        <div
          v-if="isLoading"
          class="text-center text-gray-500 py-10 border border-gray-200"
        >
          Завантаження даних...
        </div>

        <div v-else class="overflow-x-auto border border-muted-background">
          <table class="w-full text-left border-collapse">
            <thead class="bg-gray-100 border-b border-gray-300">
              <tr>
                <th class="p-3 font-semibold border-r border-gray-200">ID</th>
                <th class="p-3 font-semibold border-r border-gray-200">
                  Назва
                </th>
                <th class="p-3 font-semibold border-r border-gray-200">
                  Автор
                </th>
                <th class="p-3 font-semibold border-r border-gray-200">Жанр</th>
                <th class="p-3 font-semibold border-r border-gray-200">Тип</th>
                <th class="p-3 font-semibold border-r border-gray-200">Рік</th>
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
                  {{ book.genreName }}
                </td>
                <td class="p-3 border-r border-gray-200">
                  {{ book.bookType }}
                </td>
                <td class="p-3 border-r border-gray-200">
                  {{ book.publicationYear }}
                </td>
                <td class="p-3 flex justify-center gap-2">
                  <button
                    @click="openEditModal(book)"
                    class="bg-blue-100 text-blue-700 hover:bg-blue-200 px-3 py-1 rounded-none border border-blue-300 transition-colors"
                  >
                    Редаг.
                  </button>
                  <button
                    @click="handleDelete(book.id)"
                    class="bg-red-100 text-red-700 hover:bg-red-200 px-3 py-1 rounded-none border border-red-300 transition-colors"
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

        <div
          v-if="isModalOpen"
          class="fixed inset-0 bg-black/60 flex items-center justify-center z-50 p-4"
          @click.self="closeModal"
        >
          <div
            class="bg-white border border-gray-400 w-full max-w-2xl max-h-[90vh] overflow-y-auto"
          >
            <div class="p-5 border-b border-gray-300 bg-gray-50">
              <h2 class="text-xl font-bold">
                {{ isEditing ? 'Редагувати книгу' : 'Додати нову книгу' }}
              </h2>
            </div>

            <form
              @submit.prevent="handleSubmit"
              class="p-5 flex flex-col gap-4"
            >
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Назва книги</label>
                  <input
                    v-model="form.title"
                    type="text"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600"
                  />
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Автор</label>
                  <input
                    v-model="form.author"
                    type="text"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600"
                  />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Тип</label>
                  <select
                    v-model="form.bookType"
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600 bg-white"
                  >
                    <option value="Paper">Паперова</option>
                    <option value="Electronic">Електронна</option>
                    <option value="Audio">Аудіокнига</option>
                  </select>
                </div>

                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Жанр</label>
                  <select
                    v-model="form.genreName"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600 bg-white"
                  >
                    <option value="" disabled>Оберіть жанр</option>
                    <option
                      v-for="genre in genres"
                      :key="genre.id"
                      :value="genre.name"
                    >
                      {{ genre.name }}
                    </option>
                  </select>
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Рік видання</label>
                  <input
                    v-model.number="form.publicationYear"
                    type="number"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600"
                  />
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">Мова</label>
                  <input
                    v-model="form.language"
                    type="text"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600"
                  />
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-semibold">К-сть сторінок</label>
                  <input
                    v-model.number="form.pagesCount"
                    type="number"
                    required
                    class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600"
                  />
                </div>
              </div>

              <div class="flex flex-col gap-1">
                <label class="text-sm font-semibold">Опис</label>
                <textarea
                  v-model="form.description"
                  rows="3"
                  required
                  class="border border-gray-400 p-2 rounded-none outline-none focus:border-green-600 resize-y"
                ></textarea>
              </div>

              <div
                class="flex justify-end gap-3 pt-4 border-t border-gray-300 mt-2"
              >
                <button
                  type="button"
                  @click="closeModal"
                  class="px-5 py-2 border border-gray-400 text-gray-700 hover:bg-gray-100 rounded-none transition-colors"
                >
                  Скасувати
                </button>
                <button
                  type="submit"
                  class="px-5 py-2 bg-green-600 text-white hover:bg-green-700 border border-green-700 rounded-none transition-colors"
                >
                  Зберегти
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </Container></Section
  >
</template>
