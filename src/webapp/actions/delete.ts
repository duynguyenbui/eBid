'use server';

import axios from 'axios';
import { getToken } from './token';

export const deleteItem = async (id: string) => {
  try {
    const token = getToken();

    if (!token) {
      return { error: 'Failed to get token' };
    }

    const response = await axios.delete(
      `${process.env.AUCTION_API_URL}/api/auction/items/${id}?api-version=${process.env.AUCTION_API_URL_VERSION}`,
      {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (response.status === 204) {
      return { success: 'Item deleted successfully' };
    }

    if (response.status === 400) {
      return {
        error: response.data.message || 'Failed to delete item',
      };
    }

    return {
      error: 'Failed to delete item',
    };
  } catch (error) {
    return { error: 'Something went wrong' };
  }
};
