import { z } from 'zod';
/* ===============================TYPES OBJECTS=================================== */

export interface PaginationRequest {
  from: number;
  size: number;
}

export interface PaginationItems<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T[];
}

export interface Auction {
  id: number;
  name: string;
  description: string;
  pictureUrl?: string;
  sellerId: string;
  winnerId?: string;
  soldAmount?: string;
  startingPrice: number;
  onSell: boolean;
  createdAt: string;
  updatedAt: string;
  endingTime: string;
  status: string;
  auctionTypeId: number;
  auctionType: string;
}

export interface AuctionType {
  id: number;
  type: string;
}

/* ===============================ZOD OBJECTS=================================== */
export const LoginSchema = z.object({
  email: z.string().email({
    message: 'Email is required',
  }),
  password: z.string().min(1, {
    message: 'Password is required',
  }),
  code: z.optional(z.string()),
});

export const CreateAuctionSchema = z.object({
  name: z.string().min(3, {
    message: 'Title must be at least 3 characters.',
  }),
  description: z.string().min(10, {
    message: 'Description must be at least 10 characters.',
  }),
  auctionTypeId: z.string({
    required_error: 'Please select a type of auctions.',
  }),
  startingPrice: z
    .string()
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Starting price must be a positive number.',
    }),
  endingTime: z
    .date({
      required_error: 'A date of ending is required.',
    })
    .optional(),
});

export const UpdateAuctionSchema = z.object({
  id: z.number(),
  name: z.string().min(3, {
    message: 'Title must be at least 3 characters.',
  }),
  description: z.string().min(10, {
    message: 'Description must be at least 10 characters.',
  }),
  auctionTypeId: z.string({
    required_error: 'Please select a type of auctions.',
  }),
  startingPrice: z
    .string()
    .refine((val) => !isNaN(Number(val)) && Number(val) > 0, {
      message: 'Starting price must be a positive number.',
    }),
  endingTime: z
    .date({
      required_error: 'Please select an auction duration.',
    })
    .optional(),
  // image: z.instanceof(File).optional(),
});

export const AuctionTypeSchema = z.object({
  type: z.string().min(2).max(50),
});
