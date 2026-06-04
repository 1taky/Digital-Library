<script setup lang="ts">
import type { BaseIconProps } from '@/shared/ui/icon';
import { Icon } from '@/shared/ui/icon';

const props = withDefaults(defineProps<BaseIconProps>(), {
  size: 24,
  viewBox: '0 0 24 24',
});
</script>

<template>
  <Icon v-bind="props">
    <path
      fill="none"
      stroke="currentColor"
      stroke-width="2"
      stroke-linecap="round"
      stroke-linejoin="round"
      d="M13.875 18.825A10.05 10.05 0 0112 19c-4.477 0-8.268-2.943-9.542-7a9.97 9.97 0 012.19-3.584M6.53 6.533A9.963 9.963 0 0112 5c4.477 0 8.268 2.943 9.542 7a9.97 9.97 0 01-1.342 2.634M6.53 6.533L3 3m3.53 3.533l11.47 11.434M15 12a3 3 0 00-3-3m0 0a3 3 0 00-2.122.879M12 9l3 3"
    />
  </Icon>
</template>
