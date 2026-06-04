<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { useRegister } from './lib/useRegister';
import { Container } from '@/shared/ui/container';

const { fullName, email, password, isLoading, errorMessage, handleSubmit } = useRegister();

const confirmPassword = ref('');
const confirmPasswordError = ref('');

const onSubmit = () => {
  if (password.value !== confirmPassword.value) {
    confirmPasswordError.value = 'Паролі не співпадають';
    return;
  }
  confirmPasswordError.value = '';
  handleSubmit();
};
</script>

<template>
  <Section class="mt-20">
    <Container>
      <div class="max-w-md mx-auto bg-white rounded-2xl shadow-md p-8">
        <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">Реєстрація</h2>

        <p v-if="errorMessage" class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg p-3 mb-4">
          {{ errorMessage }}
        </p>

        <form @submit.prevent="onSubmit" class="flex flex-col gap-4">
          <div class="flex flex-col gap-1">
            <label for="fullName" class="text-sm font-medium text-gray-700">Повне ім'я</label>
            <input
              id="fullName"
              v-model="fullName"
              type="text"
              required
              :disabled="isLoading"
              placeholder="Кінг Стівен"
              class="border border-gray-300 rounded-lg px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-400 disabled:bg-gray-100 transition"
            />
          </div>

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
            <input
              id="password"
              v-model="password"
              type="password"
              required
              :disabled="isLoading"
              placeholder="••••••••"
              class="border border-gray-300 rounded-lg px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-400 disabled:bg-gray-100 transition"
            />
          </div>

          <div class="flex flex-col gap-1">
            <label for="confirmPassword" class="text-sm font-medium text-gray-700">Підтвердіть пароль</label>
            <input
              id="confirmPassword"
              v-model="confirmPassword"
              type="password"
              required
              :disabled="isLoading"
              placeholder="••••••••"
              class="border rounded-lg px-4 py-2 text-sm focus:outline-none focus:ring-2 disabled:bg-gray-100 transition"
              :class="confirmPasswordError ? 'border-red-400 focus:ring-red-300' : 'border-gray-300 focus:ring-blue-400'"
            />
            <p v-if="confirmPasswordError" class="text-red-500 text-xs mt-1">
              {{ confirmPasswordError }}
            </p>
          </div>

          <button
            type="submit"
            :disabled="isLoading"
            class="mt-2 text-white font-semibold rounded-lg py-2 transition disabled:opacity-50 disabled:cursor-not-allowed"
            style="background-color: #0C6038;"
            @mouseover="(e) => (e.currentTarget as HTMLButtonElement).style.backgroundColor = '#0a5230'"
            @mouseleave="(e) => (e.currentTarget as HTMLButtonElement).style.backgroundColor = '#0C6038'"
          >
            {{ isLoading ? 'Зачекайте...' : 'Зареєструватись' }}
          </button>
        </form>
      </div>
    </Container>
  </Section>
</template>