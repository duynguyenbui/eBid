'use client';

import { Switch } from '@/components/ui/switch';
import { Label } from '@/components/ui/label';
import { useEffect, useState } from 'react';
import { auctionStatusChangedToBid } from '@/actions/put';
import { getAuctionIsOnSell } from '@/actions/get';
import { toast } from '@/hooks/use-toast';
import { useRouter } from 'next/navigation';
import { Loader2 } from 'lucide-react';

export const ChangeToSell = ({ auctionId }: { auctionId: string }) => {
  const router = useRouter();
  const [onSell, setOnSell] = useState(false);
  const [loading, setLoading] = useState(true);

  const fetchAuctionById = async () => {
    try {
      const isOnSell = await getAuctionIsOnSell(auctionId);
      setOnSell(isOnSell);
    } catch (error) {
      toast({
        variant: 'destructive',
        title: 'Failed to fetch auction status',
      });
    } finally {
      setLoading(false);
    }
  };

  const changeAuctionStatusToSell = async () => {
    if (!auctionId || onSell) return;

    try {
      const res = await auctionStatusChangedToBid(auctionId);
      if (res.success) {
        setOnSell(true);
        toast({
          variant: 'default',
          title: 'Auction status changed to Sell',
        });
      } else {
        toast({
          variant: 'destructive',
          title: 'Failed to change auction status to Sell',
        });
      }
    } catch (error) {
      toast({
        variant: 'destructive',
        title: 'An error occurred',
      });
    } finally {
      router.refresh();
      router.push(`/auctions/${auctionId}`);
    }
  };

  useEffect(() => {
    fetchAuctionById();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [auctionId]);

  if (loading) {
    return (
      <div className="text-center">
        <Loader2 className="h-8 w-8 animate-spin text-primary mx-auto" />
        <p className="mt-2 text-sm text-muted-foreground">Loading...</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col items-start space-y-4 p-4 bg-background rounded-lg shadow">
      <div className="flex items-center space-x-2">
        <Switch
          id="sale-mode"
          disabled={onSell}
          checked={onSell}
          onCheckedChange={(checked) => {
            if (!onSell) changeAuctionStatusToSell();
          }}
        />
        <Label htmlFor="sale-mode">Item on Sale</Label>
      </div>
      <p className="text-sm text-muted-foreground">
        This item is currently{' '}
        {onSell ? (
          <span className="font-semibold text-green-500">on sale</span>
        ) : (
          <span className="font-semibold text-red-500">not on sale</span>
        )}
        .
      </p>
    </div>
  );
};
