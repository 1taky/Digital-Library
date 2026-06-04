<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { useLogin } from './lib/useLogin';
import { Container } from '@/shared/ui/container';
import ShowPassword from '@/shared/icons/show-password/ui/ShowPassword.vue';
import ShowPasswordActive from '@/shared/icons/show-password/ui/ShowPasswordActive.vue';

const { email, password, isLoading, errorMessage, handleSubmit } = useLogin();
const showPassword = ref(false);
</script>

<template>
  <Section class="mt-20">
    <Container>
      <div class="max-w-md mx-auto bg-white rounded-2xl shadow-md p-8">
        <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">
          Авторизація
        </h2>

        <p
          v-if="errorMessage"
          class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg p-3 mb-4"
        >
          {{ errorMessage }}
        </p>

        <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
          <div class="flex flex-col gap-1">
            <label for="email" class="text-sm font-medium text-gray-700"
              >Email</label
            >
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
            <label for="password" class="text-sm font-medium text-gray-700"
              >Пароль</label
            >
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
                <ShowPassword v-if="!showPassword" />
                <ShowPasswordActive v-else />
              </button>
            </div>
          </div>

          <button
            type="submit"
            :disabled="isLoading"
            class="mt-2 text-white font-semibold rounded-lg py-2 transition disabled:opacity-50 disabled:cursor-not-allowed"
            style="background-color: #0c6038"
            @mouseover="
              (e) =>
                ((e.currentTarget as HTMLButtonElement).style.backgroundColor =
                  '#0a5230')
            "
            @mouseleave="
              (e) =>
                ((e.currentTarget as HTMLButtonElement).style.backgroundColor =
                  '#0C6038')
            "
          >
            {{ isLoading ? 'Зачекайте...' : 'Увійти' }}
          </button>
        </form>
      </div>
    </Container>
  </Section>
</template>
