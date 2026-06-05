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
import { useBookMedia } from '../model/use-book-media.ts';

const { book, isLoading, errorMessage, goBack } = useBookDetails();

const { isAuthenticated } = useAuthStore();

const bookIdForOrder = computed(() => book.value?.id);

const hasPaperFormat = computed(() =>
  book.value?.formats?.some((f) => f.formatType === 'Paper'),
);
const hasElectronicFormat = computed(() =>
  book.value?.formats?.some((f) => f.formatType === 'Electronic'),
);
const hasAudioFormat = computed(() =>
  book.value?.formats?.some((f) => f.formatType === 'Audio'),
);

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
const {
  isDownloading,
  isAudioOpening,
  downloadFileWithAuth,
  listenAudioWithAuth,
} = useBookMedia();
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
              <button
                v-if="hasElectronicFormat && book.downloadUrl"
                @click="downloadFileWithAuth(book.downloadUrl, book.title)"
                :disabled="isDownloading"
                class="w-full px-6 py-3 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-md transition-colors cursor-pointer shadow-sm text-sm uppercase tracking-wider flex justify-center items-center gap-2 disabled:opacity-75 disabled:cursor-wait"
              >
                <svg
                  v-if="isDownloading"
                  class="animate-spin h-5 w-5 text-white"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <circle
                    class="opacity-25"
                    cx="12"
                    cy="12"
                    r="10"
                    stroke="currentColor"
                    stroke-width="4"
                  ></circle>
                  <path
                    class="opacity-75"
                    fill="currentColor"
                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                  ></path>
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke-width="2"
                  stroke="currentColor"
                  class="w-5 h-5"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    d="M3 16.5v2.25A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75V16.5M16.5 12 12 16.5m0 0L7.5 12m4.5 4.5V3"
                  />
                </svg>
                {{ isDownloading ? 'Завантаження...' : 'Завантажити файл' }}
              </button>

              <button
                v-if="hasAudioFormat && book.listenUrl"
                @click="listenAudioWithAuth(book.listenUrl)"
                :disabled="isAudioOpening"
                class="w-full px-6 py-3 bg-purple-600 hover:bg-purple-700 text-white font-bold rounded-md transition-colors cursor-pointer shadow-sm text-sm uppercase tracking-wider flex justify-center items-center gap-2 disabled:opacity-75 disabled:cursor-wait"
              >
                <svg
                  v-if="isAudioOpening"
                  class="animate-spin h-5 w-5 text-white"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                >
                  <circle
                    class="opacity-25"
                    cx="12"
                    cy="12"
                    r="10"
                    stroke="currentColor"
                    stroke-width="4"
                  ></circle>
                  <path
                    class="opacity-75"
                    fill="currentColor"
                    d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"
                  ></path>
                </svg>
                <svg
                  v-else
                  xmlns="http://www.w3.org/2000/svg"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke-width="2"
                  stroke="currentColor"
                  class="w-5 h-5"
                >
                  <path
                    stroke-linecap="round"
                    stroke-linejoin="round"
                    d="M5.25 5.653c0-.856.917-1.398 1.667-.986l11.54 6.347a1.125 1.125 0 0 1 0 1.972l-11.54 6.347a1.125 1.125 0 0 1-1.667-.986V5.653Z"
                  />
                </svg>
                {{ isAudioOpening ? 'Відкриття...' : 'Слухати аудіо' }}
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
