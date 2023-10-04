import { UserContext } from "@/contexts/auth";
import { useContext, useEffect } from "react";

import { redirect, useRouter } from "next/navigation";
import { usePathname } from "next/navigation";

export default function useProtectPage({
  from = "unauthenticated",
}: {
  from?: "unauthenticated" | "authenticated";
}) {
  const router = useRouter();

  const Auth = useContext(UserContext);
  const path = usePathname();

  useEffect(() => {
    console.log(`protection order`, Auth?.auth);
    if (!Auth?.auth) {
      // Send user to login page with callback url
      redirect(`/authentication/login?callback=${path}`);
    } else if (from === "unauthenticated") {
      router.back();
    }
  }, [Auth, from, path, router]);
}
