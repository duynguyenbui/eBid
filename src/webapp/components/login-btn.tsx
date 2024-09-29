'use client';

import { signIn } from 'next-auth/react';
import { Button } from './ui/button';

const LoginBtn = ({ className }: { className?: string }) => {
  return (
    <Button
      className={className}
      onClick={() => {
        signIn(
          'duende-identity-server6',
          { callBackUrl: '/' },
          { prompt: 'none' }
        );
      }}
    >
      Login
    </Button>
  );
};

export default LoginBtn;
