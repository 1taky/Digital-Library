<script setup lang="ts">
import { Navbar } from '@/shared/ui/navbar';
</script>

<template>
  <div class="overflow-hidden">
    <header>
      <Navbar />
    </header>
    <main>
      <slot />
    </main>
    <footer></footer>
  </div>
</template>
