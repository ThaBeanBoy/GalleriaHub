"use client";

import Link from "next/link";
import { FaHamburger } from "react-icons/fa";
import { FiChevronDown } from "react-icons/fi";
import { MenuSquareIcon } from "lucide-react";

import * as Nav from "@radix-ui/react-navigation-menu";

import Button from "@/components/Button";

type navLinkProps = {
  title: string;
  href: string;
  children?: React.ReactNode;
};

const navLinks: navLinkProps[] = [
  {
    title: "home",
    href: "/",
  },
  {
    title: "shop",
    href: "/",
    children: <div>Hello</div>,
  },
  {
    title: "categories",
    href: "/",
    children: <div>Hello</div>,
  },
  {
    title: "login",
    href: "/authentication/login",
  },
];

export default function Navigation() {
  return (
    <>
      <div id="header-container" className="sticky top-0 bg-white">
        <header className="max-width flex items-center justify-between border-b py-5">
          <Link href="/">
            <p>logo</p>
          </Link>

          <Nav.Root className="hidden md:block">
            <Nav.List className="flex items-center gap-4">
              {navLinks.map((navLink, key) => (
                <NavItem {...navLink} key={key} />
              ))}
              <li>
                <Link href="/authentication/" className="hover:text-active">
                  <Button label="sign up" />
                </Link>
              </li>

              <Nav.Indicator className="data-[state=visible]:animate-fadeIn data-[state=hidden]:animate-fadeOut top-full z-[1] flex h-[10px] items-end justify-center overflow-hidden transition-[width,transform_250ms_ease]">
                <div className="relative top-[70%] h-[10px] w-[10px] rotate-[45deg] rounded-tl-[2px] bg-white" />
              </Nav.Indicator>
            </Nav.List>

            <div className="perspective-[2000px] absolute left-0 top-full flex w-full justify-center">
              <Nav.Viewport className="data-[state=open]:animate-scaleIn data-[state=closed]:animate-scaleOut relative mt-[10px] h-[var(--radix-navigation-menu-viewport-height)] w-full origin-[top_center] overflow-hidden rounded-[6px] bg-white transition-[width,_height] duration-300 sm:w-[var(--radix-navigation-menu-viewport-width)]" />
            </div>
          </Nav.Root>

          <Button
            icon={<MenuSquareIcon />}
            variant="hollow"
            className="md:hidden"
          />
        </header>
      </div>
    </>
  );
}

function NavItem({ title, href, children }: navLinkProps) {
  const link = (
    <Link
      href={href}
      className="hover:text-active flex items-center gap-1 capitalize text-black"
    >
      {children && (
        <FiChevronDown
          className="text-violet10 relative top-[1px] transition-transform duration-[250] ease-in group-data-[state=open]:-rotate-180"
          aria-hidden
        />
      )}
      <span>{title}</span>
    </Link>
  );

  return children ? (
    <Nav.Item className="relative">
      <Nav.Trigger asChild>{link}</Nav.Trigger>
      <Nav.Content className="data-[motion=from-start]:animate-enterFromLeft data-[motion=from-end]:animate-enterFromRight data-[motion=to-start]:animate-exitToLeft data-[motion=to-end]:animate-exitToRight absolute left-0 top-0 w-full bg-white shadow-xl sm:w-auto">
        {children}
      </Nav.Content>
    </Nav.Item>
  ) : (
    link
  );
}
