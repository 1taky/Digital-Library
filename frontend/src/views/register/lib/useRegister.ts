import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '@/app/stores';
import { registerUser } from '@/shared';

export const useRegister = () => {
  const fullName = ref('');
  const email = ref('');
  const password = ref('');
  const isLoading = ref(false);
  const errorMessage = ref('');

  const authStore = useAuthStore();
  const router = useRouter();

  const handleSubmit = async () => {
    isLoading.value = true;
    errorMessage.value = '';

    try {
      const response = await registerUser({
        fullName: fullName.value,
        email: email.value,
        password: password.value,
      });

      const { token, ...userData } = response;

      authStore.setAuthData(token, userData);
      router.push('/');
    } catch (error: any) {
      console.log(error);
      errorMessage.value =
        error.response?.data?.message || 'Authorization error';
    } finally {
      isLoading.value = false;
    }
  };

  return {
    fullName,
    email,
    password,
    isLoading,
    errorMessage,
    handleSubmit,
  };
};
