import { createRouter, createWebHistory } from 'vue-router';
import { HomePage, LogInPage, RegisterPage } from '@/views';
import { useAuthStore } from '../stores';
import { AdminDashboard } from '@/views/admin-dashboard';
import { BookInfo } from '@/views/book-info';
import CatalogPage from '@/views/catalog/CatalogPage.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage,
    meta: { title: 'Електронна бібліотека' },
  },
  {
    path: '/log-in',
    name: 'Log-In',
    component: LogInPage,
    meta: { title: 'Вхід' },
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterPage,
    meta: { title: 'Реєстрація' },
  },
  {
    path: '/catalog',
    name: 'Catalog',
    component: CatalogPage,
    meta: { title: 'Каталог' },
  },
  {
    path: '/admin/panel',
    name: 'Admin Panel',
    component: AdminDashboard,
    meta: {
      requiresAuth: true,
      requiresAdmin: true,
    },
  },
  {
    path: '/books/:id',
    name: 'BookDetails',
    component: BookInfo,
    meta: { title: 'Деталі книги' },
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach(async (to, _, next) => {
  const authStore = useAuthStore();

  if (authStore.token && !authStore.user) {
    try {
      await authStore.loadUser();
    } catch (error) {
      console.error(error);
    }
  }
  if (to.meta.requiresAuth) {
    if (!authStore.token) {
      return next('/log-in');
    }

    if (to.meta.requiresAdmin && authStore.user?.role !== 'Admin') {
      return next('/');
    }
  }

  next();
});

export default router;
