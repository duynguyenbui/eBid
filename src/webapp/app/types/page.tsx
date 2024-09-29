"use client";

import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Gavel } from "lucide-react";
import { AuctionType, AuctionTypeSchema } from "@/types";
import { getAuctionTypes } from "@/actions/get";
import { createAuctionType } from "@/actions/post";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

import { toast } from "@/hooks/use-toast";
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { useSession } from "next-auth/react";
import { useRouter } from "next/router";

export default function AuctionTypesPage() {
  const session = useSession();

  const form = useForm<z.infer<typeof AuctionTypeSchema>>({
    resolver: zodResolver(AuctionTypeSchema),
    defaultValues: {
      type: "",
    },
  });

  const [auctionTypes, setAuctionTypes] = useState<AuctionType[]>([]);

  useEffect(() => {
    getAuctionTypes().then((res) => {
      setAuctionTypes(res);
    });
  }, [form.formState.isSubmitSuccessful, session.status]);

  async function onSubmit(data: z.infer<typeof AuctionTypeSchema>) {
    try {
      const res = await createAuctionType(data);
      toast({
        title: "Type Created Successfully",
        description: (
          <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
            <h2 className="text-white">{res.location}</h2>
          </pre>
        ),
      });
      form.reset();
    } catch (error) {
      toast({
        variant: "destructive",
        title: "Uh oh! Something went wrong.",
        description: "There was a problem with your request.",
      });
    }
  }

  return (
    <div className="container mx-auto p-4">
      <h1 className="text-3xl font-bold mb-6">Auction Types</h1>
      {session.status === ("authenticated" || "loading") && (
        <Form {...form}>
          <form
            onSubmit={form.handleSubmit(onSubmit)}
            className="w-2/3 space-y-6"
          >
            <FormField
              control={form.control}
              name="type"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>New Type:</FormLabel>
                  <FormControl>
                    <Input placeholder="shadcn" {...field} />
                  </FormControl>
                  <FormDescription>This is a public type name.</FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />
            <Button type="submit">Submit</Button>
          </form>
        </Form>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 mb-8 mt-8">
        {auctionTypes.map((type) => (
          <Card key={type.id}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">{type.type}</CardTitle>
              <Gavel className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <p className="text-xs text-muted-foreground">
                Click to learn more about this auction type
              </p>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
}
