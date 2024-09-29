'use server';

import { signIn } from '@/auth';

export async function SignIn() {
  return await signIn(
    'duende-identity-server6',
    { callBackUrl: '/' },
    { prompt: 'none' }
  );
}
