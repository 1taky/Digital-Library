import { createRouter, createWebHistory } from 'vue-router';
import { HomePage, LogInPage, RegisterPage } from '@/views';

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
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});
