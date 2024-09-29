import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { SessionProvider } from "next-auth/react";
import { Footer } from "@/components/footer";
import Header from "@/components/header";
import { ThemeProvider } from "@/providers/theme-provider";
import AIAssistant from "@/components/ai-assistant";
import { Toaster } from "@/components/ui/toaster";
import { auth } from "@/auth";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "eBid",
  description: "Bidding platform for NFTs",
};

interface RootLayoutProps {
  children: React.ReactNode;
}

export default async function RootLayout({ children }: RootLayoutProps) {
  const session = await auth();

  return (
    <SessionProvider session={session}>
      <html lang="en">
        <body className={inter.className}>
          <Toaster />
          <ThemeProvider
            attribute="class"
            defaultTheme="system"
            enableSystem
            disableTransitionOnChange
          >
            <AIAssistant />
            <div className="flex flex-col min-h-screen">
              <Header />
              <main className="flex-1 px-3 sm:px-14 md:px-16 w-full">
                {children}
              </main>
              <Footer />
            </div>
          </ThemeProvider>
        </body>
      </html>
    </SessionProvider>
  );
}
