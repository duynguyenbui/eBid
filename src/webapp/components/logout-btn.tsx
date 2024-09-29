'use client';

import React from 'react';
import { Button } from './ui/button';
import { signOut } from 'next-auth/react';

const LogoutBtn = ({ className }: { className?: string }) => {
  return (
    <Button className={className} onClick={() => signOut()}>
      Logout
    </Button>
  );
};

export default LogoutBtn;
