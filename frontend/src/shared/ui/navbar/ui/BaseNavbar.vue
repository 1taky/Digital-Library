<script setup lang="ts">
import { Typography } from '../../typography';
import type { NavbarProps } from '../model/types';
import { useNavbar } from '../model/useNavbar';

const props = withDefaults(defineProps<NavbarProps>(), {});

const labels = {
  navbar: {
    community: 'Community',
    rankings: 'Rankings',
    analytics: 'Analytics',
    about: 'About',
  },
};
const { className, sideClassName, actionClassname } = useNavbar(props);
</script>

<template>
  <nav :class="className">
    <div :class="sideClassName" class="gap-6">
      <a href="/">
        <Typography size="sm" weight="medium"> EBooks </Typography>
      </a>
      <span class="bg-outline h-4 w-px"></span>
      <a v-for="(value, key) in labels.navbar" :key="key" :href="`/${key}`">
        <Typography size="sm" weight="medium" :class="actionClassname">
          {{ value }}
        </Typography>
      </a>
    </div>
    <div :class="sideClassName" class="gap-4">
      <Typography size="sm" weight="medium"> o </Typography>
    </div>
  </nav>
</template>
