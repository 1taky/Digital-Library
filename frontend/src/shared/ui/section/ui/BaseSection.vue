<script setup lang="ts">
import type { SectionProps } from '../model/types';
import { useSection } from '../model/use-section';

const props = withDefaults(defineProps<SectionProps>(), {});
const { className } = useSection(props);
</script>

<template>
  <section :class="className">
    <slot />
  </section>
</template>
