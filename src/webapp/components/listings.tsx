'use client';

import { Auction } from '@/types';
import { AuctionCard } from './product-card';
import Link from 'next/link';
import { useEffect, useState } from 'react';
import { getAuctions } from '@/actions/get';
import { Card, CardContent } from '@/components/ui/card';
import { AlertCircle, Loader2 } from 'lucide-react';

export default function Listings({
  from = 0,
  size = 10,
}: {
  from?: number;
  size?: number;
}) {
  const [auctions, setAuctions] = useState<Auction[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    setLoading(true);
    getAuctions({ from, size }).then((results) => {
      if (results) {
        setAuctions(results.data);
      }
      setLoading(false);
    });
  }, [from, size]);

  if (loading) {
    return (
      <div className="text-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary mx-auto" />
        <p className="mt-2 text-sm text-muted-foreground">Loading...</p>
      </div>
    );
  }

  if (auctions.length === 0) {
    return (
      <Card className="w-full max-w-md mx-auto">
        <CardContent className="flex items-center justify-center p-6">
          <AlertCircle className="h-5 w-5 mr-2 text-muted-foreground" />
          <p className="text-muted-foreground">
            No auctions available at the moment.
          </p>
        </CardContent>
      </Card>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      {auctions.map((auction) => (
        <Link href={`/auctions/${auction.id}`} key={auction.id}>
          <AuctionCard {...auction} />
        </Link>
      ))}
    </div>
  );
}
