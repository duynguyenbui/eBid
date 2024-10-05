'use server';

import {
  AuctionTypeSchema,
  CreateAuctionSchema,
  UploadFormSchema,
} from '@/types';
import axios from 'axios';
import * as z from 'zod';
import { getToken } from './token';

export const placeBid = async (auctionId: number, amount: number) => {
  const token = await getToken();

  if (!token) {
    return { error: 'Failed to get token' };
  }

  try {
    const requestId = await generateGUID();

    const response = await axios.post(
      `${process.env.BIDDING_API_URL}/api/bidding/placebid?api-version=${process.env.BIDDING_API_URL_VERSION}`,
      { auctionId: auctionId, amount: amount },
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
          'x-requestid': requestId,
        },
      }
    );

    console.log(response);

    if (response.status === 201) {
      return { success: 'Bid placed successfully' };
    }

    return {
      error: 'Failed to place bid',
    };
  } catch (error) {
    return { error: 'Something went wrong' };
  }
};

export const uploadImage = async (auctionId: string, form: FormData) => {
  const token = await getToken();
  if (!token) {
    return { error: 'Failed to get token' };
  }
  try {
    const response = await axios.post(
      `${process.env.AUCTION_API_URL}/api/auction/items/image/${auctionId}?api-version=${process.env.AUCTION_API_URL_VERSION}`,
      form,
      {
        headers: {
          'Content-Type': 'multipart/form-data',
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (response.status === 201) {
      return { success: 'Upload image successfully' };
    }

    return {
      error: response.data || 'Failed to upload image',
      status: response.status,
    };
  } catch (error) {
    if (axios.isAxiosError(error)) {
      return {
        error: error.response?.data || error.message,
        status: error.response?.status,
      };
    }
    return { error: 'Something went wrong', status: null };
  }
};

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

export const generateGUID = () => {
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = (Math.random() * 16) | 0,
      v = c == 'x' ? r : (r & 0x3) | 0x8;
    return v.toString(16);
  });
};
