import NextAuth from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';
import { extractSub } from './helpers/jwt';

export const { handlers, auth, signIn, signOut, unstable_update } = NextAuth({
  trustHost: true,
  session: {
    strategy: 'jwt',
  },
  providers: [
    DuendeIdentityServer6({
      id: 'idserver',
      clientId: process.env.AUTH_DUENDE_IDENTITY_SERVER6_ID || 'webapp',
      clientSecret: process.env.AUTH_DUENDE_IDENTITY_SERVER6_SECRET || 'secret',
      issuer:
        process.env.AUTH_DUENDE_IDENTITY_SERVER6_ISSUER ||
        'http://localhost:5000',
      authorization: {
        params: { scope: 'openid profile offline_access auction' },
      },
    }),
  ],
  callbacks: {
    async jwt({ token, account, user }) {
      if (account) {
        token.accessToken = account.access_token;
      }

      return token;
    },
    async session({ session, token }) {
      session.accessToken = token.accessToken as string;

      if (token?.accessToken) {
        const identityUser = extractSub(token.accessToken as string);

        if (identityUser) {
          session.user = {
            ...session.user,
            id: identityUser,
          };
        }
      }

      return session;
    },
  },
});
