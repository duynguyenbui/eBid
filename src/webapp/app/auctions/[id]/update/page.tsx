'use client';

import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Textarea } from '@/components/ui/textarea';
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { FaIntercom } from 'react-icons/fa';
import { Auction, AuctionType, UpdateAuctionSchema } from '@/types';
import { getAuctionById, getAuctionTypes } from '@/actions/get';
import { toast } from '@/hooks/use-toast';
import { updateAuction } from '@/actions/put';
import { ChangeToSell } from '@/components/change-to-sell';
import { TimePicker } from '@/components/ui/time-picker';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { cn } from '@/lib/utils';
import { CalendarIcon, Trash } from 'lucide-react';
import { format } from 'date-fns';
import { Calendar } from '@/components/ui/calendar';
import { ImageUploader } from '@/components/image-upload';
import { deleteItem } from '@/actions/delete';
import { useRouter } from 'next/navigation';

const DEFAULT_VALUE = {
  datetime: undefined,
};

export default function UpdatePage({ params }: { params: { id: string } }) {
  const { id } = params;
  const router = useRouter();
  const [auction, setAuction] = useState<Auction | null>(null);
  const [isOnSell, setIsOnSell] = useState<boolean>();
  const [auctionTypes, setAuctionTypes] = useState<AuctionType[]>([]);

  const form = useForm<z.infer<typeof UpdateAuctionSchema>>({
    resolver: zodResolver(UpdateAuctionSchema),
    defaultValues: {
      id: 0,
      name: '',
      description: '',
      auctionTypeId: '',
      startingPrice: '',
      endingTime: DEFAULT_VALUE.datetime,
    },
  });

  async function onSubmit(values: z.infer<typeof UpdateAuctionSchema>) {
    updateAuction(values).then((res) => {
      if (res.error) {
        toast({
          variant: 'destructive',
          title: 'Failed to update auction',
          description: res.error,
        });
      } else {
        toast({
          title: 'Auction updated successfully',
          description: 'Your auction has been successfully updated.',
        });
      }
    });
  }

  useEffect(() => {
    getAuctionTypes().then((types) => {
      setAuctionTypes(types);
    });
  }, []);

  useEffect(() => {
    getAuctionById(id)
      .then((res) => {
        if (res) {
          form.reset({
            id: res.id || 0,
            name: res.name || '',
            description: res.description || '',
            auctionTypeId: res.auctionTypeId.toString(),
            startingPrice: res.startingPrice.toString(),
            endingTime: new Date(res.endingTime),
          });

          if (res.onSell) {
            setIsOnSell(res.onSell);
          }
        }
      })
      .catch((err) => {
        console.error(err);
        setAuction(null);
      });
  }, [id, form]);

  if (!id) {
    return <div>No auction id found</div>;
  }

  return (
    <div className="container mx-auto py-10">
      <h1 className="text-3xl font-bold mb-6 flex items-center">
        <div className="flex">
          Modify
          <h4 className="text-sm text-muted-foreground">(v)</h4>
        </div>
        Your Item <FaIntercom className="ml-4 text-blue-600" />
      </h1>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <div className="space-y-5">
              <FormField
                control={form.control}
                name="name"
                disabled={isOnSell}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Item Title</FormLabel>
                    <FormControl>
                      <Input
                        placeholder="Enter the title of your item"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      Provide a clear and concise title for your auction item.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="description"
                disabled={isOnSell}
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Item Description</FormLabel>
                    <FormControl>
                      <Textarea
                        placeholder="Describe your item in detail"
                        className="resize-none"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      Include relevant details about the items features,
                      condition, and any unique characteristics.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />

              <FormField
                control={form.control}
                name="auctionTypeId"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Category</FormLabel>
                    <Select
                      value={field.value}
                      onValueChange={field.onChange}
                      disabled={isOnSell}
                    >
                      <FormControl>
                        <SelectTrigger>
                          <SelectValue placeholder="Select a category" />
                        </SelectTrigger>
                      </FormControl>
                      <SelectContent>
                        {auctionTypes.map((type) => (
                          <SelectItem key={type.id} value={type.id.toString()}>
                            {type.type}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    <FormDescription>
                      Choose the category that best fits your item.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                disabled={isOnSell}
                name="startingPrice"
                render={({ field }) => (
                  <FormItem>
                    <FormLabel>Starting Price</FormLabel>
                    <FormControl>
                      <Input
                        type="number"
                        placeholder="Enter starting price"
                        {...field}
                      />
                    </FormControl>
                    <FormDescription>
                      Set the initial bidding price for your item.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
              <FormField
                control={form.control}
                name="endingTime"
                render={({ field }) => (
                  <FormItem className="w-80">
                    <FormLabel>Ending time of your auction</FormLabel>
                    <Popover>
                      <FormControl>
                        <PopoverTrigger asChild disabled={isOnSell}>
                          <Button
                            variant="outline"
                            className={cn(
                              'w-[280px] justify-start text-left font-normal',
                              !field.value && 'text-muted-foreground'
                            )}
                          >
                            <CalendarIcon className="mr-2 h-4 w-4" />
                            {field.value ? (
                              format(field.value, 'PPP HH:mm:ss')
                            ) : (
                              <span>Pick a date</span>
                            )}
                          </Button>
                        </PopoverTrigger>
                      </FormControl>
                      <PopoverContent className="w-auto p-0">
                        <Calendar
                          mode="single"
                          selected={field.value}
                          onSelect={field.onChange}
                          initialFocus
                          disabled={(date) =>
                            date < new Date() || date < new Date('1900-01-01')
                          }
                        />
                        <div className="p-3 border-t border-border">
                          <TimePicker
                            setDate={field.onChange}
                            date={field.value}
                          />
                        </div>
                      </PopoverContent>
                    </Popover>
                    <FormDescription>
                      Choose how long your auction will run.
                    </FormDescription>
                    <FormMessage />
                  </FormItem>
                )}
              />
            </div>
            <Button
              type="submit"
              disabled={form.formState.isSubmitting}
              variant={'destructive'}
              className={cn('w-full', isOnSell && 'hidden')}
            >
              {form.formState.isSubmitting
                ? 'Submitting...'
                : 'Update for Auction'}
            </Button>
          </form>
        </Form>
        <div>
          <ChangeToSell auctionId={id} />
          <ImageUploader auctionId={id} />
        </div>
      </div>
    </div>
  );
}
