<script setup lang="ts">
import type { ContainerProps } from '../model/types';
import { useContainer } from '../model/use-container';

const props = withDefaults(defineProps<ContainerProps>(), {});
const { className } = useContainer(props);
</script>

<template>
  <section :class="className">
    <slot />
  </section>
</template>
