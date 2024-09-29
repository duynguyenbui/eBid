'use server';

import {
  Auction,
  AuctionType,
  PaginationItems,
  PaginationRequest,
} from '@/types';
import axios from 'axios';
import { getUser } from './user';

export const getAuctions = async (request: PaginationRequest) => {
  const { from, size } = request;

  const res = await axios
    .get(
      `${
        process.env.SEARCH_API_URL
      }/api/search/items/all?From=${from}&Size=${size}&api-version=${
        process.env.SEARCH_API_URL_VERSION ?? '1.0'
      }`
    )
    .then((res) => {
      const results = res.data as PaginationItems<Auction>;
      return results;
    })
    .catch((err) => {
      console.error(err);

      return null;
    });

  return res;
};

export const getAuctionById = async (id: string) => {
  const res = (await axios
    .get(
      `${process.env.SEARCH_API_URL}/api/search/items/by/id/${id}?api-version=${process.env.SEARCH_API_URL_VERSION}`
    )
    .then((res) => {
      if (res.status === 200) {
        return res.data;
      } else return null;
    })
    .catch((err) => {
      console.error(err);
      return null;
    })) as Auction;

  return res;
};

export const getAuctionIsOnSell = async (auctionId: string) => {
  const url = `${process.env.SEARCH_API_URL}/api/search/by/id/${auctionId}/onsell?api-version=${process.env.SEARCH_API_URL_VERSION}`;

  const res = await axios
    .get(url)
    .then((res) => {
      return res.data;
    })
    .catch((err) => {
      console.error(err);

      return false;
    });

  return res;
};

export const getAuctionsBySellerId = async (
  sub: string,
  paginationRequest: PaginationRequest
) => {
  const user = await getUser();

  if (!user) {
    return [];
  }

  const { from, size } = paginationRequest;

  const query = `${process.env.SEARCH_API_URL}/api/search/by/sub/${sub}?From=${from}&Size=${size}&api-version=${process.env.SEARCH_API_URL_VERSION}`;

  const res = await axios
    .get(query)
    .then((res) => {
      return res.data;
    })
    .catch((err) => {
      console.error(err);

      return [];
    });

  return res;
};

export const getAuctionTypes = async () => {
  const res: AuctionType[] = await axios
    .get(
      `${process.env.AUCTION_API_URL}/api/auction/auctiontypes?api-version=${process.env.AUCTION_API_URL_VERSION}`
    )
    .then((res) => {
      return res.data;
    })
    .catch((err) => {
      console.error(err);

      return [];
    });

  return res;
};
