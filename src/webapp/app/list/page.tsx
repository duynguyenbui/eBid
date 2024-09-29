import { columns } from './columns';
import { Auction, PaginationItems } from '@/types';
import { DataTable } from './data-table';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Download } from 'lucide-react';
import { getAuctionsBySellerId } from '@/actions/get';
import { getUser } from '@/actions/user';
import { FaMoneyBill } from 'react-icons/fa';

export default async function ListPage() {
  const user = await getUser();

  if (!user) {
    return null;
  }

  const result: PaginationItems<Auction> = await getAuctionsBySellerId(
    user.id!,
    {
      from: 0,
      size: 10,
    }
  );

  return (
    <div className="min-h-screen bg-gradient-to-b ">
      <div className="container mx-auto py-10 px-4 sm:px-6 lg:px-8">
        <Card className="shadow-lg">
          <CardHeader className="space-y-1">
            <div className="flex items-center justify-between">
              <CardTitle className="text-2xl font-bold">
                Auctions Dashboard
              </CardTitle>
            </div>
            <CardDescription>Manage and monitor your auction</CardDescription>
          </CardHeader>
          <CardContent>
            <div className="mb-4 flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
              <Button disabled className='bg-green-400'>
                <FaMoneyBill className="mr-2 h-4 w-4" />
                Auctions
              </Button>
            </div>
            <div className="rounded-md">
              <DataTable columns={columns} data={result.data} />
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
