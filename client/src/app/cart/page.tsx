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
  }, [Auth]);

  if (Auth == null) return "please login";

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
      <aside className="flex w-full max-w-[400px] flex-col gap-2 border-l-2 pl-2 text-sm">
        <p className="pl-4 text-lg font-bold">Order Summary</p>

        <div className="flex items-end">
          <Input label="promo code" className="max-w-none rounded-r-[0px]" />
          <Button
            label="apply code"
            className="h-[38.6px] rounded-l-[0px] text-sm"
          />
        </div>

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

        <div className="flex gap-4">
          <Input label="expiry date" icon={<SlCalender />} type="number" />
          <Input label="CVV" type="number" />
        </div>

        <Button className="w-full text-sm" label={`Pay R${total}`} />
      </aside>
    </main>
  );
}

function CartItemComponent({ quantity, product }: CartItemType) {
  const Auth = useContext(UserContext);
  return (
    <div className="relative">
      <div className="block w-full rounded-lg p-4">
        <Link href={`/shop/${product.productID}`}>
          <span>{product.productName}</span>
        </Link>
        <Input label="quantity" type="number" defaultValue={quantity} />
      </div>
      <Button
        onClick={() => Auth?.DeleteFromCartHandler(product.productID, false)}
        icon={<LiaTimesSolid />}
        className="absolute right-2 top-2"
        variant="flat"
        desctructive
      />

      <Button
        label="Save for later"
        icon={<BsBookmarks />}
        variant="flat"
        className="text-sm"
      />
    </div>
  );
}
