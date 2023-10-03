import { UserContext } from "@/contexts/auth";
import { useContext, useEffect } from "react";

import { redirect } from "next/navigation";
import { usePathname } from "next/navigation";

export default function useProtectPage() {
  const Auth = useContext(UserContext);
  const path = usePathname();

  useEffect(() => {
    if (Auth?.auth == null) {
      // Send user to login page with callback url
      redirect(`/authentication/login?callback=${path}`);
    }
  }, [Auth, path]);
}
