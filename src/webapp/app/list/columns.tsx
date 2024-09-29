'use client';

import { ColumnDef } from '@tanstack/react-table';
import { ArrowUpDown } from 'lucide-react';
import { Auction } from '@/types';
import { MoreHorizontal } from 'lucide-react';
import { Button } from '@/components/ui/button';
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import Link from 'next/link';

export const columns: ColumnDef<Auction>[] = [
  {
    accessorKey: 'id',
    header: 'ID',
  },
  {
    accessorKey: 'name',
    header: ({ column }) => {
      return (
        <Button
          variant="ghost"
          onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
        >
          Name
          <ArrowUpDown className="ml-2 h-4 w-4" />
        </Button>
      );
    },
  },
  {
    accessorKey: 'startingPrice',
    header: 'Starting Price',
    cell: ({ row }) => {
      const amount = parseFloat(row.getValue('startingPrice'));
      const formatted = new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: 'USD',
      }).format(amount);

      return <div className="font-medium">{formatted}</div>;
    },
  },
  {
    accessorKey: 'soldAmount',
    header: 'Sold Amount',
  },
  {
    accessorKey: 'endingTime',
    header: 'Ending Time',
    cell: ({ row }) => {
      const endingTime = row.getValue('endingTime');
      const date = new Date(endingTime as string | number | Date);

      return <div>{date.toLocaleString()}</div>;
    },
  },
  {
    accessorKey: 'status',
    header: 'Status',
  },
  {
    accessorKey: 'onSell',
    header: ({ column }) => {
      return (
        <div className="text-center">
          <Button
            variant="ghost"
            onClick={() => column.toggleSorting(column.getIsSorted() === 'asc')}
          >
            On Sell
            <ArrowUpDown className="ml-2 h-4 w-4" />
          </Button>
        </div>
      );
    },
    cell: ({ row }) => {
      const onSell = row.getValue('onSell');

      return <div className="text-center">{onSell ? 'Yes' : 'No'}</div>;
    },
  },
  {
    id: 'actions',
    cell: ({ row }) => {
      const auction = row.original;

      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="h-8 w-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="h-4 w-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() =>
                navigator.clipboard.writeText(
                  `/auctions/${auction?.id ? auction.id.toString() : ''}`
                )
              }
            >
              Copy auction ID
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <Link href={`/auctions/${auction.id}/update`}>
              <DropdownMenuItem>View Details</DropdownMenuItem>
            </Link>
          </DropdownMenuContent>
        </DropdownMenu>
      );
    },
  },
];
