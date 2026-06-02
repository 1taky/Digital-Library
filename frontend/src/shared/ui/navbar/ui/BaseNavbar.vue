<script setup lang="ts">
import { useRouter } from 'vue-router';
import { Typography } from '../../typography';
import type { NavbarProps } from '../model/types';
import { useNavbar } from '../model/use-navbar';
import { useAuthStore } from '@/app/stores';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/');
};

const props = withDefaults(defineProps<NavbarProps>(), {});

const { className, sideClassName, actionClassname } = useNavbar(props);
</script>

<template>
  <nav :class="className">
    <div :class="sideClassName" class="gap-6">
      <a href="/">
        <Typography size="sm" weight="bold">EBooks</Typography>
      </a>
      <span class="bg-outline h-4 w-px"></span>
      <a :href="`/catalog`">
        <Typography size="sm" weight="medium" :class="actionClassname">
          Каталог
        </Typography>
      </a>
      <div class="w-54 bg-muted-background rounded-full">
        <input class="w-full mx-6 focus:outline-none" />
      </div>
    </div>
    <div :class="sideClassName" class="gap-4" v-if="authStore.isAuthenticated">
      <Typography size="sm" weight="medium">{{
        authStore.user?.fullName
      }}</Typography>
      <a @click="handleLogout">
        <Typography
          size="sm"
          weight="medium"
          :class="actionClassname"
          class="select-none cursor-pointer"
        >
          Вихід
        </Typography>
      </a>
    </div>
    <div :class="sideClassName" class="gap-4" v-else>
      <a :href="`/log-in`">
        <Typography size="sm" weight="medium" :class="actionClassname">
          Вхід
        </Typography>
      </a>
      <a :href="`/register`">
        <Typography size="sm" weight="medium" :class="actionClassname">
          Регістрація
        </Typography>
      </a>
    </div>
  </nav>
</template>
