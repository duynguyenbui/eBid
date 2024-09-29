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
import { getAuctionIsOnSell } from '@/actions/get';

interface ImageUploaderProps {
  auctionId: string;
}

export const ImageUploader: React.FC<ImageUploaderProps> = ({ auctionId }) => {
  const [onSell, setOnSell] = React.useState(false);
  const [preview, setPreview] = React.useState<string | ArrayBuffer | null>('');

  const formSchema = z.object({
    image: z
      .instanceof(File)
      .refine((file) => file.size !== 0, 'Please upload an image'),
  });

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    mode: 'onBlur',
    defaultValues: {
      image: new File([''], 'filename'),
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
      disabled: onSell, // Disable dropzone when onSell is true
    });

  const onSubmit = (values: z.infer<typeof formSchema>) => {
    console.log(values);
    toast({
      variant: 'default',
      title: `Image uploaded successfully 🎉 ${values.image.name}`,
    });
  };

  useEffect(() => {
    getAuctionIsOnSell(auctionId).then((res) => {
      setOnSell(res);
    });
  }, [auctionId]);

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-2 mt-5 text-start"
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
                    onSell ? 'cursor-not-allowed opacity-50' : ''
                  }`}
                >
                  {preview && (
                    <img
                      src={preview as string}
                      alt="Uploaded image"
                      className="max-h-[400px] rounded-lg"
                    />
                  )}
                  <ImagePlus
                    className={`size-30 ${preview ? 'hidden' : 'block'}`}
                  />
                  <Input {...getInputProps()} type="file" disabled={onSell} />
                  {isDragActive ? (
                    <p>Drop the image!</p>
                  ) : (
                    <p>Click here or drag an image to upload it</p>
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
      </form>
    </Form>
  );
};
