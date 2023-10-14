"use client";

import { UserContext } from "@/contexts/auth";
import axios from "axios";
import { useContext, useEffect, useState } from "react";

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

export default function Sales() {
  const Auth = useContext(UserContext);

  const [sales, setSales] = useState<any[]>([]);

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
          {sales.map(({ orderID, orderDate, buyer, product }, key) => (
            <SaleItem
              key={key}
              orderID={orderID}
              orderDate={orderDate}
              product={product}
              buyer={buyer}
            />
          ))}
        </TableBody>
      </Table>
    </main>
  );
}

function SaleItem({ orderID, orderDate, buyer, product }: any) {
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
        <Link href={`/users/${buyer.userID}`}>{buyer.username}</Link>
      </TableCell>
      <TableCell>
        <Link href={`/dashboard/products/${product.productID}`}>
          {product.productName}
        </Link>
      </TableCell>
    </TableRow>
  );
}
