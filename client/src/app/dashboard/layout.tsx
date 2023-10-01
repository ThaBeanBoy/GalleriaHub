"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

import {
  FiChevronLeft,
  FiChevronRight,
  FiUser,
  FiFile,
  FiClock,
  FiDollarSign,
} from "react-icons/fi";

import Button from "@/components/Button";
import { cn } from "@/lib/utils";
import { useState } from "react";

const Navigation: { title: string; href: string; icon: React.ReactNode }[] = [
  {
    title: "dashboard",
    href: "/dashboard",
    icon: <FiUser />,
  },
  {
    title: "my profile",
    href: "/dashboard/profile",
    icon: <FiUser />,
  },
  {
    title: "lists",
    href: "/dashboard/lists",
    icon: <FiFile />,
  },
  {
    title: "invoices",
    href: "/dashboard/invoices",
    icon: <FiClock />,
  },
  {
    title: "sales",
    href: "/dashboard/invoices",
    icon: <FiDollarSign />,
  },
  {
    title: "products",
    href: "/dashboard/products",
    icon: <FiDollarSign />,
  },
  {
    title: "verify",
    href: "/dashboard/verify",
    icon: <FiDollarSign />,
  },
  {
    title: "gallery",
    href: "/dashboard/gallery",
    icon: <FiDollarSign />,
  },
];

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const [navExpanded, setNavExpanded] = useState(true);

  const currentPath = usePathname();

  return (
    <div className="flex gap-4">
      <aside className={cn({ "w-[284px]": navExpanded })}>
        <Button
          icon={navExpanded ? <FiChevronLeft /> : <FiChevronRight />}
          variant="hollow"
          onClick={() => setNavExpanded(!navExpanded)}
          className="mb-4"
        />

        <ul>
          {Navigation.map(({ title, href, icon }, key) => {
            const active = currentPath == href;
            return (
              <li key={key} className="">
                <Link
                  href={href}
                  className={cn(
                    "hover:text-active flex h-[50px] items-center gap-3 rounded-xl rounded-r-none border-r px-4 py-3 font-medium capitalize text-black",
                    { "text-active border border-r-0": active },
                  )}
                >
                  {" "}
                  {icon} {navExpanded && <label>{title}</label>}
                </Link>
              </li>
            );
          })}
        </ul>
      </aside>

      <div id="dash-container" className="flex-1">
        {children}
      </div>
    </div>
  );
}
