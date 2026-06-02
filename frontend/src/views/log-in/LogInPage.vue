<script setup lang="ts">
import { Section } from '@/shared/ui/section';
import { useLogin } from './lib/useLogin';
import { Container } from '@/shared/ui/container';

const { email, password, isLoading, errorMessage, handleSubmit } = useLogin();
</script>

<template>
  <Section class="mt-20">
    <Container>
      <form @submit.prevent="handleSubmit">
        <h2>Авторизація</h2>

        <p v-if="errorMessage" style="color: red">
          {{ errorMessage }}
        </p>

        <div>
          <label for="email">Email:</label>
          <input
            id="email"
            v-model="email"
            type="email"
            required
            :disabled="isLoading"
          />
        </div>

        <div>
          <label for="password">Пароль:</label>
          <input
            id="password"
            v-model="password"
            type="password"
            required
            :disabled="isLoading"
          />
        </div>

        <button type="submit" :disabled="isLoading">
          {{ isLoading ? 'Зачекайте...' : 'Увійти' }}
        </button>
      </form>
    </Container>
  </Section>
</template>
