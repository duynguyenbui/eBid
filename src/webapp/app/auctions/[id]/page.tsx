'use client';

import { useEffect, useState } from 'react';
import Image from 'next/image';
import { Separator } from '@/components/ui/separator';
import { Card, CardContent } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Clock, DollarSign } from 'lucide-react';
import BidForm from '@/components/bid-form';
import { Auction } from '@/types';
import { getAuctionById } from '@/actions/get';
import { useSession } from 'next-auth/react';
import Link from 'next/link';

export default function UpdatePage({ params }: { params: { id: string } }) {
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
    <div className="container mx-auto">
      <Card className="overflow-hidden">
        <div className="grid lg:grid-cols-2">
          <div className="relative aspect-square">
            <Image
              fill
              src={auction?.pictureUrl || '/placeholder.webp'}
              alt={auction?.name || 'Product Image'}
              className="object-cover -mb-2"
            />
          </div>
          <CardContent className="p-3 flex flex-col justify-between">
            <div className="space-y-4 pt-5 pl-5 pr-5">
              <div className="flex justify-between items-start">
                <div className="flex space-x-1">
                  <div>
                    <h1 className="text-3xl font-bold mb-2">{auction?.name}</h1>
                    <Badge variant="outline" className="text-sm">
                      {auction?.auctionType}
                    </Badge>
                    <Badge
                      variant="default"
                      className="text-sm ml-2 bg-green-500"
                    >
                      {auction?.status}
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
            <BidForm
              auctionId={auction?.id.toString()!}
            />
          </CardContent>
        </div>
      </Card>
    </div>
  );
}
