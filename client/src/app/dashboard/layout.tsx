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
  FiShoppingCart,
} from "react-icons/fi";

import { BsGraphUp, BsListNested } from "react-icons/bs";

import { VscVerified } from "react-icons/vsc";
import { PiBuildings } from "react-icons/pi";
import { GiReceiveMoney } from "react-icons/gi";
import { CiBoxes } from "react-icons/ci";

import Button from "@/components/Button";

import { cn } from "@/lib/utils";
import { useState } from "react";
import useProtectPage from "@/lib/protectPage";

const Navigation: { title: string; href: string; icon: React.ReactNode }[] = [
  {
    title: "cart",
    href: "/dashboard",
    icon: <FiShoppingCart />,
  },
  {
    title: "my profile",
    href: "/dashboard/profile",
    icon: <FiUser />,
  },
  {
    title: "lists",
    href: "/dashboard/lists",
    icon: <BsListNested />,
  },
  {
    title: "invoices",
    href: "/dashboard/invoices",
    icon: <FiClock />,
  },
  {
    title: "sales",
    href: "/dashboard/sales",
    icon: <GiReceiveMoney />,
  },
  {
    title: "products",
    href: "/dashboard/products",
    icon: <CiBoxes />,
  },
  // {
  //   title: "verify",
  //   href: "/dashboard/verify",
  //   icon: <VscVerified />,
  // },
  // {
  //   title: "gallery",
  //   href: "/dashboard/gallery",
  //   icon: <PiBuildings />,
  // },
];

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // useProtectPage({ from: "unauthenticated" });

  const [navExpanded, setNavExpanded] = useState(true);

  const currentPath = usePathname();

  return (
    <div className="flex gap-4">
      <aside
        className={cn("sticky top-0", { "w-[195px] resize-x": navExpanded })}
      >
        <Button
          icon={navExpanded ? <FiChevronLeft /> : <FiChevronRight />}
          // variant="hollow"
          onClick={() => setNavExpanded(!navExpanded)}
          className="mb-4"
        />

        <ul>
          {Navigation.map(({ title, href, icon }, key) => {
            const active = currentPath?.startsWith(href);
            return (
              <li key={key} className="">
                <Link
                  href={href}
                  className={cn(
                    "hover:text-active-light flex h-[47.2px] items-center gap-3 rounded-xl rounded-r-none border-r-2 px-4 py-3 text-sm font-medium capitalize text-black",
                    { "text-active border-2 border-r-0": active },
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
