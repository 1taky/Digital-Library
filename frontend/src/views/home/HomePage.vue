<script setup lang="ts">
import { useAuthStore } from '@/app/stores';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/');
};
const goToRegister = () => {
  router.push('/register');
};
const goToLogin = () => {
  router.push('/log-in');
};
</script>

<template>
  <div>
    <h1>Головна сторінка</h1>

    <div v-if="authStore.user">
      <p>Привіт, {{ authStore.user.fullName }}!</p>
      <button @click="handleLogout">Вийти</button>
    </div>

    <div v-else>
      <p>Ви не увійшли в систему.</p>
      <button @click="goToRegister">Реєстрація</button>

      <button @click="goToLogin" style="margin-left: 10px">Увійти</button>
    </div>
  </div>
</template>
