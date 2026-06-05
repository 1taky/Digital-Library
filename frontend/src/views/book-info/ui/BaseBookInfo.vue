<script setup lang="ts">
import { computed } from 'vue';

import { Container } from '@/shared/ui/container';
import { Section } from '@/shared/ui/section';
import { Typography } from '@/shared/ui/typography';

import { useBookDetails } from '../model/use-book-details';
import { BOOK_TYPE } from '../lib/dictionary';
import OrderBookModal from './order/OrderBookModal.vue';
import { useOrderBook } from '../model/use-order-book';
import { useAuthStore } from '@/app/stores/auth.ts';

const { book, isLoading, errorMessage, goBack } = useBookDetails();
const hasPaperFormat = computed(() =>
  book.value?.formats?.some((f) => f.formatType === 'Paper'),
);
const { isAuthenticated } = useAuthStore();
const bookIdForOrder = computed(() => book.value?.id);

const {
  isOpen,
  phoneNumber,
  isSubmitting,
  errorMessage: orderErrorMessage,
  successMessage,
  openModal,
  closeModal,
  submitOrder,
} = useOrderBook(bookIdForOrder);
</script>

<template>
  <Section class="mt-16">
    <Container>
      <div class="max-w-4xl mx-auto p-4 md:p-6 font-sans text-foreground">
        <button
          @click="goBack"
          class="mb-6 text-emerald-600 hover:text-emerald-800 font-medium transition-colors cursor-pointer"
        >
          &larr; Повернутися назад
        </button>

        <div
          v-if="isLoading"
          class="text-center py-20 text-muted-foreground border border-gray-200"
        >
          <Typography size="md" weight="regular">
            Завантаження інформації про книгу...
          </Typography>
        </div>

        <div
          v-else-if="errorMessage"
          class="text-center py-20 text-red-600 border border-red-200 bg-red-50"
        >
          <h2 class="text-xl font-bold mb-2">Упс!</h2>
          <p>{{ errorMessage }}</p>
        </div>

        <div v-else-if="book" class="bg-background p-1 md:p-2">
          <div
            class="border-b border-gray-200 pb-6 mb-6 flex flex-row justify-between items-center h-full"
          >
            <Typography as="h1" size="lg" weight="bold" class="text-foreground">
              {{ book.title }}
            </Typography>

            <Typography
              v-if="book.formats?.[0]?.isAvailable"
              as="span"
              align="center"
              transform="uppercase"
              size="xs"
              weight="semibold"
              class="px-6 h-8 flex bg-green-300 items-center border-accent-dark-green/50 text-accent-dark-green border rounded-full"
            >
              В наявності
            </Typography>
            <Typography
              v-else
              as="span"
              align="center"
              transform="uppercase"
              size="xs"
              weight="semibold"
              class="px-6 h-8 flex bg-muted/25 items-center border-muted/50 text-muted-foreground border rounded-full"
            >
              Немає в наявності
            </Typography>
          </div>

          <div class="grid grid-cols-1 md:grid-cols-5 gap-8">
            <div class="col-span-2 flex flex-col gap-4">
              <img
                :src="book.coverUrl || ''"
                class="w-full aspect-2/3 object-cover rounded-md border border-gray-200"
                alt="Обкладинка книги"
              />

              <button
                v-if="
                  isAuthenticated &&
                  book.formats?.[0]?.isAvailable &&
                  hasPaperFormat
                "
                @click="openModal"
                class="w-full px-6 py-3.5 bg-emerald-600 hover:bg-emerald-700 text-background font-bold rounded-md transition-colors cursor-pointer shadow-sm text-sm uppercase tracking-wider flex justify-center items-center gap-2"
              >
                Орендувати книгу
              </button>
            </div>

            <div class="col-span-3">
              <div class="border border-gray-200 bg-gray-50 p-5 mb-6">
                <Typography
                  as="h3"
                  weight="bold"
                  size="sm"
                  transform="uppercase"
                  class="text-foreground mb-4 tracking-wider"
                >
                  Інформація
                </Typography>

                <ul class="text-sm flex flex-col gap-3">
                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Автор
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{ book.author }}
                    </Typography>
                  </li>
                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Рік видання
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{ book.publicationYear }}
                    </Typography>
                  </li>
                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Жанр
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{ book.genreName }}
                    </Typography>
                  </li>

                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Тип
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{
                        book.formats?.length
                          ? book.formats
                              .map(
                                (f) => BOOK_TYPE[f.formatType] || f.formatType,
                              )
                              .join(', ')
                          : '-'
                      }}
                    </Typography>
                  </li>

                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Мова
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{ book.language }}
                    </Typography>
                  </li>

                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Сторінок
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{ book.formats?.[0]?.pagesCount || '-' }}
                    </Typography>
                  </li>
                  <li
                    class="flex justify-between border-b border-gray-200 pb-2"
                  >
                    <Typography
                      as="span"
                      size="sm"
                      weight="regular"
                      class="text-muted-foreground"
                    >
                      Довжина (годин)
                    </Typography>
                    <Typography
                      as="span"
                      size="sm"
                      weight="medium"
                      class="text-foreground"
                    >
                      {{
                        (book.formats?.[0]?.durationMinutes / 60).toFixed(1) ||
                        'Не вказано'
                      }}
                    </Typography>
                  </li>
                </ul>
              </div>

              <div class="px-5">
                <Typography
                  as="h3"
                  weight="bold"
                  size="sm"
                  transform="uppercase"
                  class="text-foreground mt-4 mb-4 tracking-wider"
                >
                  Опис
                </Typography>
                <Typography
                  size="md"
                  weight="regular"
                  class="text-foreground/90 leading-relaxed whitespace-pre-line"
                >
                  {{ book.description }}
                </Typography>
              </div>
            </div>
          </div>
        </div>

        <OrderBookModal
          v-if="book"
          :is-open="isOpen"
          :book-title="book.title"
          :is-submitting="isSubmitting"
          :error-message="orderErrorMessage"
          :success-message="successMessage"
          v-model:phone-number="phoneNumber"
          @close="closeModal"
          @submit="submitOrder"
        />
      </div>
    </Container>
  </Section>
</template>
