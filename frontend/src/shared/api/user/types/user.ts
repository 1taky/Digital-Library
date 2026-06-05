import type { UserRoleType } from '@/shared/types';

export interface User {
  id: number;
  fullName: string;
  email: string;
  role: string;
  isActive: boolean;
  createdAt: string;
}

export interface UpdateRolePayload {
  role: UserRoleType | string;
}
