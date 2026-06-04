<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { useLogin } from './lib/useLogin';
import { Container } from '@/shared/ui/container';

const { email, password, isLoading, errorMessage, handleSubmit } = useLogin();
const showPassword = ref(false);
</script>

<template>
  <Section class="mt-20">
    <Container>
      <div class="max-w-md mx-auto bg-white rounded-2xl shadow-md p-8">
        <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">Авторизація</h2>

        <p v-if="errorMessage" class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg p-3 mb-4">
          {{ errorMessage }}
        </p>

        <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
          <div class="flex flex-col gap-1">
            <label for="email" class="text-sm font-medium text-gray-700">Email</label>
            <input
              id="email"
              v-model="email"
              type="email"
              required
              :disabled="isLoading"
              placeholder="example@email.com"
              class="border border-gray-300 rounded-lg px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-400 disabled:bg-gray-100 transition"
            />
          </div>

          <div class="flex flex-col gap-1">
            <label for="password" class="text-sm font-medium text-gray-700">Пароль</label>
            <div class="relative">
              <input
                id="password"
                v-model="password"
                :type="showPassword ? 'text' : 'password'"
                required
                :disabled="isLoading"
                placeholder="••••••••"
                class="w-full border border-gray-300 rounded-lg px-4 py-2 pr-10 text-sm focus:outline-none focus:ring-2 focus:ring-blue-400 disabled:bg-gray-100 transition"
              />
              <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 hover:text-gray-600 transition"
              >
                <svg v-if="!showPassword" xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" />
                  <path stroke-linecap="round" stroke-linejoin="round" d="M2.458 12C3.732 7.943 7.523 5 12 5c4.477 0 8.268 2.943 9.542 7-1.274 4.057-5.065 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" />
                </svg>
                <svg v-else xmlns="http://www.w3.org/2000/svg" class="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M13.875 18.825A10.05 10.05 0 0112 19c-4.477 0-8.268-2.943-9.542-7a9.97 9.97 0 012.19-3.584M6.53 6.533A9.963 9.963 0 0112 5c4.477 0 8.268 2.943 9.542 7a9.97 9.97 0 01-1.342 2.634M6.53 6.533L3 3m3.53 3.533l11.47 11.434M15 12a3 3 0 00-3-3m0 0a3 3 0 00-2.122.879M12 9l3 3" />
                </svg>
              </button>
            </div>
          </div>

          <button
  type="submit"
  :disabled="isLoading"
  class="mt-2 text-white font-semibold rounded-lg py-2 transition disabled:opacity-50 disabled:cursor-not-allowed"
  style="background-color: #0C6038;"
  @mouseover="(e) => (e.currentTarget as HTMLButtonElement).style.backgroundColor = '#0a5230'"
  @mouseleave="(e) => (e.currentTarget as HTMLButtonElement).style.backgroundColor = '#0C6038'"
>
  {{ isLoading ? 'Зачекайте...' : 'Увійти' }}
</button>
        </form>
      </div>
    </Container>
  </Section>
</template>