"use client";

import { ProductType } from "@/lib/types";
import Link from "next/link";
import Button from "./Button";
import { BsCart2, BsCartPlus, BsPlus } from "react-icons/bs";
import { ReactNode, useContext, useEffect, useState } from "react";
import Tooltip from "./Menubar";
import { SlOptionsVertical } from "react-icons/sl";
import { UserContext } from "@/contexts/auth";
import { BsBookmarks } from "react-icons/bs";
import Avatar from "./Avatar";
import Menubar from "./Menubar";

import { cn } from "@/lib/utils";

export default function ProductCard(
  props: {
    tooltip?: ReactNode;
    displaySeller?: boolean;
    displayActions?: boolean;
    href?: string;
  } & ProductType,
) {
  const Auth = useContext(UserContext);

  return (
    <div key={props.productID} className="group relative">
      <Link
        href={props.href ? props.href : `/shop/${props.productID}`}
        className="group flex h-full w-full flex-col justify-between rounded-3xl drop-shadow"
      >
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src={props.images.length > 0 ? props.images[0] : ""}
          alt={`${props.productName} image`}
          width="284"
          height="284"
          className={cn(
            "rounded-[inherit] duration-500 group-hover:brightness-50",
            { "brightness-50": Auth?.InCart(props.productID) },
          )}
        />
      </Link>

      {/* Artist */}
      {props.displaySeller && (
        <Tooltip
          trigger={
            <Avatar
              src=""
              alt={props.seller.username}
              fallback="ar"
              className="absolute left-2 top-2 opacity-0 duration-500 group-hover:opacity-100"
            />
          }
          className="z-40"
        >
          Hello
        </Tooltip>
      )}

      {/* Product information */}
      <div className="absolute bottom-3 left-2 mt-3 flex flex-col justify-between px-3 text-white opacity-0 duration-500 group-hover:opacity-100">
        <p className="text-sm font-semibold">R{props.price}</p>
        <h3 className="text-2xl font-bold text-white">{props.productName}</h3>
      </div>

      {/* user actions */}
      {Auth?.auth?.user && (props.displayActions || true) && (
        <ProductActions
          {...props}
          className="absolute right-2 top-2 flex opacity-0 duration-500 group-hover:opacity-100"
          buttonClassName="rounded-r-[0] py-0.5 text-xs leading-none"
        />
      )}

      {Auth?.InCart(props.productID) && (
        <p className="absolute right-2 top-2 touch-none rounded-full bg-red-600 px-2 py-1 text-xs font-bold text-white duration-500 group-hover:opacity-0">
          in cart
        </p>
      )}
    </div>
  );
}

export function ProductActions(
  props: {
    className?: string;
    buttonClassName?: string;
  } & ProductType,
) {
  const Auth = useContext(UserContext);

  const [inCart, setIntCart] = useState<boolean | undefined>(undefined);

  useEffect(() => {
    if (Auth) {
      setIntCart(
        Auth?.cart.some((CartItem) => {
          return CartItem.product.productID === Number(props.productID);
        }) || false,
      );
    }
  }, [Auth, Auth?.cart, props.productID]);

  if (!Auth) {
    return <Link href="/authentication/login">Login</Link>;
  }

  return (
    <div className={cn("flex", props.className)}>
      {
        <Button
          // label="Add to cart"
          className={cn("rounded-r-[0]", props.buttonClassName)}
          label={!inCart ? "Add to cart" : "Remove from cart"}
          icon={<BsCartPlus />}
          onClick={() =>
            !inCart
              ? Auth.AddToCartHandler(props.productID)
              : Auth.DeleteFromCartHandler(props.productID)
          }
          desctructive={inCart}
        />
      }
      <Button
        icon={<BsBookmarks />}
        className="rounded-l-[0] border-l-white py-0.5 text-sm"
        onClick={() => Auth.OpenListEditor({ ...props })}
      />
    </div>
  );
}
