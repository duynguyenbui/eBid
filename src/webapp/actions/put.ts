'use server';

import { UpdateAuctionSchema } from '@/types';
import axios from 'axios';
import * as z from 'zod';
import { getToken } from './token';

export const updateAuction = async (
  values: z.infer<typeof UpdateAuctionSchema>
) => {
  try {
    const token = await getToken();

    if (!token) {
      return { error: 'Unauthorized' };
    }

    const safe = UpdateAuctionSchema.safeParse(values);

    if (!safe.success) {
      return { error: 'Invalid values', details: safe.error.errors };
    }

    const data = {
      id: safe.data.id,
      name: safe.data.name,
      description: safe.data.description,
      startingPrice: parseFloat(safe.data.startingPrice),
      endingTime: safe.data.endingTime,
      auctionTypeId: safe.data.auctionTypeId,
    };

    const url = `${process.env.AUCTION_API_URL}/api/auction/items?api-version=${process.env.AUCTION_API_URL_VERSION}`;

    const response = await axios.put(url, data, {
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
    });

    if (response.status === 201) {
      const location = response.headers['location'];
      return { success: 'Update auction successfully', location };
    } else {
      return { error: 'Failed to update auction', status: response.status };
    }
  } catch (error) {
    return { error: 'An error occurred while updating the auction' };
  }
};

export const auctionStatusChangedToBid = async (auctionId: string) => {
  try {
    const token = await getToken();

    if (!token) {
      return { error: 'Unauthorized' };
    }

    const url = `${process.env.AUCTION_API_URL}/api/auction/items/${auctionId}/state/to/sell?api-version=${process.env.AUCTION_API_URL_VERSION}`;

    const response = await axios.post(
      url,
      {},
      {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      }
    );

    if (response.status === 200) {
      return { success: 'Auction status changed to Bid' };
    } else {
      return {
        error: 'Failed to change auction status to Bid',
        status: response.status,
      };
    }
  } catch (error) {
    return { error: 'An error occurred while changing auction status to Bid' };
  }
};
