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
export default function ProductCard({
  productID,
  productName,
  images,
  price,
  tooltip,
  seller,
}: { tooltip?: ReactNode } & ProductType) {
  const Auth = useContext(UserContext);

  const [inCart, setIntCart] = useState(false);

  useEffect(() => {
    if (Auth) {
      setIntCart(
        Auth?.cart.some(
          (CartItem) => CartItem.product.productID === productID,
        ) || false,
      );
    }
  }, [Auth, Auth?.cart, productID]);

  return (
    <div key={productID} className="group relative">
      <Link
        href={`/shop/${productID}`}
        className="group flex h-full w-full flex-col justify-between rounded-lg drop-shadow"
      >
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src={images.length > 0 ? images[0] : ""}
          alt={`${productName} image`}
          width="284"
          height="284"
          className="rounded-[inherit] duration-300 group-hover:brightness-50"
        />
      </Link>

      {/* Artist */}
      <Tooltip
        trigger={
          <Avatar
            src=""
            alt={seller.username}
            fallback="ar"
            className="absolute left-2 top-2 hidden group-hover:block"
          />
        }
        className="z-40"
      >
        Hello
      </Tooltip>

      {/* Product information */}
      <div className="absolute bottom-3 left-2 mt-3 hidden flex-col justify-between px-3 text-white group-hover:flex">
        <p className="text-sm font-semibold">R{price}</p>
        <h3 className="text-2xl font-bold text-white">{productName}</h3>
      </div>

      {/* user actions */}
      {Auth?.auth?.user && (
        <ProductActions
          productID={productID}
          className="absolute right-2 top-2 hidden group-hover:flex"
          buttonClassName="rounded-r-[0] py-0.5 text-xs leading-none"
        />
      )}
    </div>
  );
}

export function ProductActions({
  productID,
  className,
  buttonClassName,
}: {
  productID: number;
  className?: string;
  buttonClassName?: string;
}) {
  const Auth = useContext(UserContext);

  const [inCart, setIntCart] = useState<boolean | undefined>(undefined);

  useEffect(() => {
    if (Auth) {
      setIntCart(
        Auth?.cart.some((CartItem) => {
          return CartItem.product.productID === Number(productID);
        }) || false,
      );
    }
  }, [Auth, Auth?.cart, productID]);

  if (!Auth) {
    return <Link href="/authentication/login">Login</Link>;
  }

  return (
    <div className={cn("flex", className)}>
      {
        <Button
          // label="Add to cart"
          className={cn("rounded-r-[0]", buttonClassName)}
          label={!inCart ? "Add to cart" : "Remove from cart"}
          icon={<BsCartPlus />}
          onClick={() =>
            !inCart
              ? Auth.AddToCartHandler(productID)
              : Auth.DeleteFromCartHandler(productID)
          }
          desctructive={inCart}
        />
      }
      <Tooltip
        trigger={
          <Button
            icon={<BsBookmarks />}
            className="rounded-l-[0] py-0.5 text-sm"
          />
        }
        className="z-40"
        align="end"
      >
        {Auth.lists.map((list) => (
          <Button
            label={
              <>
                <BsPlus /> {list.name}
              </>
            }
            className="mb-2 p-0 text-xs last:mb-0"
            variant="flat"
            key={`${productID}-${list.listID}`}
            onClick={() => Auth.AddToListHandler(list.listID, productID)}
          />
        ))}
      </Tooltip>
    </div>
  );
}
