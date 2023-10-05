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
    title: "dashboard",
    href: "/dashboard",
    icon: <BsGraphUp />,
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
    href: "/dashboard/invoices",
    icon: <GiReceiveMoney />,
  },
  {
    title: "products",
    href: "/dashboard/products",
    icon: <CiBoxes />,
  },
  {
    title: "verify",
    href: "/dashboard/verify",
    icon: <VscVerified />,
  },
  {
    title: "gallery",
    href: "/dashboard/gallery",
    icon: <PiBuildings />,
  },
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
      <aside className={cn({ "w-[284px]": navExpanded })}>
        <Button
          icon={navExpanded ? <FiChevronLeft /> : <FiChevronRight />}
          // variant="hollow"
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
                    "hover:text-active flex h-[50px] items-center gap-3 rounded-xl rounded-r-none border-r-2 px-4 py-3 font-medium capitalize text-black",
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
