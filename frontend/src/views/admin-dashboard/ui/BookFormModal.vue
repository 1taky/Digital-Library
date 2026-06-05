<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { Typography } from '@/shared/ui/typography';
import type { BookPayload } from '@/shared';
import type { GenreType } from '@/shared/types/genre';

const props = defineProps<{
  isOpen: boolean;
  isEditing: boolean;
  form: BookPayload;
  genres: GenreType[];
  errorMessage: string;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'submit'): void;
}>();

const localPagesCount = ref(1);
const localDurationMinutes = ref(0);

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen && props.form.formats) {
      localPagesCount.value =
        props.form.formats.find((f) => f.pagesCount)?.pagesCount || 1;
      localDurationMinutes.value =
        props.form.formats.find((f) => f.formatType === 'Audio')
          ?.durationMinutes || 0;
    }
  },
);

const selectedFormatTypes = computed({
  get: () => props.form.formats.map((f) => f.formatType),
  set: (newTypes) => {
    props.form.formats = newTypes.map((type) => ({
      formatType: type,
      pagesCount: localPagesCount.value,
      durationMinutes: type === 'Audio' ? localDurationMinutes.value : 0,
    }));
  },
});

watch([localPagesCount, localDurationMinutes], () => {
  props.form.formats.forEach((format) => {
    format.pagesCount = localPagesCount.value;
    if (format.formatType === 'Audio') {
      format.durationMinutes = localDurationMinutes.value;
    }
  });
});
</script>

<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 bg-black/60 flex items-center justify-center z-50 p-4"
  >
    <div
      class="bg-white border border-gray-400 w-full max-w-2xl max-h-[90vh] overflow-y-auto"
    >
      <div class="p-5 border-b border-gray-300 bg-gray-50">
        <Typography as="h2" weight="bold" size="md">
          {{ isEditing ? 'Редагувати книгу' : 'Додати нову книгу' }}
        </Typography>
      </div>

      <form @submit.prevent="$emit('submit')" class="p-5 flex flex-col gap-4">
        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div class="flex flex-col gap-1">
            <label class="text-sm font-semibold">Назва книги</label>
            <input
              v-model="form.title"
              type="text"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
            />
          </div>
          <div class="flex flex-col gap-1">
            <label class="text-sm font-semibold">Автор</label>
            <input
              v-model="form.author"
              type="text"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
            />
          </div>
        </div>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div class="flex flex-col gap-1">
            <Typography
              as="label"
              size="sm"
              weight="semibold"
              class="mt-1 mb-0.5"
              >Доступні формати</Typography
            >
            <div
              class="flex gap-4 border border-gray-400 p-2 rounded-sm bg-white h-12 items-center"
            >
              <label
                class="flex items-center gap-1.5 text-sm cursor-pointer hover:text-green-700"
              >
                <input
                  type="checkbox"
                  value="Paper"
                  v-model="selectedFormatTypes"
                  class="accent-green-600 w-4 h-4 cursor-pointer"
                />
                Паперова
              </label>
              <label
                class="flex items-center gap-1.5 text-sm cursor-pointer hover:text-green-700"
              >
                <input
                  type="checkbox"
                  value="Electronic"
                  v-model="selectedFormatTypes"
                  class="accent-green-600 w-4 h-4 cursor-pointer"
                />
                Електронна
              </label>
              <label
                class="flex items-center gap-1.5 text-sm cursor-pointer hover:text-green-700"
              >
                <input
                  type="checkbox"
                  value="Audio"
                  v-model="selectedFormatTypes"
                  class="accent-green-600 w-4 h-4 cursor-pointer"
                />
                Аудіо
              </label>
            </div>
            <span
              v-if="selectedFormatTypes.length === 0"
              class="text-xs text-red-500 font-medium"
            >
              * Оберіть хоча б один формат
            </span>
          </div>

          <div class="flex flex-col gap-1">
            <Typography
              as="label"
              size="sm"
              weight="semibold"
              class="mt-1 mb-0.5"
              >Жанр</Typography
            >
            <select
              v-model="form.genreName"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600 bg-white h-12"
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

        <div class="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div class="flex flex-col gap-1">
            <Typography as="label" size="sm" weight="semibold"
              >Рік видання</Typography
            >
            <input
              v-model.number="form.publicationYear"
              type="number"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
            />
          </div>
          <div class="flex flex-col gap-1">
            <Typography as="label" size="sm" weight="semibold">Мова</Typography>
            <input
              v-model="form.language"
              type="text"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
            />
          </div>

          <div class="flex flex-col gap-1">
            <Typography as="label" size="sm" weight="semibold"
              >К-сть сторінок</Typography
            >
            <input
              v-model.number="localPagesCount"
              type="number"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
            />
          </div>

          <div
            class="flex flex-col gap-1"
            v-if="selectedFormatTypes.includes('Audio')"
          >
            <Typography as="label" size="sm" weight="semibold"
              >Тривалість (хв)</Typography
            >
            <input
              v-model.number="localDurationMinutes"
              type="number"
              required
              class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600 bg-purple-50"
            />
          </div>
        </div>

        <div class="flex flex-col gap-1">
          <Typography as="label" size="sm" weight="semibold">Опис</Typography>
          <textarea
            v-model="form.description"
            rows="3"
            required
            class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600 resize-y"
          ></textarea>
        </div>

        <Typography
          size="sm"
          weight="medium"
          v-if="errorMessage"
          class="text-red-500 p-3 mb-4"
        >
          {{ errorMessage }}
        </Typography>

        <div class="flex justify-end gap-3 pt-4 border-t border-gray-300 mt-2">
          <button
            type="button"
            @click="$emit('close')"
            class="px-5 py-2 border border-gray-400 text-gray-700 hover:bg-gray-100 rounded-full transition-colors cursor-pointer"
          >
            Скасувати
          </button>
          <button
            type="submit"
            :disabled="selectedFormatTypes.length === 0"
            class="px-5 py-2 bg-green-600 text-white hover:bg-green-700 border border-green-700 rounded-full transition-colors cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed"
          >
            Зберегти
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
