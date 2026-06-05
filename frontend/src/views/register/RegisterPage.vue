<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { useRegister } from './lib/useRegister';
import { Container } from '@/shared/ui/container';
import { Typography } from '@/shared/ui/typography';
import ShowPassword from '@/shared/icons/show-password/ui/ShowPassword.vue';
import ShowPasswordActive from '@/shared/icons/show-password/ui/ShowPasswordActive.vue';

const { fullName, email, password, isLoading, errorMessage, handleSubmit } = useRegister();

const confirmPassword = ref('');
const confirmPasswordError = ref('');
const showPassword = ref(false);
const showConfirmPassword = ref(false);

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
        <Typography as="h1" weight="bold" size="lg" align="center" class="text-gray-800 mb-6">
          Реєстрація
        </Typography>

        <p v-if="errorMessage" class="text-red-500 text-sm bg-red-50 border border-red-200 rounded-lg p-3 mb-4">
          {{ errorMessage }}
        </p>

        <form @submit.prevent="onSubmit" class="flex flex-col gap-4">
          <div class="flex flex-col gap-1">
            <Typography as="label" for="fullName" weight="medium" size="sm">
              Повне ім'я
            </Typography>
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
            <Typography as="label" for="email" weight="medium" size="sm">
              Email
            </Typography>
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
            <Typography as="label" for="password" weight="medium" size="sm">
              Пароль
            </Typography>
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
  @click="showConfirmPassword = !showConfirmPassword"
  class="absolute right-3 top-1/2 -translate-y-1/2 text-muted hover:text-muted-foreground transition"
>
  <ShowPasswordActive v-if="!showConfirmPassword"/>
  <ShowPassword v-else />
</button>
            </div>
          </div>

          <div class="flex flex-col gap-1">
            <Typography as="label" for="confirmPassword" weight="medium" size="sm">
              Підтвердіть пароль
            </Typography>
            <div class="relative">
              <input
                id="confirmPassword"
                v-model="confirmPassword"
                :type="showConfirmPassword ? 'text' : 'password'"
                required
                :disabled="isLoading"
                placeholder="••••••••"
                class="w-full border rounded-lg px-4 py-2 pr-10 text-sm focus:outline-none focus:ring-2 disabled:bg-gray-100 transition"
                :class="confirmPasswordError ? 'border-red-400 focus:ring-red-300' : 'border-gray-300 focus:ring-blue-400'"
              
                />
                    <button
                type="button"
                @click="showPassword = !showPassword"
                class="absolute right-3 top-1/2 -translate-y-1/2 text-muted hover:text-muted-foreground transition"
              >
              <ShowPasswordActive v-if="!showPassword"/>
              <ShowPassword  v-else  />
                
              </button>
            </div>
            <Typography v-if="confirmPasswordError" as="p" size="sm" class="text-red-500 mt-1">
              {{ confirmPasswordError }}
            </Typography>
          </div>

          <button
            type="submit"
            :disabled="isLoading"
            class="mt-2 text-white font-semibold rounded-full py-2 transition disabled:opacity-50 disabled:cursor-not-allowed bg-accent-dark-green hover:bg-accent-dark-green/80"
          >
            {{ isLoading ? 'Зачекайте...' : 'Зареєструватись' }}
          </button>
        </form>
      </div>
    </Container>
  </Section>
</template>