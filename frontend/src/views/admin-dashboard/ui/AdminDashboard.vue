<script setup lang="ts">
import { Container } from '@/shared/ui/container';
import { Section } from '@/shared/ui/section';
import { useAdminBooks } from '../model/useAdminBooks';

const {
  books,
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
      <div class="max-w-5xl mx-auto p-5">
        <header class="flex justify-between items-center mb-6">
          <h1 class="text-2xl font-bold text-gray-800">
            Панель адміністратора
          </h1>
          <button
            @click="openAddModal"
            class="bg-emerald-500 hover:bg-emerald-600 text-white font-semibold py-2 px-4 rounded transition-colors"
          >
            + Додати книгу
          </button>
        </header>

        <div v-if="isLoading" class="text-center text-gray-500 py-10">
          Завантаження даних...
        </div>

        <div v-else class="overflow-x-auto bg-white rounded-lg shadow">
          <table class="w-full text-left border-collapse">
            <thead class="bg-gray-50 border-b border-gray-200">
              <tr>
                <th class="p-4 font-semibold text-gray-600">ID</th>
                <th class="p-4 font-semibold text-gray-600">Назва</th>
                <th class="p-4 font-semibold text-gray-600">Автор</th>
                <th class="p-4 font-semibold text-gray-600">Тип</th>
                <th class="p-4 font-semibold text-gray-600">Рік</th>
                <th class="p-4 font-semibold text-gray-600">Статус</th>
                <th class="p-4 font-semibold text-gray-600 text-right">Дії</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="book in books"
                :key="book.id"
                class="border-b border-gray-100 hover:bg-gray-50"
              >
                <td class="p-4 text-gray-500">{{ book.id }}</td>
                <td class="p-4 font-medium text-gray-900">{{ book.title }}</td>
                <td class="p-4 text-gray-700">{{ book.author }}</td>
                <td class="p-4 text-gray-600">{{ book.bookType }}</td>
                <td class="p-4 text-gray-600">{{ book.publicationYear }}</td>
                <td class="p-4">
                  <span
                    :class="
                      book.isAvailable
                        ? 'text-emerald-600 bg-emerald-100'
                        : 'text-red-600 bg-red-100'
                    "
                    class="px-2 py-1 rounded text-sm font-medium"
                  >
                    {{ book.isAvailable ? 'Доступна' : 'Недоступна' }}
                  </span>
                </td>
                <td class="p-4 text-right space-x-2">
                  <button
                    @click="openEditModal(book)"
                    class="bg-amber-400 hover:bg-amber-500 text-amber-900 font-medium py-1.5 px-3 rounded text-sm transition-colors"
                  >
                    Редагувати
                  </button>
                  <button
                    @click="handleDelete(book.id)"
                    class="bg-rose-500 hover:bg-rose-600 text-white font-medium py-1.5 px-3 rounded text-sm transition-colors"
                  >
                    Видалити
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <div
          v-if="isModalOpen"
          class="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 p-4"
          @click.self="closeModal"
        >
          <div
            class="bg-white rounded-xl shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto"
          >
            <div class="p-6 border-b border-gray-100">
              <h2 class="text-xl font-bold text-gray-800">
                {{ isEditing ? 'Редагувати книгу' : 'Додати нову книгу' }}
              </h2>
            </div>

            <form @submit.prevent="handleSubmit" class="p-6 space-y-4">
              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700"
                    >Назва книги</label
                  >
                  <input
                    v-model="form.title"
                    type="text"
                    required
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none"
                  />
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700">Автор</label>
                  <input
                    v-model="form.author"
                    type="text"
                    required
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none"
                  />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700">Тип</label>
                  <select
                    v-model="form.bookType"
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none bg-white"
                  >
                    <option value="Paper">Паперова</option>
                    <option value="Electronic">Електронна</option>
                    <option value="Audio">Аудіокнига</option>
                  </select>
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700"
                    >ID Жанру</label
                  >
                  <input
                    v-model.number="form.genreId"
                    type="number"
                    required
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none"
                  />
                </div>
              </div>

              <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700"
                    >Рік видання</label
                  >
                  <input
                    v-model.number="form.publicationYear"
                    type="number"
                    required
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none"
                  />
                </div>
                <div class="flex flex-col gap-1">
                  <label class="text-sm font-medium text-gray-700">Мова</label>
                  <input
                    v-model="form.language"
                    type="text"
                    required
                    class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none"
                  />
                </div>
              </div>

              <div class="flex flex-col gap-1">
                <label class="text-sm font-medium text-gray-700">Опис</label>
                <textarea
                  v-model="form.description"
                  rows="3"
                  required
                  class="border border-gray-300 rounded-md p-2 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none resize-none"
                ></textarea>
              </div>

              <div v-if="isEditing" class="flex items-center gap-2 pt-2">
                <input
                  v-model="form.isAvailable"
                  type="checkbox"
                  id="availability"
                  class="w-4 h-4 text-emerald-600 rounded border-gray-300 focus:ring-emerald-500"
                />
                <label
                  for="availability"
                  class="text-sm text-gray-700 cursor-pointer"
                  >Книга доступна для користувачів</label
                >
              </div>

              <div class="flex justify-end gap-3 pt-4 border-t border-gray-100">
                <button
                  type="button"
                  @click="closeModal"
                  class="px-4 py-2 bg-white border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50 font-medium transition-colors"
                >
                  Скасувати
                </button>
                <button
                  type="submit"
                  class="px-4 py-2 bg-emerald-500 text-white rounded-md hover:bg-emerald-600 font-medium transition-colors shadow-sm"
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
