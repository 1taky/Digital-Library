<script setup lang="ts">
import { onMounted } from 'vue';
import { useAuthStore } from '@/app/stores';
import { Layout } from '@/app/layouts';

const authStore = useAuthStore();
onMounted(() => {
  authStore.loadUser();
});
</script>

<template>
  <Layout>
    <router-view />
  </Layout>
</template>
