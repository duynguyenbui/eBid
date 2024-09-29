import * as jwt from 'jsonwebtoken';

export const extractSub = (token: string) => {
  const decoded = jwt.decode(token) as { sub: string };
  return decoded.sub;
};
