<script setup lang="ts">
import { Section } from '@/shared/ui/section';
import { Container } from '@/shared/ui/container';
import { Typography } from '@/shared/ui/typography';
import { useAdminUsers } from '../model/use-admin-users';
import AdminUsersTable from './AdminUsersTable.vue';

const { users, isLoading, errorMessage, handleRoleChange, handleToggleBlock } =
  useAdminUsers();
</script>

<template>
  <Section class="mt-20 mb-20">
    <Container>
      <div class="flex flex-col gap-6">
        <div class="flex justify-between items-center">
          <Typography as="h1" size="lg" weight="bold" class="text-gray-900">
            Керування користувачами
          </Typography>
        </div>

        <div
          v-if="errorMessage"
          class="p-4 bg-red-50 border border-red-200 rounded-md"
        >
          <Typography size="sm" class="text-red-600">{{
            errorMessage
          }}</Typography>
        </div>

        <AdminUsersTable
          :users="users"
          :isLoading="isLoading"
          @change-role="handleRoleChange"
          @toggle-block="handleToggleBlock"
        />
      </div>
    </Container>
  </Section>
</template>
