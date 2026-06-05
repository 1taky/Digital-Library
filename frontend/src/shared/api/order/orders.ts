import { apiClient, type Order, type OrderRequestPayload } from '@/shared';

export const createOrderRequest = async (
  payload: OrderRequestPayload,
): Promise<Order> => {
  const response = await apiClient.post<Order>('/orders/request', payload);
  return response.data;
};

export const fetchMyOrders = async (): Promise<Order[]> => {
  const response = await apiClient.get<Order[]>('/orders/my');
  return response.data;
};

export const fetchAllOrders = async (): Promise<Order[]> => {
  const response = await apiClient.get<Order[]>('/orders');
  return response.data;
};

export const fetchOrder = async (id: number): Promise<Order> => {
  const response = await apiClient.get<Order>(`/orders/${id}`);
  return response.data;
};

export const fetchOverdueOrders = async (): Promise<Order[]> => {
  const response = await apiClient.get<Order[]>('/orders/overdue');
  return response.data;
};

export const approveOrder = async (id: number): Promise<Order> => {
  const response = await apiClient.patch<Order>(`/orders/${id}/approve`);
  return response.data;
};

export const borrowOrder = async (id: number): Promise<Order> => {
  const response = await apiClient.patch<Order>(`/orders/${id}/borrow`);
  return response.data;
};

export const returnOrder = async (id: number): Promise<Order> => {
  const response = await apiClient.patch<Order>(`/orders/${id}/return`);
  return response.data;
};

export const rejectOrder = async (id: number): Promise<Order> => {
  const response = await apiClient.patch<Order>(`/orders/${id}/reject`);
  return response.data;
};
