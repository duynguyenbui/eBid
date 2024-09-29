import Listings from "@/components/listings";
import Link from "next/link";
import React from "react";
import { FaHistory } from "react-icons/fa";

const AuctionsPage = () => {
  return (
    <div className="w-full min-w-full">
      {/* Header Section */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Featured Auctions</h1>
        <Link
          href="/history"
          className="text-xl text-gray-600 hover:text-gray-900"
        >
          <FaHistory className="text-blue-600 w-6 h-6" />
        </Link>
      </div>

      {/* Auction Listings */}
      <Listings />
    </div>
  );
};

export default AuctionsPage;
