'use client';

import { useEffect, useState } from 'react';
import Image from 'next/image';
import { Separator } from '@/components/ui/separator';
import { Card, CardContent } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Clock, DollarSign, Info } from 'lucide-react';
import BidForm from '@/components/bid-form';
import { Auction } from '@/types';
import { getAuctionById } from '@/actions/get';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

export default function Component({ params }: { params: { id: string } }) {
  const session = useSession();
  const [auction, setAuction] = useState<Auction | null>(null);
  const [isClient, setIsClient] = useState(false);

  useEffect(() => {
    getAuctionById(params.id)
      .then((res) => {
        setAuction(res);
      })
      .catch((err) => {
        setAuction(null);
      });
  }, [params.id]);

  useEffect(() => {
    setIsClient(true);
  }, []);

  if (!isClient) {
    return null;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <Card className="overflow-hidden">
        <div className="grid lg:grid-cols-2 gap-8">
          <div className="relative aspect-square">
            <Image
              fill
              src={`/products/${params.id}.webp`}
              alt={auction?.name || 'Product Image'}
              className="object-cover"
            />
          </div>
          <CardContent className="p-6 flex flex-col justify-between">
            <div className="space-y-4">
              <div className="flex justify-between items-start">
                <div className="flex space-x-1">
                  <div>
                    <h1 className="text-3xl font-bold">{auction?.name}</h1>
                    <Badge variant="secondary" className="text-sm">
                      {auction?.auctionType}
                    </Badge>
                  </div>
                  <div className="mt-1">
                    {session.data?.user ? (
                      <Link href={`/auctions/${params.id}/update`}>
                        <Badge variant={'outline'}>Details</Badge>
                      </Link>
                    ) : null}
                  </div>
                </div>
                <Badge variant="secondary" className="text-sm">
                  Auction #{params.id}
                </Badge>
              </div>
              <p className="text-muted-foreground">{auction?.description}</p>
              <div className="flex items-center space-x-2 text-2xl font-semibold">
                <DollarSign className="w-6 h-6 text-green-500" />
                <span>Starting Price: </span>
                <span className="text-green-500">
                  {new Intl.NumberFormat('en-US', {
                    style: 'currency',
                    currency: 'USD',
                  }).format(auction?.startingPrice || 0)}
                </span>
              </div>
              <div className="flex items-center space-x-2 text-sm">
                <Clock className="w-4 h-4 text-blue-500" />
                <span>Ending: </span>
                <span className="font-semibold">
                  {new Date(auction?.endingTime as string).toLocaleString()}
                </span>
              </div>
            </div>
            <Separator className="my-6" />
            <div className="space-y-4">
              <h2 className="text-xl font-semibold">Place Your Bid</h2>
              <BidForm />
            </div>
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
{
  /* <div className="mt-8">
        <h2 className="text-2xl font-bold mb-4">Auction Details</h2>
        <Card>
          <CardContent className="p-6">
            <div className="grid gap-4">
              <div className="flex items-center space-x-2">
                <Info className="w-5 h-5 text-blue-500" />
                <span className="font-semibold">Condition:</span>
                <span>New</span>
              </div>
              <div className="flex items-center space-x-2">
                <Info className="w-5 h-5 text-blue-500" />
                <span className="font-semibold">Shipping:</span>
                <span>Free Shipping</span>
              </div>
              <div className="flex items-center space-x-2">
                <Info className="w-5 h-5 text-blue-500" />
                <span className="font-semibold">Returns:</span>
                <span>30 Day Returns</span>
              </div>
            </div>
          </CardContent>
        </Card>
      </div> */
}
