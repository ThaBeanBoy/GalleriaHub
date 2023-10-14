"use client";

import { UserContext } from "@/contexts/auth";
import axios from "axios";
import { useContext, useEffect, useState } from "react";

import Avatar from "@/components/Avatar";

import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import Input from "@/components/Input";
import { formatDistance } from "date-fns";
import Link from "next/link";
import Button from "@/components/Button";

import { BiFilter } from "react-icons/bi";
import { SlCalender } from "react-icons/sl";
import Tooltip from "@/components/Tooltip";
import { LucideLoader2 } from "lucide-react";

export default function Sales() {
  const Auth = useContext(UserContext);

  const [sales, setSales] = useState<any[] | undefined>(undefined);

  useEffect(() => {
    axios({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/sales/`,
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
    })
      .then(({ data }) => {
        setSales(data);
        console.log(data);
      })
      .catch((error) => console.log(error));
  }, [Auth?.auth?.jwt.token]);

  if (!sales) return <LucideLoader2 className="animate-spin" />;

  return (
    <main>
      <h1 className="mb-6 text-4xl font-bold">
        {Auth?.auth?.user.username}&apos;s Sales
      </h1>

      <Button
        label="Filters"
        icon={<BiFilter />}
        variant="flat"
        className="py-0 pr-0 text-sm"
      />

      <Table className="mt-4">
        <TableCaption>Sales</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead>Order ID</TableHead>
            <TableHead className="flex items-center gap-2">
              <SlCalender />
              <span>Order Date</span>
            </TableHead>
            <TableHead>Buyer</TableHead>
            <TableHead>Product</TableHead>
            <TableHead>Price</TableHead>
            <TableHead>Quantity</TableHead>
            <TableHead>Total</TableHead>
            {/* <TableHead>Status</TableHead> */}
          </TableRow>
        </TableHeader>

        <TableBody>
          {sales.map(
            ({ orderID, orderDate, quantity, buyer, product }, key) => (
              <SaleItem
                key={key}
                orderID={orderID}
                orderDate={orderDate}
                product={product}
                buyer={buyer}
                price={product.price}
                quantity={quantity}
              />
            ),
          )}
        </TableBody>
      </Table>
    </main>
  );
}

function SaleItem({
  orderID,
  orderDate,
  buyer,
  product,
  price,
  quantity,
}: any) {
  orderDate = new Date(orderDate);

  const [date, setDate] = useState(
    formatDistance(orderDate, new Date(), { addSuffix: true }),
  );

  setInterval(() => {
    setDate(formatDistance(orderDate, new Date(), { addSuffix: true }));
  }, 1000);

  return (
    <TableRow>
      <TableCell>{orderID}</TableCell>
      <TableCell>{date}</TableCell>
      <TableCell>
        <Link
          className="flex items-center gap-2"
          href={`/users/${buyer.userID}`}
        >
          <Avatar src="" alt={product.productName} fallback="0" />
          <span>{buyer.username}</span>
        </Link>
      </TableCell>
      <TableCell>
        <Tooltip
          trigger={
            <Link href={`/dashboard/products/${product.productID}`}>
              {product.productName}
            </Link>
          }
          side="right"
        >
          {/* eslint-disable-next-line @next/next/no-img-element */}
          <img
            src={`${process.env.NEXT_PUBLIC_SERVER_URL}/assets/products/${product.productID}/${product.displayImage}`}
            alt={product.productName}
            width={200}
            className="rounded-md"
          />
        </Tooltip>
      </TableCell>

      <TableCell>R{price}</TableCell>
      <TableCell>{quantity}</TableCell>
      <TableCell>R{price * quantity}</TableCell>
    </TableRow>
  );
}
