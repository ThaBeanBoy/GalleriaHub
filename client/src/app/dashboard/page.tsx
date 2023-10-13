"use client";

import Button from "@/components/Button";
import Input from "@/components/Input";
import { UserContext } from "@/contexts/auth";
import axios from "axios";
import { useContext, useEffect, useState } from "react";

export default function Cart() {
  const Auth = useContext(UserContext);

  const [cart, setCart] = useState<any[]>([]);

  useEffect(() => {
    if (Auth?.auth?.user.cart) {
      console.log(Auth.auth.user);
      setCart(Auth?.auth?.user.cart);
    }
  }, [Auth?.auth?.jwt.token, Auth?.auth?.user]);

  if (!Auth?.auth) return "Loading";

  return (
    <main>
      <Button label="Buy" />

      {Auth.auth.user.cart.map((item, key) => (
        <OrderItem
          key={key}
          name={item.product.productName}
          price={item.product.price}
          quantity={item.quantity}
        />
      ))}
    </main>
  );
}

function OrderItem({
  name,
  price,
  quantity,
}: {
  name: string;
  price: number;
  quantity: number;
}) {
  useEffect(() => {
    axios({
      method: "get",
      url: ``,
    });
  });

  return (
    <div className="rounded-lg p-4 shadow-md">
      <span>{name}</span>
      <Input defaultValue={quantity} />
      <Button label="update" />
      <Button label="Remove" variant="flat" desctructive />
      <p className="font-semibold">R{price}</p>
    </div>
  );
}
