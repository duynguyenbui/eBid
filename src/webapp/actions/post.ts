'use server';

import { AuctionTypeSchema, CreateAuctionSchema } from '@/types';
import axios from 'axios';
import * as z from 'zod';
import { getToken } from './token';

export const createAuction = async (
  data: z.infer<typeof CreateAuctionSchema>
) => {
  const safe = CreateAuctionSchema.safeParse(data);

  if (!safe.success) {
    return { error: 'Invalid auction data', issues: safe.error.issues };
  }

  const auctionToCreate = {
    name: data.name,
    description: data.description,
    startingPrice: parseFloat(data.startingPrice),
    auctionTypeId: parseInt(data.auctionTypeId),
    endingTime: data.endingTime,
  };

  const token = await getToken();
  if (!token) {
    return { error: 'Failed to get token' };
  }

  try {
    const response = await axios.post(
      `${process.env.AUCTION_API_URL}/api/auction/items?api-version=${process.env.AUCTION_API_URL_VERSION}`,
      auctionToCreate,
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (response.status === 201) {
      const location = response.headers['location'];
      return { success: true, location };
    } else {
      return { error: 'Failed to create auction', status: response.status };
    }
  } catch (error) {
    if (axios.isAxiosError(error)) {
      return { error: error.message, details: error.response?.data };
    }
    return { error: 'Something went wrong', details: error };
  }
};

export const createAuctionType = async (
  data: z.infer<typeof AuctionTypeSchema>
) => {
  const token = await getToken();

  if (!token) {
    return { error: 'Failed to get token' };
  }

  const safe = AuctionTypeSchema.safeParse(data);

  if (!safe.success) {
    return { error: 'Invalid auction type data', issues: safe.error.issues };
  }

  try {
    const response = await axios.post(
      `${process.env.AUCTION_API_URL}/api/auction/items/type?api-version=${process.env.AUCTION_API_URL_VERSION}`,
      data,
      {
        headers: {
          accept: '*/*',
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (response.status === 201) {
      const location = response.headers['location'];
      return { success: true, location };
    } else {
      return {
        error: 'Failed to create auction type',
        status: response.status,
      };
    }
  } catch (error) {
    console.error('Error occurred:', error);
    return { error: 'Request failed', details: error };
  }
};
