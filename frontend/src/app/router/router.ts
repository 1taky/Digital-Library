import { createRouter, createWebHistory } from 'vue-router';
import { HomePage, LogInPage, RegisterPage } from '@/views';
import { useAuthStore } from '../stores';
import { AdminDashboard } from '@/views/admin-dashboard';
import { BookInfo } from '@/views/book-info';
import CatalogPage from '@/views/catalog/CatalogPage.vue';
import OrdersPage from '@/views/order/ui/OrdersPage.vue';

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
      allowedRoles: ['Admin', 'Manager'],
    },
  },
  {
    path: '/admin/orders',
    name: 'Admin Orders',
    component: OrdersPage,
    meta: {
      requiresAuth: true,
      allowedRoles: ['Admin', 'Manager'],
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

    if (to.meta.allowedRoles) {
      const userRole = authStore.user?.role;
      const roles = to.meta.allowedRoles as string[];

      if (!userRole || !roles.includes(userRole)) {
        return next('/');
      }
    }
  }

  next();
});

export default router;
