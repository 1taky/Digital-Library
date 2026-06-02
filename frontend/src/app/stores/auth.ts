import { fetchMe } from '@/shared/api';
import type { UserType } from '@/shared/types';
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token-jwt'));
  const user = ref<UserType | null>(null);

  const isAuthenticated = computed(() => !!token.value);

  const currentRole = computed(() =>
    user.value ? user.value.role : undefined,
  );

  const setAuthData = (jwt: string, userData: UserType) => {
    token.value = jwt;
    user.value = userData;

    localStorage.setItem('token-jwt', jwt);
  };

  const logout = () => {
    token.value = null;
    user.value = null;
    localStorage.removeItem('token-jwt');
  };

  const loadUser = async () => {
    if (!token.value) return;

    try {
      const userData = await fetchMe();
      user.value = userData;
    } catch (error) {
      console.error('Error renewing session:', error);
      logout();
    }
  };

  return {
    token,
    user,
    isAuthenticated,
    currentRole,
    setAuthData,
    logout,
    loadUser,
  };
});
