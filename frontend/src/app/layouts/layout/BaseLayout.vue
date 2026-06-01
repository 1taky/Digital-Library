<script setup lang="ts">
import { Navbar } from '@/shared/ui/navbar';
</script>

<template>
  <body>
    <header>
      <Navbar />
    </header>
    <main>
      <slot />
    </main>
    <footer></footer>
  </body>
</template>
