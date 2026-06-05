<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';

import type { Book } from '@/shared';
import { useBookFileManager } from '../model/use-book-file-manager';
import { computed } from 'vue';
import { useAudioPlayer } from '../model/use-audio-player';

const props = defineProps<{
  book: Book;
}>();

const emit = defineEmits<{
  (e: 'close'): void;
  (e: 'uploaded'): void;
}>();

const {
  isUploadingCover,
  isUploadingFile,
  isUploadingAudio,
  message,
  handleUploadCover,
  handleUploadFile,
  handleUploadAudio,
  downloadFileWithAuth,
  fetchAudioBlobUrl,
} = useBookFileManager();

const hasElectronicFormat = computed(() =>
  props.book.formats?.some((f) => f.formatType === 'Electronic'),
);

const hasAudioFormat = computed(() =>
  props.book.formats?.some((f) => f.formatType === 'Audio'),
);
const onCoverUpload = async (e: Event) => {
  const success = await handleUploadCover(props.book.id, e);
  if (success) emit('uploaded');
};

const onFileUpload = async (e: Event) => {
  const success = await handleUploadFile(props.book.id, e);
  if (success) emit('uploaded');
};

const onAudioUpload = async (e: Event) => {
  const success = await handleUploadAudio(props.book.id, e);
  if (success) emit('uploaded');
};

const { audioPlayerUrl, isAudioLoading, loadAudioPlayer } = useAudioPlayer();
</script>

<template>
  <div
    class="fixed inset-0 bg-black/60 flex items-center justify-center z-60 p-4"
    @click.self="emit('close')"
  >
    <div class="bg-white border border-gray-400 w-full max-w-lg">
      <div
        class="p-5 border-b border-gray-300 bg-gray-50 flex justify-between items-center"
      >
        <div>
          <Typography as="h2" weight="bold" size="md"
            >Керування файлами</Typography
          >
          <Typography as="p" size="sm" class="text-gray-500 mt-1"
            >Книга: {{ props.book.title }}</Typography
          >
        </div>
        <button
          @click="emit('close')"
          class="text-gray-500 hover:text-gray-800 text-xl font-bold px-2 cursor-pointer"
        >
          &times;
        </button>
      </div>

      <div
        v-if="message.text"
        :class="
          message.type === 'success'
            ? 'bg-green-100 text-green-800 border-green-300'
            : 'bg-red-100 text-red-800 border-red-300'
        "
        class="p-3 border-b text-sm font-medium text-center transition-all"
      >
        {{ message.text }}
      </div>

      <div class="p-6 flex flex-col gap-6">
        <div class="border border-gray-200 p-4 bg-gray-50 relative">
          <div class="flex justify-between items-start mb-2">
            <Typography as="h3" weight="semibold" size="sm"
              >Обкладинка (Зображення)</Typography
            >
            <span
              v-if="props.book.coverUrl"
              class="text-xs bg-green-200 text-green-800 px-2 py-1 rounded-full font-medium"
              >Завантажено</span
            >
            <span
              v-else
              class="text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full font-medium"
              >Немає файлу</span
            >
          </div>
          <img
            v-if="props.book.coverUrl"
            :src="props.book.coverUrl"
            target="_blank"
            class="size-32"
          />

          <div class="flex items-center gap-3 mt-2">
            <input
              type="file"
              accept="image/*"
              @change="onCoverUpload"
              :disabled="isUploadingCover"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:border-0 file:text-sm file:font-semibold file:bg-green-100 file:text-green-700 hover:file:bg-green-200 cursor-pointer disabled:opacity-50"
            />
            <span
              v-if="isUploadingCover"
              class="text-sm text-gray-500 animate-pulse"
              >...</span
            >
          </div>
        </div>

        <div
          v-if="hasElectronicFormat"
          class="border border-gray-200 p-4 bg-gray-50"
        >
          <div class="flex justify-between items-start mb-2">
            <Typography as="h3" weight="semibold" size="sm"
              >Файл книги (PDF, EPUB, FB2)</Typography
            >
            <span
              v-if="props.book.downloadUrl"
              class="text-xs bg-green-200 text-green-800 px-2 py-1 rounded-full font-medium"
              >Завантажено</span
            >
            <span
              v-else
              class="text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full font-medium"
              >Немає файлу</span
            >
          </div>
          <button
            v-if="props.book.downloadUrl"
            @click="
              downloadFileWithAuth(
                props.book.downloadUrl,
                `${props.book.title}.pdf`,
              )
            "
            class="text-blue-600 text-sm hover:underline mb-3 block text-left cursor-pointer"
          >
            Завантажити поточний файл
          </button>

          <div class="flex items-center gap-3 mt-2">
            <input
              type="file"
              accept=".pdf,.epub,.fb2,.txt"
              @change="onFileUpload"
              :disabled="isUploadingFile"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:border-0 file:text-sm file:font-semibold file:bg-blue-100 file:text-blue-700 hover:file:bg-blue-200 cursor-pointer disabled:opacity-50"
            />
            <span
              v-if="isUploadingFile"
              class="text-sm text-gray-500 animate-pulse"
              >...</span
            >
          </div>
        </div>

        <div
          v-if="hasAudioFormat"
          class="border border-gray-200 p-4 bg-gray-50"
        >
          <div class="flex justify-between items-start mb-2">
            <Typography as="h3" weight="semibold" size="sm"
              >Аудіокнига (MP3, WAV)</Typography
            >
            <span
              v-if="props.book.listenUrl"
              class="text-xs bg-green-200 text-green-800 px-2 py-1 rounded-full font-medium"
              >Завантажено</span
            >
            <span
              v-else
              class="text-xs bg-yellow-100 text-yellow-800 px-2 py-1 rounded-full font-medium"
              >Немає файлу</span
            >
          </div>
          <button
            @click="loadAudioPlayer(props.book.listenUrl, fetchAudioBlobUrl)"
            :disabled="isAudioLoading"
            class="text-blue-600 text-sm hover:underline flex items-center gap-2 cursor-pointer disabled:opacity-50 disabled:cursor-wait"
          >
            <span v-if="isAudioLoading">Завантаження...</span>
            <span v-else>Показати аудіо</span>
          </button>

          <audio
            v-if="audioPlayerUrl"
            controls
            :src="audioPlayerUrl"
            class="w-full h-10 outline-none rounded-md mt-2"
          ></audio>

          <div class="flex items-center gap-3 mt-2">
            <input
              type="file"
              accept="audio/*"
              @change="onAudioUpload"
              :disabled="isUploadingAudio"
              class="block w-full text-sm text-gray-500 file:mr-4 file:py-2 file:px-4 file:border-0 file:text-sm file:font-semibold file:bg-purple-100 file:text-purple-700 hover:file:bg-purple-200 cursor-pointer disabled:opacity-50"
            />
            <span
              v-if="isUploadingAudio"
              class="text-sm text-gray-500 animate-pulse"
              >...</span
            >
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
