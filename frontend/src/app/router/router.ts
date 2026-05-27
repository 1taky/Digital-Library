import { createRouter, createWebHistory } from 'vue-router';
import { HomePage, LogInPage, RegisterPage } from '@/views';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomePage,
  },
  {
    path: '/log-in',
    name: 'Log-In',
    component: LogInPage,
  },
  {
    path: '/register',
    name: 'Register',
    component: RegisterPage,
  },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});
