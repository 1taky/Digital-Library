import { ref, onMounted } from 'vue';
import {
  fetchUsers,
  updateUserRole,
  blockUser,
  unblockUser,
  type User,
} from '@/shared';

export const useAdminUsers = () => {
  const users = ref<User[]>([]);
  const isLoading = ref(false);
  const errorMessage = ref('');

  const loadUsers = async () => {
    isLoading.value = true;
    errorMessage.value = '';
    try {
      users.value = await fetchUsers();
      users.value.sort(
        (a, b) =>
          new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime(),
      );
    } catch (error: any) {
      console.error('Помилка завантаження користувачів:', error);
      errorMessage.value = 'Не вдалося завантажити список користувачів.';
    } finally {
      isLoading.value = false;
    }
  };

  const updateLocalUser = (updatedUser: User) => {
    const index = users.value.findIndex((u) => u.id === updatedUser.id);
    if (index !== -1) {
      users.value[index] = updatedUser;
    }
  };

  const handleRoleChange = async (userId: number, newRole: string) => {
    try {
      const updated = await updateUserRole(userId, {
        role: newRole,
      });
      updateLocalUser(updated);
    } catch (error: any) {
      console.error('Помилка зміни ролі:', error);
      alert('Не вдалося змінити роль користувача.');
      await loadUsers();
    }
  };

  const handleToggleBlock = async (user: User) => {
    const actionText = user.isActive ? 'заблокувати' : 'розблокувати';
    if (!confirm(`Ви впевнені, що хочете ${actionText} цього користувача?`))
      return;

    try {
      let updated: User;
      if (user.isActive) {
        updated = await blockUser(user.id);
      } else {
        updated = await unblockUser(user.id);
      }
      updateLocalUser(updated);
    } catch (error: any) {
      console.error(`Помилка під час спроби ${actionText}:`, error);
      alert(`Не вдалося ${actionText} користувача.`);
    }
  };

  onMounted(() => {
    loadUsers();
  });

  return {
    users,
    isLoading,
    errorMessage,
    handleRoleChange,
    handleToggleBlock,
  };
};
