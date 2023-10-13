"use client";

import { UserContext } from "@/contexts/auth";
import { useContext, useEffect, useState } from "react";

import Input from "@/components/Input";
import Link from "next/link";
import Button from "@/components/Button";
import { BsBookmarks, BsCreditCard2Back } from "react-icons/bs";
import { SlCalender } from "react-icons/sl";
import { CartItemType } from "@/lib/types";

import { LiaTimesSolid } from "react-icons/lia";
import { BiMinus, BiPlus } from "react-icons/bi";

export default function Cart() {
  const Auth = useContext(UserContext);

  const [total, setTotal] = useState(0);

  useEffect(() => {
    if (Auth != null) {
      setTotal(
        Auth.cart
          .map((CartItem) => CartItem.product.price)
          .reduce((accumulator, currentValue) => accumulator + currentValue, 0),
      );
    }
  }, [Auth, Auth?.cart]);

  if (Auth == null) return "please login";

  if (Auth.cart.length === 0)
    return (
      <p>
        Visit the <Link href="/shop">shop</Link> to add some products
      </p>
    );

  return (
    <main className="flex justify-between gap-4">
      {/* Products */}
      <div className="flex-1">
        <h1 className="mb-4 text-3xl font-black">
          {Auth.auth?.user.username}&apos;s Cart
        </h1>
        <ul>
          {Auth.cart.map((CartItem, key) => (
            <li className="border-b last:border-b-0" key={key}>
              <CartItemComponent {...CartItem} />
            </li>
          ))}
        </ul>
      </div>

      {/* payment */}
      <aside className="sticky top-4 flex h-min w-full max-w-[400px] flex-col gap-2 border-l-2 pl-2 text-sm">
        <p className="pl-4 text-lg font-bold">Order Summary</p>

        <div className="flex justify-between pl-4">
          <label className="font-semibold" htmlFor="">
            Sub Total
          </label>
          <p>R{total}</p>
        </div>

        <div className="flex justify-between pl-4">
          <label className="font-semibold" htmlFor="">
            VAT (15%)
          </label>
          <p>R{total * 0.15}</p>
        </div>

        <div className="flex w-full items-end">
          <Input
            label="promo code"
            className="max-w-none flex-1 rounded-r-[0px]"
          />
          <Button
            label="apply code"
            className="h-[38.6px] rounded-l-[0px] text-sm"
          />
        </div>

        <div className="flex justify-between pl-4">
          <label className="font-semibold" htmlFor="">
            Total
          </label>
          <p>R{total * 1.15}</p>
        </div>

        <hr className="my-3" />

        <p className="pl-4 text-lg font-bold">Payment</p>

        <Input
          label="card number"
          icon={<BsCreditCard2Back />}
          className="w-full max-w-none"
          type="number"
        />

        <div className="flex w-full gap-4">
          <Input
            label="expiry date"
            icon={<SlCalender />}
            type="date"
            wrapperClassName="flex-1"
          />
          <Input label="CVV" type="number" wrapperClassName="flex-1" />
        </div>

        <Button
          className="mt-2 w-full text-sm"
          label={`Pay R${total}`}
          disabled={total === 0}
        />
      </aside>
    </main>
  );
}

function CartItemComponent({ quantity, product }: CartItemType) {
  const Auth = useContext(UserContext);

  return (
    <div className="relative">
      <div className="flex w-full gap-2 rounded-lg py-4">
        {/* eslint-disable-next-line @next/next/no-img-element */}
        <img
          src={`${process.env.NEXT_PUBLIC_SERVER_URL}/assets/products/${product.productID}/${product.coverImage}`}
          alt={product.productName}
          width="175"
          className="rounded-lg"
        />

        {/* Product info */}
        <div className="flex flex-col justify-between">
          <div>
            <Link href={`/shop/${product.productID}`}>
              <span>{product.productName}</span>
            </Link>

            <p>Price: R{product.price}</p>

            <div className="flex items-end">
              <Button
                icon={<BiMinus />}
                onClick={() =>
                  Auth?.UpdateCartHandler(
                    product.productID,
                    quantity - 1,
                    false,
                  )
                }
                disabled={quantity === 1}
                className="h-[41.6px] rounded-r-[0]"
              />
              <Input
                label="quantity"
                value={quantity}
                className="w-[85px] rounded-l-[0] rounded-r-[0] px-2"
                readOnly
              />
              <Button
                icon={<BiPlus />}
                className="h-[41.6px] rounded-l-[0]"
                onClick={() =>
                  Auth?.UpdateCartHandler(
                    product.productID,
                    quantity + 1,
                    false,
                  )
                }
              />
            </div>

            <p>Total: R{product.price * quantity}</p>
          </div>

          <Button
            label="Save for later"
            icon={<BsBookmarks />}
            variant="flat"
            className="justify-start p-0 text-sm"
          />
        </div>
      </div>
      <Button
        onClick={() => Auth?.DeleteFromCartHandler(product.productID, false)}
        icon={<LiaTimesSolid />}
        className="absolute right-2 top-2"
        variant="flat"
        desctructive
      />
    </div>
  );
}
