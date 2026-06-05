export type OrderStatus =
  | 'Requested'
  | 'Approved'
  | 'Borrowed'
  | 'Returned'
  | 'Rejected'
  | 'Overdue';

export interface Order {
  id: number;
  bookId: number;
  bookTitle: string;
  userId: number;
  userFullName: string;
  userEmail: string;
  phoneNumber: string;
  borrowedAt: string | null;
  dueDate: string | null;
  returnedAt: string | null;
  status: OrderStatus | string;
  managerId: number | null;
  managerFullName: string | null;
  createdAt: string;
}

export interface OrderRequestPayload {
  bookId: number;
  phoneNumber: string;
}
