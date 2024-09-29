import {
  FaBook,
  FaDiscord,
  FaRedditAlien,
  FaTelegramPlane,
  FaTwitter,
} from "react-icons/fa";

import { Separator } from "@/components/ui/separator";

const sections = [
  {
    title: "Product",
    links: [
      { name: "Overview", href: "#" },
      { name: "Pricing", href: "#" },
      { name: "Marketplace", href: "#" },
      { name: "Features", href: "#" },
      { name: "Integrations", href: "#" },
      { name: "Pricing", href: "#" },
    ],
  },
  {
    title: "Company",
    links: [
      { name: "About", href: "#" },
      { name: "Team", href: "#" },
      { name: "Blog", href: "#" },
      { name: "Careers", href: "#" },
      { name: "Contact", href: "#" },
      { name: "Privacy", href: "#" },
    ],
  },
  {
    title: "Resources",
    links: [
      { name: "Help", href: "#" },
      { name: "Sales", href: "#" },
      { name: "Advertise", href: "#" },
    ],
  },
];

export const Footer = () => {
  return (
    <section className="py-28 w-full p-3 md:ml-36 md:mt-30">
      <div className="container">
        <footer>
          <div className="flex flex-col justify-between gap-4 md:flex-row md:items-center">
            <div className="flex flex-col gap-4 md:flex-row md:items-center">
              <div className="flex gap-2">
                <a
                  href="#"
                  className="inline-flex items-center justify-center rounded-lg bg-primary p-2.5"
                >
                  <FaBook className="size-7 text-background" />
                </a>
              </div>
              <div className="font-bold text-2xl">
                <h1>eBid</h1>
              </div>
            </div>
          </div>
          <Separator className="my-14 sm:mr-10 md:ml-20" />
          <div className="hidden sm:grid gap-8 md:grid-cols-2 lg:grid-cols-4">
            {sections.map((section, sectionIdx) => (
              <div key={sectionIdx}>
                <h3 className="mb-4 font-bold">{section.title}</h3>
                <ul className="space-y-4 text-muted-foreground">
                  {section.links.map((link, linkIdx) => (
                    <li
                      key={linkIdx}
                      className="font-medium hover:text-primary"
                    >
                      <a href={link.href}>{link.name}</a>
                    </li>
                  ))}
                </ul>
              </div>
            ))}
            <div>
              <h3 className="mb-4 font-bold">Legal</h3>
              <ul className="space-y-4 text-muted-foreground">
                <li className="font-medium hover:text-primary">
                  <a href="#">Term of Services</a>
                </li>
                <li className="font-medium hover:text-primary">
                  <a href="#">Privacy Policy</a>
                </li>
              </ul>
              <h3 className="mb-4 mt-8 font-bold">Social</h3>
              <ul className="flex items-center space-x-6 text-muted-foreground">
                <li className="font-medium hover:text-primary">
                  <a href="#">
                    <FaDiscord className="size-6" />
                  </a>
                </li>
                <li className="font-medium hover:text-primary">
                  <a href="#">
                    <FaRedditAlien className="size-6" />
                  </a>
                </li>
                <li className="font-medium hover:text-primary">
                  <a href="#">
                    <FaTwitter className="size-6" />
                  </a>
                </li>
                <li className="font-medium hover:text-primary">
                  <a href="#">
                    <FaTelegramPlane className="size-6" />
                  </a>
                </li>
              </ul>
            </div>
          </div>
          <p className="text-sm text-muted-foreground mt-5">
            © 2024 duynguyenbui. All rights reserved.
          </p>
        </footer>
      </div>
    </section>
  );
};
