import NextAuth from 'next-auth';
import DuendeIdentityServer6 from 'next-auth/providers/duende-identity-server6';
import { extractSub } from './helpers/jwt';
import type { Provider } from 'next-auth/providers';

const providers: Provider[] = [
  DuendeIdentityServer6({
    id: 'idserver',
    name: 'Duende Identity Server 6',
    clientId: process.env.AUTH_DUENDE_IDENTITY_SERVER6_ID || 'webapp',
    clientSecret: process.env.AUTH_DUENDE_IDENTITY_SERVER6_SECRET || 'secret',
    issuer:
      process.env.AUTH_DUENDE_IDENTITY_SERVER6_ISSUER ||
      'http://localhost:5000',
    authorization: {
      params: { scope: 'openid profile offline_access auction' },
    },
  }),
];

export const providerMap = providers
  .map((provider) => {
    if (typeof provider === 'function') {
      const providerData = provider();
      return { id: providerData.id, name: providerData.name };
    } else {
      return { id: provider.id, name: provider.name };
    }
  })
  .filter((provider) => provider.id !== 'credentials');

export const { handlers, auth, signIn, signOut, unstable_update } = NextAuth({
  trustHost: true,
  session: {
    strategy: 'jwt',
  },
  providers,
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
