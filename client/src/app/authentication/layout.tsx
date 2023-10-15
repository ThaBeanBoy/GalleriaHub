"use client";

import React from "react";
import Image from "next/image";

import useProtectPage from "@/lib/protectPage";

export default function AuthenticationLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // useProtectPage({ from: "authenticated" });

  return (
    <div className="max-width flex h-full w-full items-center justify-center lg:justify-between lg:px-6">
      {children}
      <Image
        width={506}
        height={768}
        src="/sign-up-hero.png"
        className="hidden lg:block"
        alt="sign up"
      />
    </div>
  );
}
