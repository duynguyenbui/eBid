'use client';

import React, { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useDropzone } from 'react-dropzone';
import { z } from 'zod';
import { Input } from './ui/input';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from './ui/form';
import { ImagePlus } from 'lucide-react';
import { toast } from '@/hooks/use-toast';
import { getAuctionById } from '@/actions/get';
import { Button } from './ui/button';
import { uploadImage } from '@/actions/post';
import { Auction, UploadFormSchema } from '@/types';

interface ImageUploaderProps {
  auctionId: string;
}

export const ImageUploader: React.FC<ImageUploaderProps> = ({ auctionId }) => {
  const [auction, setAuction] = React.useState<Auction | null>(null);
  const [preview, setPreview] = React.useState<string | ArrayBuffer | null>('');

  const form = useForm<z.infer<typeof UploadFormSchema>>({
    resolver: zodResolver(UploadFormSchema),
    mode: 'onBlur',
    defaultValues: {
      image: new File([''], 'filename'),
      id: auctionId,
    },
  });

  const onDrop = React.useCallback(
    (acceptedFiles: File[]) => {
      const reader = new FileReader();
      try {
        reader.onload = () => setPreview(reader.result);
        reader.readAsDataURL(acceptedFiles[0]);
        form.setValue('image', acceptedFiles[0]);
        form.clearErrors('image');
      } catch (error) {
        setPreview(null);
        form.resetField('image');
      }
    },
    [form]
  );

  const { getRootProps, getInputProps, isDragActive, fileRejections } =
    useDropzone({
      onDrop,
      maxFiles: 1,
      maxSize: 1000000,
      accept: { 'image/png': [], 'image/jpg': [], 'image/jpeg': [] },
      disabled: auction?.onSell,
    });

  const onSubmit = async (values: z.infer<typeof UploadFormSchema>) => {
    try {
      const formData = new FormData();
      formData.append('image', values.image);

      const response = await uploadImage(values.id, formData);

      if (response?.error) {
        toast({
          title: 'Failed to upload image',
          description: response.error,
        });
        return;
      }

      toast({
        title: 'Upload image successfully',
        description: 'Image has been uploaded',
      });
    } catch (error) {
      toast({
        title: 'Failed to upload image',
        description: 'Something went wrong',
      });
    }
  };

  useEffect(() => {
    getAuctionById(auctionId).then((res) => {
      setAuction(res);
    });
  }, [auctionId]);

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-2 mt-5 flex flex-col items-center"
      >
        <FormField
          control={form.control}
          name="image"
          render={() => (
            <FormItem className="mx-auto md:w-1/2">
              <FormLabel
                className={`${
                  fileRejections.length !== 0 && 'text-destructive'
                }`}
              >
                Review
                <span
                  className={
                    form.formState.errors.image || fileRejections.length !== 0
                      ? 'text-destructive'
                      : 'text-muted-foreground'
                  }
                ></span>
              </FormLabel>
              <FormControl>
                <div
                  {...getRootProps()}
                  className={`mx-auto flex cursor-pointer flex-col items-center justify-center gap-y-2 rounded-lg border border-foreground p-8 shadow-sm shadow-foreground ${
                    auction?.onSell ? 'cursor-not-allowed opacity-50' : ''
                  }`}
                >
                  {preview ? (
                    // eslint-disable-next-line @next/next/no-img-element
                    <img
                      src={preview as string}
                      alt="Uploaded image"
                      className="max-h-[400px] rounded-lg"
                    />
                  ) : (
                    auction?.pictureUrl && (
                      // eslint-disable-next-line @next/next/no-img-element
                      <img
                        src={auction.pictureUrl}
                        alt="Auction image"
                        className="max-h-[400px] rounded-lg"
                      />
                    )
                  )}
                  <ImagePlus
                    className={`size-30 ${
                      preview || auction?.pictureUrl ? 'hidden' : 'block'
                    }`}
                  />
                  <Input
                    {...getInputProps()}
                    type="file"
                    disabled={auction?.onSell}
                  />
                  {isDragActive ? (
                    <p>Drop the image!</p>
                  ) : (
                    form.getValues('image') === null && (
                      <p>Click here or drag an image to upload it</p>
                    )
                  )}
                </div>
              </FormControl>
              <FormMessage>
                {fileRejections.length !== 0 && (
                  <p>
                    Image must be less than 1MB and of type png, jpg, or jpeg
                  </p>
                )}
              </FormMessage>
            </FormItem>
          )}
        />
        <Button
          type="submit"
          disabled={
            auction?.onSell ||
            form.getValues('image') === null ||
            form.formState.isSubmitting
          }
        >
          Upload
        </Button>
      </form>
    </Form>
  );
};
