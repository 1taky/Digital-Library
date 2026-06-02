<script setup lang="ts">
import { Typography } from '@/shared/ui/typography';

defineProps({
  variant: {
    type: String,
    default: 'primary',
    validator: (value: unknown) =>
      typeof value === 'string' && ['primary', 'secondary'].includes(value),
  },
});

defineEmits(['click']);
</script>

<template>
  <button
    :class="[
      'px-4 py-2 rounded-full transition-colors focus:outline-none',
      variant === 'primary'
        ? 'bg-accent-yellow text-foreground transition-color duration-200 ease-in-out hover:bg-accent-yellow/70  active:bg-accent-yellow/50'
        : 'bg-background text-foreground border-2 border-foreground transition-color duration-200 ease-in-out hover:bg-foreground/5 active:bg-foreground/15',
    ]"
    @click="$emit('click', $event)"
  >
    <Typography weight="medium" size="sm"><slot /></Typography>
  </button>
</template>
