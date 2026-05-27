import type { UserType } from '@/shared/types';
import { defineStore } from 'pinia';
import { ref, computed } from 'vue';

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem('token-jwt'));
  const user = ref<UserType | null>(null);

  const isAuthenticated = computed(() => !!token.value);

  // перевірка ролі | треба узгодження типів з бекендом
  // const currentRole = computed<RoleType>(() => {
  //   return user.value ? user.value.role : 'guest';
  // });

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

  return {
    token,
    user,
    isAuthenticated,
    // currentRole,
    setAuthData,
    logout,
  };
});
