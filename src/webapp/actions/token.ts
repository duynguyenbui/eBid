"use server";

import { auth } from "@/auth";

export const getToken = async () => {
  const session = await auth();

  if (!session) {
    return null;
  }

  return session.accessToken ?? "";
};
