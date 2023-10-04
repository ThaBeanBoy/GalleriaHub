"use client";

import { usePathname } from "next/navigation";

import * as Nav from "@radix-ui/react-navigation-menu";
import * as Avatar from "@radix-ui/react-avatar";

import Link from "next/link";
import { FiChevronDown } from "react-icons/fi";
import { MenuSquareIcon } from "lucide-react";

import Button from "@/components/Button";
import { useContext } from "react";
import { UserContext } from "@/contexts/auth";

type navLinkProps = {
  title: string;
  href: string;
  children?: React.ReactNode;
  display?: "authenticated" | "unauthenticated";
};

export default function Navigation() {
  const pathname = usePathname();
  const Auth = useContext(UserContext);

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
      href: `/authentication/login?callback=${pathname}`,
      display: "unauthenticated",
    },
    {
      title: "dashboard",
      href: `/dashboard`,
      display: "authenticated",
      children: <div>Dash Children</div>,
    },
  ];
  return (
    <>
      <div id="header-container" className="sticky top-0 bg-white">
        <header className="max-width flex items-center justify-between border-b py-5">
          <Link href="/">
            <p>logo</p>
          </Link>

          <Nav.Root className="hidden md:block">
            <Nav.List className="flex items-center gap-4">
              {navLinks.map((navLink, key) => {
                const Item = <NavItem {...navLink} key={key} />;

                if (navLink.display === undefined) return Item;

                if (
                  Auth?.auth === undefined &&
                  navLink.display === "unauthenticated"
                )
                  return Item;

                if (Auth?.auth && navLink.display === "authenticated")
                  return Item;
              })}

              {!Auth?.auth && (
                <li>
                  <Link
                    href={`/authentication?callback=${pathname}`}
                    className="hover:text-active"
                  >
                    <Button label="sign up" />
                  </Link>
                </li>
              )}

              {Auth?.auth && (
                <li>
                  <Avatar.Root className="inline-flex h-[45px] w-[45px] select-none items-center justify-center overflow-hidden rounded-2xl bg-white align-middle shadow">
                    <Avatar.Image
                      src=""
                      alt={Auth?.auth?.user.email}
                      className="h-full w-full rounded-[inherit] object-cover"
                    />
                    <Avatar.Fallback
                      delayMs={600}
                      className="leading-1 bg-active-light flex h-full w-full items-center justify-center text-[15px] font-medium text-white"
                    >
                      {Auth?.auth?.user.email[0]}
                    </Avatar.Fallback>
                  </Avatar.Root>
                </li>
              )}

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
          className="text-violet10 relative top-[1px] transition-transform ease-in group-data-[state=open]:-rotate-180"
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
