"use client";

import { useState } from "react";
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Clock, DollarSign } from "lucide-react";
import { FaPeopleCarry } from "react-icons/fa";

export default function BidForm() {
  const [currentBid, setCurrentBid] = useState(1000);
  const [bidAmount, setBidAmount] = useState("");
  const [timeRemaining, setTimeRemaining] = useState("2h 30m");
  const [error, setError] = useState("");

  const handleBidSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const newBid = parseFloat(bidAmount);
    if (isNaN(newBid) || newBid <= currentBid) {
      setError("Your bid must be higher than the current bid");
      return;
    }
    setCurrentBid(newBid);
    setBidAmount("");
    setError("");
    // Here you would typically send the bid to a server
    console.log(`New bid submitted: $${newBid}`);
  };

  return (
    <Card className="w-full max-w-md mx-auto">
      <CardHeader>
        <CardTitle className="text-2xl font-bold">
          Vintage Watch Auction
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        <div className="flex justify-between items-center">
          <div className="flex items-center">
            <DollarSign className="w-5 h-5 mr-2 text-green-600" />
            <span className="text-lg font-semibold">Current Bid:</span>
          </div>
          <h3 className="text-xl font-bold">${currentBid.toFixed(2)}</h3>
        </div>
        <div className="flex justify-between items-center">
          <div className="flex items-center">
            <FaPeopleCarry className="w-5 h-5 mr-2 text-purple-400" />
            <span className="text-lg font-semibold">Bidder:</span>
          </div>
          <h3 className="text-xl font-bold">Nguyen Bui</h3>
        </div>
        <div className="flex justify-between items-center">
          <div className="flex items-center">
            <Clock className="w-5 h-5 mr-2 text-blue-600" />
            <span className="text-lg font-semibold">Time Remaining:</span>
          </div>
          <span className="text-xl">{timeRemaining}</span>
        </div>
        <form onSubmit={handleBidSubmit} className="space-y-4">
          <div className="space-y-2">
            <Label htmlFor="bid-amount">Your Bid</Label>
            <div className="relative">
              <DollarSign className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-500" />
              <Input
                id="bid-amount"
                type="number"
                placeholder="Enter your bid"
                value={bidAmount}
                onChange={(e) => setBidAmount(e.target.value)}
                className="pl-10"
                step="0.01"
                min={currentBid + 0.01}
                required
              />
            </div>
          </div>
          {error && <p className="text-red-500 text-sm">{error}</p>}
          <Button type="submit" className="w-full">
            Place Bid
          </Button>
        </form>
      </CardContent>
      <CardFooter className="text-sm text-gray-500">
        By placing a bid, you agree to our terms and conditions.
      </CardFooter>
    </Card>
  );
}
