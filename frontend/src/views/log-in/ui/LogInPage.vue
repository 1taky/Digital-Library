<script setup lang="ts">
import { ref } from 'vue';
import { Section } from '@/shared/ui/section';
import { useLogin } from '../lib/useLogin';
import { Container } from '@/shared/ui/container';
import ShowPassword from '@/shared/icons/show-password/ui/ShowPassword.vue';
import ShowPasswordActive from '@/shared/icons/show-password/ui/ShowPasswordActive.vue';
import { Typography } from '@/shared/ui/typography';

const { email, password, isLoading, errorMessage, handleSubmit } = useLogin();
const showPassword = ref(false);
</script>

<template>
  <Section class="mt-20">
    <Container>
      <div class="max-w-md mx-auto bg-white rounded-2xl shadow-md p-8">
        <Typography
          as="h2"
          weight="bold"
          size="lg"
          align="center"
          class="text-foreground mb-6"
          >Авторизація</Typography
        >
        <Typography
          size="sm"
          weight="medium"
          v-if="errorMessage"
          class="text-red-500 p-3 mb-4"
        >
          {{ errorMessage }}
        </Typography>
        <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
          <div class="flex flex-col gap-1">
            <Typography
              as="label"
              for="email"
              size="sm"
              weight="medium"
              class="text-muted-foreground"
              >Email</Typography
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
            <Typography
              as="label"
              for="password"
              size="sm"
              weight="medium"
              class="text-muted-foreground"
              >Пароль</Typography
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
                class="absolute right-3 top-1/2 -translate-y-1/2 text-muted hover:text-muted-foreground transition"
              >
               <ShowPasswordActive v-if="!showPassword"/>
              <ShowPassword  v-else  />
                
              </button>
            </div>
          </div>

          <button
            type="submit"
            :disabled="isLoading"
            class="bg-accent-dark-green/90 hover:bg-accent-dark-green active:bg-accent-dark-green/80 mt-2 text-background font-semibold rounded-full py-2 transition disabled:opacity-50 disabled:cursor-not-allowed cursor-pointer"
          >
            {{ isLoading ? 'Зачекайте...' : 'Увійти' }}
          </button>
        </form>
      </div>
    </Container>
  </Section>
</template>
