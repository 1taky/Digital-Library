<script setup lang="ts">
import { onMounted } from 'vue';
import { useAuthStore } from '@/app/stores';

const authStore = useAuthStore();
onMounted(() => {
  authStore.loadUser();
});
</script>

<template>
  <router-view></router-view>
</template>
