import axios from "axios";
// import { redirect } from "next/dist/server/api-utils";
import { usePathname, redirect } from "next/navigation";
import { createContext, useEffect, useState } from "react";

import { useToast } from "@/components/ui/use-toast";

import { JwtType, UserType } from "@/lib/types";
import Link from "next/link";

export type AuthType = {
  jwt: JwtType;
  user: UserType;
};

type loginProps = {
  username: string;
  password: string;
  success?: (data: any) => void;
  failed?: (error: any) => void;
};

type signUpProps = {
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  success?: (data: any) => void;
  failed?: (error: any) => void;
};

type userContextAuthType = AuthType | undefined;

export type UserContextType = {
  auth: userContextAuthType;
  // Authentication
  loginHandler: (loginDetails: loginProps) => void;
  signUpHandler: (signUpDetails: signUpProps) => void;
  logoutHandler: () => void;

  // Cart
  AddToCartHandler: (ProductID: number) => void;
  DeleteFromCartHandler: (ProductID: number) => void;
  UpdateCartHandler: (ProductID: number, Quantity: number) => void;
};

export const UserContext = createContext<UserContextType | null>(null);

export default function AuthProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const pathname = usePathname();
  const [auth, setAuth] = useState<userContextAuthType>(undefined);

  const { toast } = useToast();

  const loginHandler = async ({
    username,
    password,
    success,
    failed,
  }: loginProps) => {
    try {
      const formData = new FormData();
      formData.append("username", username);
      formData.append("password", password);

      const { data } = await axios<AuthType>({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/login`,
        data: formData,
      });

      setAuth(ResponseDataToAuthType(data));

      toast({
        title: "Login",
        description: `Successfully logged in as ${auth?.user.username}`,
      });

      if (success) success(data);
    } catch (error: any) {
      if (failed) failed(error);
    }
  };

  const signUpHandler = async ({
    email,
    username,
    password,
    confirmPassword,
    success,
    failed,
  }: signUpProps) => {
    try {
      const formData = new FormData();
      formData.append("email", email);
      formData.append("username", username);
      formData.append("password", password);
      formData.append("confirm-password", confirmPassword);

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/sign-up`,
        data: formData,
      });

      setAuth(ResponseDataToAuthType(data));

      toast({
        title: "Login",
        description: `Successfully logged in as ${auth?.user.username}`,
      });

      if (success) success(data);
    } catch (error: any) {
      if (failed) failed(error);
    }
  };

  const logoutHandler = () => {
    //make the user object null
    // remove the jwt token from the cookies/internal storage
    localStorage.removeItem("jwt");

    setAuth(undefined);

    toast({
      title: "Logout",
      description: "Successfully logged out",
    });
  };

  const ViewCartCompontent = <Link href="/cart">View Cart</Link>;

  const AddToCartHandler = async (ProductID: number) => {
    try {
      toast({
        title: "Cart",
        description: "Attempting to add to cart",
      });

      const { data } = await axios({
        method: "put",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/cart/add`,

        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },

        params: {
          productID: ProductID,
        },
      });

      toast({
        title: "Cart",
        description: "Added to cart",
        action: ViewCartCompontent,
      });

      console.log(data);
    } catch (error: any) {
      toast({
        title: "Cart",
        description: "Something went wrong",
        variant: "destructive",
      });
    }
  };

  const DeleteFromCartHandler = async (ProductID: number) => {
    try {
      toast({
        title: "Cart",
        description: "Attempting to add to cart",
      });

      const { data } = await axios({
        method: "delete",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/cart/delete`,

        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },

        params: {
          productID: ProductID,
        },
      });

      console.log(data);

      toast({
        title: "Cart",
        description: "Deleted from the cart",
        action: ViewCartCompontent,
      });
    } catch (error: any) {
      toast({
        title: "Cart",
        description: "Something went wrong",
        variant: "destructive",
      });
    }
  };

  const UpdateCartHandler = async (ProductID: number, Quantity: number) => {
    try {
      toast({
        title: "Cart",
        description: "Updating Cart Item",
      });

      const { data } = await axios({
        method: "put",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/cart/update`,

        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },

        params: {
          productID: ProductID,
          quantity: Quantity,
        },
      });

      console.log(data);
    } catch (error) {
      console.log(error),
        toast({
          title: "Toast",
          description: "Something went wrong",
        });
    }
  };

  useEffect(() => {
    console.log(auth);

    if (auth) {
      // handling the expirations
      localStorage.setItem("jwt", JSON.stringify(auth.jwt));

      setTimeout(() => {
        alert("Authentication token expired, please login again");
        // removing token from storage
        localStorage.removeItem("jwt");

        // setting auth to null
        setAuth(undefined);

        // redirecting
        redirect(`/authentication/login`);
      }, auth.jwt.expiryDate.getTime() - new Date().getTime());
    } else {
      // handling retrieving token from memory
      try {
        const JwtInStorage = localStorage.getItem("jwt");

        if (!JwtInStorage) {
          throw new Error("JWT Token not in storage");
        }

        const jwt = JSON.parse(JwtInStorage) as JwtType;
        jwt.expiryDate = new Date(jwt.expiryDate);

        if (JwtExpired(jwt)) {
          // Removing from session storage
          localStorage.removeItem("jwt");
          throw new Error("Web token expired");
        }

        // Get the user object
        axios<UserType>({
          method: "get",
          headers: {
            Authorization: `Bearer ${jwt.token}`,
          },
          url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/get-user`,
        })
          .then(({ data }) => {
            setAuth(ResponseDataToAuthType({ jwt, user: data }));
          })
          .catch((error) => {
            console.log(error);
          });
      } catch (error: any) {
        // console.log(error.mess);
        console.warn(error.message);
      }
    }
  }, [auth, pathname]);

  return (
    <UserContext.Provider
      value={{
        auth,
        loginHandler,
        signUpHandler,
        logoutHandler,
        AddToCartHandler,
        DeleteFromCartHandler,
        UpdateCartHandler,
      }}
    >
      {children}
    </UserContext.Provider>
  );
}

function ResponseDataToAuthType(data: AuthType) {
  // Making dates
  data.jwt.expiryDate = new Date(data.jwt.expiryDate);
  data.user.createdOn = new Date(data.user.createdOn);
  data.user.lastUpdate = new Date(data.user.lastUpdate);

  return data;
}

function JwtExpired(JWT: JwtType) {
  console.log(JWT.expiryDate.getTime() - new Date().getTime());
  return JWT.expiryDate.getTime() - new Date().getTime() < 0;
}
