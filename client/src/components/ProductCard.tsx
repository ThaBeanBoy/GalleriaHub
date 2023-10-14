"use client";

import { ProductType } from "@/lib/types";
import Link from "next/link";
import Button from "./Button";
import { BsCart2, BsCartPlus } from "react-icons/bs";
import { ReactNode, useContext, useEffect, useState } from "react";
import Tooltip from "./Menubar";
import { SlOptionsVertical } from "react-icons/sl";
import { UserContext } from "@/contexts/auth";
import { BsBookmarks } from "react-icons/bs";
import Avatar from "./Avatar";
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
      console.log(Auth?.cart);
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
      >
        Hello
      </Tooltip>

      {/* Product information */}
      <div className="absolute bottom-3 left-2 mt-3 hidden flex-col justify-between px-3 text-white group-hover:flex">
        <p className="text-sm font-semibold">R{price}</p>
        <h3 className="text-lg font-bold text-white">{productName}</h3>
      </div>

      {/* user actions */}
      {Auth?.auth?.user && (
        <div className="absolute right-2 top-2 hidden group-hover:flex">
          {!inCart ? (
            <Button
              // label="Add to cart"
              className="rounded-r-[0] py-0.5 text-xs leading-none"
              label="Add to cart"
              icon={<BsCartPlus />}
              onClick={() => Auth.AddToCartHandler(productID)}
            />
          ) : (
            <Button
              className="rounded-r-[0] py-0.5 text-xs leading-none"
              label="Remove from cart"
              icon={<BsCartPlus />}
              onClick={() => Auth.DeleteFromCartHandler(productID)}
              desctructive
            />
          )}

          <Button
            icon={<BsBookmarks />}
            className="rounded-l-[0] py-0.5 text-sm"
          />
        </div>
      )}
    </div>
  );
}

export function ProductActions({ productID }: { productID: number }) {
  const Auth = useContext(UserContext);

  const [inCart, setIntCart] = useState<boolean | undefined>(undefined);

  useEffect(() => {
    if (Auth) {
      console.log("change");
      setIntCart(
        Auth?.cart.some((CartItem) => {
          console.log(CartItem.product.productID, Number(productID));
          return CartItem.product.productID === Number(productID);
        }) || false,
      );
    }

    // console.log(inCart);
  }, [Auth, Auth?.cart, productID]);

  if (!Auth) {
    return <Link href="/authentication/login">Login</Link>;
  }

  return (
    <div className="flex items-center gap-2">
      {inCart ? (
        <Button
          label="remove from cart"
          variant="flat"
          desctructive
          onClick={() => Auth.DeleteFromCartHandler(productID)}
        />
      ) : (
        <Button
          label="Add to cart"
          icon={<BsCart2 />}
          onClick={() => Auth.AddToCartHandler(productID)}
        />
      )}
      <Button icon={<BsBookmarks />} variant="hollow" />
    </div>
  );
}
