"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";
import { DocLink } from "@/app/documentation/layout";
import clsx from "clsx";

export default function DocLink({ title, href, icon }: DocLink) {
  const active =
    usePathname() ===
    `/documentation${href.trim() !== "" ? "/" : ""}${href.trim()}`;
  return (
    <li className="mb-1 last:mb-0">
      <Link
        href={href}
        className={clsx({
          // default styles of doclink
          "flex items-center gap-1 rounded-full px-3 py-2 capitalize": true,
          "hover:text-active text-black hover:bg-gray-100": !active,
          "bg-grey-light text-active": active,
        })}
      >
        {icon ? icon : <span className="w-[16px]" />} {title}
      </Link>
    </li>
  );
}
