<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';

defineProps<{
  isOpen: boolean;
  bookTitle: string;
  isSubmitting: boolean;
  errorMessage: string;
  successMessage: string;
  phoneNumber: string;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'submit'): void;
  (e: 'update:phoneNumber', value: string): void;
}>();
</script>

<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 bg-black/60 flex items-center justify-center z-50 p-4"
    @click.self="emit('close')"
  >
    <div
      class="bg-background border border-gray-400 w-full max-w-lg p-8 rounded-md shadow-lg"
    >
      <Typography as="h3" weight="bold" size="lg" class="mb-3">
        Оренда паперової книги
      </Typography>
      <Typography
        as="p"
        size="md"
        weight="regular"
        class="text-muted-foreground mb-4"
      >
        Ви хочете взяти книгу <strong>"{{ bookTitle }}"</strong>. <br />
        Залиште свій номер телефону, щоб бібліотекар міг підтвердити замовлення.
      </Typography>

      <div
        v-if="successMessage"
        class="bg-green-100 text-green-800 p-3 rounded-sm mb-4 text-sm font-medium"
      >
        {{ successMessage }}
      </div>

      <form v-else @submit.prevent="emit('submit')" class="flex flex-col gap-4">
        <div class="flex flex-col gap-1">
          <Typography as="label" weight="semibold" size="sm"
            >Номер телефону</Typography
          >
          <input
            :value="phoneNumber"
            @input="
              emit(
                'update:phoneNumber',
                ($event.target as HTMLInputElement).value,
              )
            "
            type="tel"
            placeholder="+380..."
            required
            class="border border-gray-400 p-2 rounded-sm outline-none focus:border-green-600"
          />
        </div>

        <Typography
          v-if="errorMessage"
          size="sm"
          weight="medium"
          class="text-red-500"
        >
          {{ errorMessage }}
        </Typography>

        <div class="flex justify-end gap-3 mt-2">
          <button
            type="button"
            @click="emit('close')"
            class="px-4 py-2 border border-gray-400 text-gray-700 hover:bg-gray-100 rounded-full transition-colors cursor-pointer"
          >
            Скасувати
          </button>
          <button
            type="submit"
            :disabled="isSubmitting"
            class="px-4 py-2 bg-green-600 text-white hover:bg-green-700 border border-green-700 rounded-full transition-colors cursor-pointer disabled:opacity-50 flex items-center gap-2"
          >
            {{ isSubmitting ? 'Відправка...' : 'Відправити запит' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>
