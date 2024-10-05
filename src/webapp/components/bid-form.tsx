'use client';

import { useEffect, useState } from 'react';
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Label } from '@/components/ui/label';
import { AlertCircle, Clock, DollarSign, Loader2 } from 'lucide-react';
import Countdown from 'react-countdown';
import { ScrollArea } from '@/components/ui/scroll-area';
import { toast } from '@/hooks/use-toast';
import { getAuctionById, getBids } from '@/actions/get';
import { placeBid } from '@/actions/post';
import { Auction, Bid } from '@/types';
import { useSession } from 'next-auth/react';

type ToastVariant = 'default' | 'destructive';

const useAuctionData = (auctionId: string) => {
  const [auction, setAuction] = useState<Auction | null>(null);
  const [bids, setBids] = useState<Bid[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAuctionData = async () => {
      try {
        const [auctionData, bidsData] = await Promise.all([
          getAuctionById(auctionId),
          getBids(auctionId),
        ]);
        setAuction(auctionData);
        setBids(bidsData);
      } catch (error) {
        toast({
          description: 'Failed to fetch auction details or bids.',
          variant: 'destructive',
        });
      } finally {
        setLoading(false);
      }
    };

    if (auctionId) {
      fetchAuctionData();
    }
  }, [auctionId]);

  return { auction, bids, loading, setBids };
};

const BidForm = ({ auctionId }: { auctionId: string }) => {
  const { data: session } = useSession();
  const { auction, bids, loading, setBids } = useAuctionData(auctionId);
  const [bidAmount, setBidAmount] = useState<string>('');

  const currentBid =
    bids.length > 0
      ? Math.max(...bids.map((bid) => bid.amount))
      : auction?.startingPrice ?? 0;

  const handleBidSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (bidAmount === '' || !auctionId) {
      showToast(
        'Please enter a bid amount and ensure auction ID is available.',
        'destructive'
      );
      return;
    }
    const bidValue = parseFloat(bidAmount);
    if (isNaN(bidValue) || bidValue <= currentBid) {
      showToast(
        'Please enter a valid bid amount higher than the current bid.',
        'destructive'
      );
      return;
    }

    if (session?.user?.id === auction?.sellerId) {
      showToast('You cannot bid on your own auction.', 'destructive');
      return;
    }

    try {
      const res = await placeBid(parseInt(auctionId), bidValue);

      if ('error' in res) {
        showToast(res.error!, 'destructive');
      } else {
        showToast(res.success, 'default');
        const updatedBids = await getBids(auctionId);
        setBids(updatedBids);
        setBidAmount('');
      }
    } catch (error) {
      showToast('An error occurred while placing your bid.', 'destructive');
    }
  };

  const showToast = (message: string, variant: ToastVariant) => {
    toast({
      description: message,
      variant: variant,
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <Loader2 className="h-8 w-8 animate-spin text-primary mx-auto" />
          <p className="mt-2 text-sm text-muted-foreground">Loading...</p>
        </div>
      </div>
    );
  }

  if (!auctionId || !auction) {
    return (
      <div className="flex items-center justify-center h-screen">
        <div className="text-center">
          <AlertCircle className="h-8 w-8 text-red-600 mx-auto" />
          <p className="mt-2 text-sm text-muted-foreground">
            Auction not found.
          </p>
        </div>
      </div>
    );
  }

  return (
    <Card className="w-full max-w-4xl mx-auto h-full">
      <CardHeader>
        <CardTitle className="text-2xl font-bold">{auction.name}</CardTitle>
      </CardHeader>
      <CardContent className="flex flex-col md:flex-row gap-4">
        <div className="w-full md:w-1/2 space-y-4">
          <AuctionDetails auction={auction} currentBid={currentBid} />
          <BidInputForm
            onSell={auction.onSell}
            bidAmount={bidAmount}
            setBidAmount={setBidAmount}
            handleBidSubmit={handleBidSubmit}
            currentBid={currentBid}
          />
        </div>
        <div className="w-full md:w-1/2">
          <PreviousBids bids={bids} />
        </div>
      </CardContent>
      <CardFooter className="text-sm text-muted-foreground">
        By placing a bid, you agree to our terms and conditions.
      </CardFooter>
    </Card>
  );
};

const AuctionDetails = ({
  auction,
  currentBid,
}: {
  auction: Auction;
  currentBid: number;
}) => (
  <>
    <div className="flex justify-between items-center">
      <div className="flex items-center">
        <DollarSign className="w-5 h-5 mr-2 text-green-600" />
        <span className="text-lg font-semibold">Current Bid:</span>
      </div>
      <h3 className="text-xl font-bold">${currentBid.toFixed(2)}</h3>
    </div>
    <div className="flex justify-between items-center">
      <div className="flex items-center">
        <Clock className="w-5 h-5 mr-2 text-blue-600" />
        <span className="text-lg font-semibold">Time Remaining:</span>
      </div>
      <Countdown
        className="text-blue-500 font-bold"
        date={new Date(auction.endingTime)}
      />
    </div>
  </>
);

const BidInputForm = ({
  bidAmount,
  setBidAmount,
  handleBidSubmit,
  currentBid,
  onSell,
}: {
  bidAmount: string;
  setBidAmount: React.Dispatch<React.SetStateAction<string>>;
  handleBidSubmit: (e: React.FormEvent<HTMLFormElement>) => void;
  currentBid: number;
  onSell: boolean;
}) => (
  <form onSubmit={handleBidSubmit} className="space-y-3">
    <div className="space-y-2">
      <Label htmlFor="bid-amount">Your Bid</Label>
      <div className="relative">
        <DollarSign className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500" />
        <Input
          disabled={!onSell}
          id="bid-amount"
          type="number"
          placeholder="Enter your bid"
          value={bidAmount}
          onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
            setBidAmount(e.target.value)
          }
          className="pl-10"
          step="0.01"
          min={currentBid + 0.01}
          required
        />
      </div>
    </div>
    <Button type="submit" className="w-full" disabled={!onSell}>
      Place Bid
    </Button>
  </form>
);

const PreviousBids = ({ bids }: { bids: Bid[] }) => (
  <>
    <h4 className="text-lg font-semibold mb-2">Previous Bids</h4>
    {bids.length > 0 ? (
      <ScrollArea className="h-[300px] w-full rounded-md border p-1">
        <ul className="space-y-2">
          {bids.map((bid) => (
            <li
              key={bid.id}
              className="p-2 rounded flex justify-between items-center bg-secondary"
            >
              <span>
                <span className="font-bold">${bid.amount.toFixed(2)}</span>{' '}
              </span>
              <span className="text-sm text-muted-foreground">
                {new Date(bid.bidTime).toLocaleTimeString()}
              </span>
            </li>
          ))}
        </ul>
      </ScrollArea>
    ) : (
      <p className="text-muted-foreground">No bids yet. Be the first to bid!</p>
    )}
  </>
);

export default BidForm;
