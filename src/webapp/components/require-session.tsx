'use client';

import { SignIn } from '@/actions/auth';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardContent,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import { signIn } from 'next-auth/react';

const RequireSession = () => {
  return (
    <Card className="w-[350px] mx-auto mt-10">
      <CardHeader>
        <CardTitle>No Active Session</CardTitle>
      </CardHeader>
      <CardContent>
        <p>
          You do not have an active session. Please log in to access this
          content.
        </p>
      </CardContent>
      <CardFooter>
        <Button
          onClick={() =>
            signIn(
              'duende-identity-server6',
              { callBackUrl: '/' },
              { prompt: 'none' }
            )
          }
          className="w-full"
          variant={'secondary'}
        >
          Log In
        </Button>
      </CardFooter>
    </Card>
  );
};

export default RequireSession;
