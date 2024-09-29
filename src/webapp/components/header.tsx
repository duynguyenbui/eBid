/**
 * v0 by Vercel.
 * @see https://v0.dev/t/lJwnQlHSEBA
 * Documentation: https://v0.dev/docs#integrating-generated-code-into-your-nextjs-app
 */
import { Sheet, SheetTrigger, SheetContent } from '@/components/ui/sheet';
import { Button, buttonVariants } from '@/components/ui/button';
import Link from 'next/link';
import { ModeToggle } from './mode-toggle';
import Image from 'next/image';
import { cn } from '@/lib/utils';
import { getUser } from '@/actions/user';
import { signOut } from '@/auth';
import LogoutBtn from './logout-btn';
import LoginBtn from './login-btn';

export default async function Header() {
  const user = await getUser();

  return (
    <header className="flex h-20 w-full shrink-0 items-center px-4 md:px-6 mb-2">
      <Sheet>
        <SheetTrigger asChild>
          <Button variant="outline" size="icon" className="lg:hidden">
            <MenuIcon className="h-6 w-6" />
            <span className="sr-only">Toggle navigation menu</span>
          </Button>
        </SheetTrigger>
        <SheetContent side="left">
          <Link href="/" className="mr-6 hidden lg:flex" prefetch={false}>
            <Image
              src={'/logo-header.svg'}
              width={40}
              height={40}
              alt="logo-header"
            />
          </Link>
          <div className="grid gap-2 py-6">
            <Link
              href="/"
              className="flex w-full items-center py-2 text-lg font-semibold"
              prefetch={false}
            >
              Home
            </Link>
            <Link
              href="/auctions"
              className="flex w-full items-center py-2 text-lg font-semibold"
              prefetch={false}
            >
              Auctions
            </Link>
            <Link
              href="/types"
              className="flex w-full items-center py-2 text-lg font-semibold"
              prefetch={false}
            >
              Types
            </Link>
            <Link
              href="/sell"
              className="flex w-full items-center py-2 text-lg font-semibold"
              prefetch={false}
            >
              Sell
            </Link>
            {user ? (
              <>
                <Link
                  href="/list"
                  className="flex w-full items-center py-2 text-lg font-semibold"
                  prefetch={false}
                >
                  List
                </Link>
                <Link
                  href="/profile"
                  className="flex w-full items-center py-2 text-lg font-semibold"
                  prefetch={false}
                >
                  Profile
                </Link>
                <LogoutBtn className="flex w-full items-center py-2 text-lg font-semibold" />
              </>
            ) : (
              <LoginBtn className="flex w-full items-center py-2 text-lg font-semibold bg-gradient-to-r from-cyan-500 to-blue-500" />
            )}
          </div>
        </SheetContent>
      </Sheet>
      <Link href="/" className="mr-6 hidden lg:flex" prefetch={false}>
        <Image
          src={'/logo-header.svg'}
          width={40}
          height={40}
          alt="logo-header"
        />
        <span className="sr-only">Acme Inc</span>
      </Link>
      <nav className="ml-auto hidden lg:flex gap-6">
        <Link
          href="/"
          className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
          prefetch={false}
        >
          Home
        </Link>
        <Link
          href="/auctions"
          className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
          prefetch={false}
        >
          Auctions
        </Link>
        <Link
          href="/types"
          className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
          prefetch={false}
        >
          Types
        </Link>
        <Link
          href="/sell"
          className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
          prefetch={false}
        >
          Sell
        </Link>
        {user !== null ? (
          <>
            <Link
              href="/list"
              className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
              prefetch={false}
            >
              List
            </Link>
            <Link
              href="/profile"
              className="group inline-flex h-9 w-max items-center justify-center rounded-md bg-white px-4 py-2 text-sm font-medium transition-colors hover:bg-gray-100 hover:text-gray-900 focus:bg-gray-100 focus:text-gray-900 focus:outline-none disabled:pointer-events-none disabled:opacity-50 data-[active]:bg-gray-100/50 data-[state=open]:bg-gray-100/50 dark:bg-gray-950 dark:hover:bg-gray-800 dark:hover:text-gray-50 dark:focus:bg-gray-800 dark:focus:text-gray-50 dark:data-[active]:bg-gray-800/50 dark:data-[state=open]:bg-gray-800/50"
              prefetch={false}
            >
              Profile
            </Link>
            <LogoutBtn
              className={cn(
                buttonVariants({ variant: 'destructive' }),
                'group inline-flex h-9 w-max items-center justify-center rounded-md px-4 py-2 text-sm font-medium transition-colors focus:outline-none disabled:pointer-events-none disabled:opacity-50'
              )}
            />
          </>
        ) : (
          <LoginBtn className="bg-gradient-to-r from-cyan-500 to-blue-500" />
        )}
      </nav>
      <ModeToggle />
    </header>
  );
}

function MenuIcon(props: any) {
  return (
    <svg
      {...props}
      xmlns="http://www.w3.org/2000/svg"
      width="24"
      height="24"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      strokeWidth="2"
      strokeLinecap="round"
      strokeLinejoin="round"
    >
      <line x1="4" x2="20" y1="12" y2="12" />
      <line x1="4" x2="20" y1="6" y2="6" />
      <line x1="4" x2="20" y1="18" y2="18" />
    </svg>
  );
}
