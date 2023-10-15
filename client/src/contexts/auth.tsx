import axios from "axios";
// import { redirect } from "next/dist/server/api-utils";
import { usePathname, redirect } from "next/navigation";
import { createContext, useEffect, useState } from "react";

import { useToast } from "@/components/ui/use-toast";

import {
  CartType,
  JwtType,
  ListType,
  ProductType,
  UserType,
} from "@/lib/types";
import Link from "next/link";

import * as Toast from "@radix-ui/react-toast";
import Input from "@/components/Input";
import Button from "@/components/Button";
import { BsBookmarks, BsPlus } from "react-icons/bs";

import { AspectRatio } from "@/components/ui/aspect-ratio";

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
  cart: CartType;
  AddToCartHandler: (ProductID: number, Toast?: boolean) => void;
  DeleteFromCartHandler: (ProductID: number, Toast?: boolean) => void;
  UpdateCartHandler: (
    ProductID: number,
    Quantity: number,
    Toast?: boolean,
  ) => void;
  PayHandler: (Toast: boolean) => void;
  InCart: (ProductID: number) => boolean;

  // Lists
  lists: any[];
  AddToListHandler: (listID: number, productID: number, Toast: boolean) => void;
  RemoveFromListHandler: (
    listID: number,
    productID: number,
    Toast: boolean,
  ) => void;
  OpenListEditor: (Product: ProductType) => void;
};

export const UserContext = createContext<UserContextType | null>(null);

export default function AuthProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const pathname = usePathname();
  const [auth, setAuth] = useState<userContextAuthType>(undefined);

  const [cart, setCart] = useState<CartType>([]);
  const [lists, setLists] = useState<ListType[]>([]);

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
      setCart(data.user.cart);

      toast({
        title: "Login",
        description: `Successfully signed up as ${auth?.user.username}`,
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

      const { data } = await axios<AuthType>({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/sign-up`,
        data: formData,
      });

      setAuth(ResponseDataToAuthType(data));
      setCart(data.user.cart);

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

  const AddToCartHandler = async (ProductID: number, Toast: boolean = true) => {
    try {
      if (Toast) {
        toast({
          title: "Cart",
          description: "Attempting to add to cart",
        });
      }

      const { data } = await axios<CartType>({
        method: "put",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/cart/add`,

        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },

        params: {
          productID: ProductID,
        },
      });

      setCart(data);

      if (Toast) {
        toast({
          title: "Cart",
          description: "Added to cart",
          action: ViewCartCompontent,
        });
      }
    } catch (error: any) {
      if (Toast) {
        toast({
          title: "Cart",
          description: "Something went wrong",
          variant: "destructive",
        });
      }
    }
  };

  const DeleteFromCartHandler = async (
    ProductID: number,
    Toast: boolean = true,
  ) => {
    try {
      if (Toast) {
        toast({
          title: "Cart",
          description: "Attempting to remove from cart",
        });
      }

      const { data } = await axios<CartType>({
        method: "delete",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/cart/delete`,

        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },

        params: {
          productID: ProductID,
        },
      });

      setCart(data);

      if (Toast) {
        toast({
          title: "Cart",
          description: "Deleted from the cart",
          action: data.length > 0 ? ViewCartCompontent : <></>,
        });
      }
    } catch (error: any) {
      if (Toast) {
        toast({
          title: "Cart",
          description: "Something went wrong",
          variant: "destructive",
        });
      }
    }
  };

  const UpdateCartHandler = async (
    ProductID: number,
    Quantity: number,
    Toast: boolean = true,
  ) => {
    try {
      if (Toast) {
        toast({
          title: "Cart",
          description: "Updating Cart Item",
        });
      }

      const { data } = await axios<CartType>({
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
      setCart(data);

      if (Toast) {
        toast({
          title: "Cart",
          description: "Successfully updated",
        });
      }
    } catch (error) {
      console.log(error);
      if (Toast) {
        toast({
          title: "Toast",
          description: "Something went wrong",
        });
      }
    }
  };

  const PayHandler = async (Toast: boolean = true) => {
    try {
      if (Toast) {
        toast({ title: "Purchase", description: "Processing payment" });
      }

      await axios({
        method: "put",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/orders/`,
        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },
      });

      setCart([]);

      if (Toast) {
        toast({
          title: "Purchase",
          description: "Successfully purchase",
          action: <Link href="/dashboard/invoices"></Link>,
        });
      }
    } catch (error) {
      console.log(error);

      if (Toast) {
        toast({
          title: "Purchase",
          description: "Something went wrong",
          variant: "destructive",
        });
      }
    }
  };

  const AddToListHandler = async (
    listID: number,
    productID: number,
    Toast: boolean,
  ) => {
    try {
      if (Toast) {
        toast({
          title: "Lists",
          description: "Adding product to list",
        });
      }

      const { data } = await axios<ListType>({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/lists/${listID}/${productID}`,
        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },
      });

      data.createdOn = new Date(data.createdOn);
      data.lastUpdate = new Date(data.lastUpdate);

      // updating the list type
      const tempIndx = lists.map((list) => list.listID).indexOf(data.listID);
      const tempLists = lists;
      tempLists[tempIndx] = data;

      setLists(tempLists);

      if (Toast) {
        toast({
          title: "Lists",
          description: "Added to list",
        });
      }
    } catch (error) {
      if (Toast) {
        toast({
          title: "Lists",
        });
      }
    }
  };

  const RemoveFromListHandler = async (
    listID: number,
    productID: number,
    Toast: boolean,
  ) => {
    try {
      if (Toast) {
        toast({ title: "Lists", description: "Removing from list" });
      }

      const { data } = await axios<ListType>({
        method: "delete",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/lists/${listID}/${productID}`,
        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },
      });

      data.createdOn = new Date(data.createdOn);
      data.lastUpdate = new Date(data.lastUpdate);

      // updating the list type
      const tempIndx = lists.map((list) => list.listID).indexOf(data.listID);
      const tempLists = lists;
      tempLists[tempIndx] = data;

      if (Toast) {
        toast({ title: "List", description: "Removed from list" });
      }
    } catch (error: any) {
      console.log(error);
      if (Toast) {
        toast({
          title: "Lists",
          description: "Something went wrong",
          variant: "destructive",
        });
      }
    }
  };

  const InCart = (ProductID: number) => {
    return cart.some((CartItem) => {
      return CartItem.product.productID === Number(ProductID);
    });
  };

  const OpenListEditor = (Product: ProductType) => {
    // setListToastOpen(true);
    toast({
      duration: 15000,
      description: (
        <div className="max-h-11 w-full overflow-scroll">
          <h4 className="flex items-center gap-2 pl-4 font-semibold">
            {auth?.user.username}&apos;s lists <BsBookmarks />
          </h4>

          <hr className="my-2" />

          <div className="mb-2 w-[64px] overflow-hidden">
            <AspectRatio ratio={1}>
              {/* eslint-disable-next-line @next/next/no-img-element */}
              <img
                src={Product.images[0]}
                alt={Product.productName}
                // width="75"
                // height={"76"}
                className="h-full w-full rounded-md object-cover"
              />
            </AspectRatio>
          </div>

          <div className="mb-3 flex">
            <Input placeholder="search lists" className="rounded-r-[0]" />
            <Button icon={<BsPlus />} className="rounded-l-[0]" />
          </div>

          <ul>
            {lists.map(({ listID, name, items }, key) => {
              const inList = items.some(
                (product) => product.productID === Product.productID,
              );

              return (
                <li
                  key={`user-list-toast-list-${key}`}
                  className="mb-3 last:mb-0"
                >
                  <Button
                    label={`${inList ? "Remove from" : "Add to"} "${name}"`}
                    className="m-0 p-0 pl-4"
                    variant="flat"
                    desctructive={inList}
                    onClick={() =>
                      inList
                        ? RemoveFromListHandler(listID, Product.productID, true)
                        : AddToListHandler(listID, Product.productID, true)
                    }
                  />
                </li>
              );
            })}
          </ul>
        </div>
      ),
    });
  };

  // Auth use effect
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
            setCart(data.cart);
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

  useEffect(() => {
    if (auth) {
      axios<ListType[]>({
        method: "get",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/lists/`,
        headers: {
          Authorization: `Bearer ${auth?.jwt.token}`,
        },
      })
        .then(({ data }) => {
          data = data.map((temp) => {
            temp.createdOn = new Date(temp.createdOn);
            temp.lastUpdate = new Date(temp.lastUpdate);
            return temp;
          });
          setLists(data);
        })
        .catch((error) => console.log(error));
    }
  }, [auth, auth?.jwt.token]);

  return (
    <>
      <UserContext.Provider
        value={{
          auth,
          loginHandler,
          signUpHandler,
          logoutHandler,

          cart,
          AddToCartHandler,
          DeleteFromCartHandler,
          UpdateCartHandler,
          PayHandler,
          InCart,

          lists,
          AddToListHandler,
          OpenListEditor,
          RemoveFromListHandler,
        }}
      >
        {children}
      </UserContext.Provider>
    </>
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
