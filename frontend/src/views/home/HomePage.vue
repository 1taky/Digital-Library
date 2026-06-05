<script setup lang="ts">
import { Container } from '@/shared/ui/container';
import { Section } from '@/shared/ui/section';
import { Typography } from '@/shared/ui/typography';
import { useBooks } from './model/use-books';

const { books } = useBooks();
</script>

<template>
  <Section class-name="mt-20">
    <Container>
      <Typography as="h1">Головна сторінка</Typography>
      <div v-for="book in books" genre="Хорор">{{ book.title }}</div>
      <div v-for="book in books" genre="Драма">{{ book.title }}</div>
    </Container>
  </Section>
</template>
