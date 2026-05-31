<script setup lang="ts">
import { useRegister } from './lib/useRegister';

const { fullName, email, password, isLoading, errorMessage, handleSubmit } =
  useRegister();
</script>

<template>
  <form @submit.prevent="handleSubmit">
    <h2>Реєстрація</h2>

    <p v-if="errorMessage" style="color: red">
      {{ errorMessage }}
    </p>

    <div>
      <label for="fullName">Повне ім'я:</label>
      <input
        id="fullName"
        v-model="fullName"
        type="fullName"
        required
        :disabled="isLoading"
      />
    </div>
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
</template>
