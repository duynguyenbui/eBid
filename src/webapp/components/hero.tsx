'use client';

import { ArrowRight } from 'lucide-react';

import { Button, buttonVariants } from '@/components/ui/button';
import Link from 'next/link';
import { cn } from '@/lib/utils';
import { SignIn } from '@/actions/auth';
import { useSession } from 'next-auth/react';

export const Hero = () => {
  const session = useSession();

  return (
    <section>
      <div className="container md:ml-20">
        <div className="grid items-center gap-8 lg:grid-cols-2">
          <div className="flex flex-col items-center py-32 text-center lg:mx-auto lg:items-start lg:px-0 lg:text-left">
            <h1 className="my-6 text-pretty text-4xl font-bold lg:text-6xl">
              Welcome to Our Bidding Platform eBid 🚀
            </h1>
            <p className="mb-8 max-w-xl text-muted-foreground lg:text-xl">
              Bidding platform for NFTs
            </p>
            <div className="flex w-full flex-col justify-center gap-2 sm:flex-row lg:justify-start">
              <Link
                href="/auctions"
                className={cn(buttonVariants(), 'w-full sm:w-auto')}
              >
                <ArrowRight className="mr-2 size-4" />
                Explore
              </Link>
              {session.status === 'unauthenticated' ? (
                <Button
                  variant="secondary"
                  className="w-full sm:w-auto bg-gradient-to-r from-cyan-500 to-blue-500 text-white"
                  onClick={() => SignIn()}
                >
                  Login
                </Button>
              ) : null}
            </div>
          </div>
          <div className="relative aspect-[3/4]">
            <div className="absolute inset-0 flex items-center justify-center">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                version="1.1"
                viewBox="0 0 800 800"
                className="size-full text-muted-foreground opacity-30"
              >
                <defs>
                  <radialGradient
                    id="grad1"
                    cx="50%"
                    cy="50%"
                    r="50%"
                    fx="50%"
                    fy="50%"
                  >
                    <stop
                      offset="0%"
                      style={{ stopColor: '#FFDEE9', stopOpacity: 1 }}
                    />
                    <stop
                      offset="100%"
                      style={{ stopColor: '#B5FFFC', stopOpacity: 1 }}
                    />
                  </radialGradient>
                </defs>
                {Array.from(Array(1500).keys()).map((dot, index, array) => {
                  const angle = 0.2 * index;
                  const scalar = 40 + index * (360 / array.length);
                  const x = Math.round(Math.cos(angle) * scalar);
                  const y = Math.round(Math.sin(angle) * scalar);
                  const radius = (3 * index) / array.length;

                  return (
                    <circle
                      key={index}
                      r={radius}
                      cx={400 + x}
                      cy={400 + y}
                      fill="url(#grad1)"
                      opacity={0.6 - Math.sin(angle) * 0.4}
                    >
                      <animate
                        attributeName="r"
                        values={`${radius};${radius + 2};${radius}`}
                        dur="3s"
                        repeatCount="indefinite"
                      />
                    </circle>
                  );
                })}
              </svg>
            </div>
            <div className="absolute left-[8%] top-[10%] flex aspect-[5/6] w-[38%] justify-center rounded-lg border border-border bg-accent hover:scale-105 transition-transform duration-300 shadow-lg bg-gradient-to-r from-blue-500 to-purple-500"></div>
            <div className="absolute right-[12%] top-[20%] flex aspect-square w-1/5 justify-center rounded-lg border border-border bg-accent hover:rotate-6 transition-transform duration-300 shadow-lg bg-gradient-to-r from-red-400 to-yellow-400"></div>
            <div className="absolute bottom-[24%] right-[24%] flex aspect-[5/6] w-[38%] justify-center rounded-lg border border-border bg-accent hover:scale-105 transition-transform duration-300 shadow-lg bg-gradient-to-r from-green-300 to-teal-300"></div>
          </div>
        </div>
      </div>
    </section>
  );
};
