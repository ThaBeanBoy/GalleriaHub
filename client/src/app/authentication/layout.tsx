import React from "react";
import Image from "next/image";

export default function AuthenticationLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="flex h-full w-full items-center justify-center lg:justify-between lg:px-6">
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
