<script setup lang="ts">
import { onMounted } from 'vue';
import { useAuthStore } from '@/app/stores';
import { Layout } from '@/app/layouts';
import { RouterView } from 'vue-router';

const authStore = useAuthStore();
onMounted(() => {
  authStore.loadUser();
});
</script>

<template>
  <Layout>
    <RouterView />
  </Layout>
</template>
