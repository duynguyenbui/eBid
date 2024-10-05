import Image from 'next/image';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { Auction } from '@/types';
import { FaMoneyBill } from 'react-icons/fa';

export const AuctionCard: React.FC<Auction> = ({
  id,
  name,
  startingPrice,
  pictureUrl,
  onSell,
}) => {
  return (
    <Card key={id} className="flex flex-col h-full">
      <CardHeader className="p-0">
        <div className="aspect-square relative overflow-hidden rounded-t-lg">
          <Image
            src={pictureUrl ? pictureUrl : '/placeholder.webp'}
            alt={name}
            fill
            className="transition-transform duration-300 ease-in-out hover:scale-110 object-cover"
          />
        </div>
      </CardHeader>
      <CardContent className="flex-grow p-4">
        <CardTitle className="text-lg mb-2">{name}</CardTitle>
        <p className="text-muted-foreground">${startingPrice.toFixed(2)}</p>
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <Button className="w-full" disabled={!onSell}>
          <FaMoneyBill className="w-4 h-4 mr-2 text-green-600" />
          Bidding
        </Button>
      </CardFooter>
    </Card>
  );
};
