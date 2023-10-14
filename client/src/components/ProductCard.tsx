"use client";

import { ProductType } from "@/lib/types";
import Link from "next/link";
import Button from "./Button";
import { BsCartPlus } from "react-icons/bs";
import { ReactNode, useContext, useEffect, useState } from "react";
import Tooltip from "./Menubar";
import { SlOptionsVertical } from "react-icons/sl";
import { UserContext } from "@/contexts/auth";
import { BsBookmarks } from "react-icons/bs";
export default function ProductCard({
  productID,
  productName,
  images,
  price,
  tooltip,
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
  }, [Auth?.cart, productID]);

  return (
    <div key={productID} className="group relative">
      <Link
        href={`/shop/${productID}`}
        className="group flex h-full w-full flex-col justify-between rounded-lg bg-white pb-2 drop-shadow"
      >
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src={images.length > 0 ? images[0] : ""}
          alt={`${productName} image`}
          width="284"
          height="284"
        />

        {/* Product information */}
        <div className="mt-3 flex justify-between px-3 text-black">
          <span className="group-hover:text-active text-sm text-black">
            {productName}
          </span>
          <span className="rounded-full bg-gray-300 px-2 py-1 text-xs font-semibold">
            R{price}
          </span>
        </div>
      </Link>

      {/* user actions */}
      {Auth?.auth?.user && (
        <div className="absolute left-[50%] top-[50%] hidden w-[90%] translate-x-[-50%] translate-y-[-50%] justify-between gap-2 rounded-xl bg-white px-3 py-2 drop-shadow-xl group-hover:flex">
          {!inCart ? (
            <Button
              // label="Add to cart"
              className="p-0 text-sm"
              label="Add to cart"
              icon={<BsCartPlus />}
              onClick={() => Auth.AddToCartHandler(productID)}
              variant="flat"
            />
          ) : (
            <Button
              className="p-0 text-sm"
              label="Remove from cart"
              variant="flat"
              onClick={() => Auth.DeleteFromCartHandler(productID)}
              desctructive
            />
          )}

          <Button icon={<BsBookmarks />} variant="flat" />
        </div>
      )}

      {/* {tooltip && (
        <Tooltip
          trigger={
            <Button
              className="absolute left-2 right-[4px] top-2 hidden hover:block group-hover:block"
              icon={<SlOptionsVertical />}
            />
          }
        >
          {tooltip}
        </Tooltip>
      )} */}
    </div>
  );
}
