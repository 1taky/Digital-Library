<script setup lang="ts">
import { Section } from '@/shared/ui/section';
import { Container } from '@/shared/ui/container';
import { Typography } from '@/shared/ui/typography';

import OrdersTable from './OrdersTable.vue';
import { useOrders } from '../model/use-orders.ts';

const {
  orders,
  isLoading,
  errorMessage,
  currentTab,
  setTab,
  handleApprove,
  handleReject,
  handleBorrow,
  handleReturn,
} = useOrders();
</script>

<template>
  <Section class="mt-20 mb-20">
    <Container>
      <div class="flex flex-col gap-6">
        <div
          class="flex flex-col md:flex-row justify-between items-start md:items-center gap-4"
        >
          <Typography as="h1" size="lg" weight="bold" class="text-gray-900">
            Керування замовленнями
          </Typography>

          <div class="flex bg-gray-100 p-1 rounded-md border border-gray-200">
            <button
              @click="setTab('all')"
              class="px-4 py-2 rounded text-sm font-medium transition-colors"
              :class="
                currentTab === 'all'
                  ? 'bg-white shadow-sm text-emerald-700'
                  : 'text-gray-600 hover:text-gray-900'
              "
            >
              Всі замовлення
            </button>
            <button
              @click="setTab('overdue')"
              class="px-4 py-2 rounded text-sm font-medium transition-colors"
              :class="
                currentTab === 'overdue'
                  ? 'bg-white shadow-sm text-red-600'
                  : 'text-gray-600 hover:text-gray-900'
              "
            >
              Прострочені
            </button>
          </div>
        </div>

        <div
          v-if="errorMessage"
          class="p-4 bg-red-50 border border-red-200 rounded-md"
        >
          <Typography size="sm" class="text-red-600">{{
            errorMessage
          }}</Typography>
        </div>

        <OrdersTable
          :orders="orders"
          :isLoading="isLoading"
          @approve="handleApprove"
          @reject="handleReject"
          @borrow="handleBorrow"
          @return="handleReturn"
        />
      </div>
    </Container>
  </Section>
</template>
