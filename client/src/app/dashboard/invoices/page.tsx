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

export default function Invoices() {
  const Auth = useContext(UserContext);

  const [orders, setOrders] = useState<any[]>([]);

  useEffect(() => {
    axios({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/orders/`,
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
    })
      .then(({ data }) => {
        setOrders(data);
      })
      .catch((error) => console.log(error));
  }, [Auth?.auth?.jwt.token]);

  return (
    <main>
      <h1 className="mb-6 text-4xl font-bold">
        {Auth?.auth?.user.username}&apos;s Invoices
      </h1>
      <Button
        label="Filters"
        icon={<BiFilter />}
        variant="flat"
        className="py-0 pr-0 text-sm"
      />
      <Table className="mt-4">
        <TableCaption>Invoices</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead>Order ID</TableHead>
            <TableHead>Order Date</TableHead>
            <TableHead>Sub Total</TableHead>
            <TableHead>Tax</TableHead>
            <TableHead>Total</TableHead>
            <TableHead>Order PDF</TableHead>
          </TableRow>
        </TableHeader>

        <TableBody>
          {orders.map(({ orderID, orderDate, subTotal, tax, total }, key) => (
            <OrderItem
              key={key}
              orderID={orderID}
              orderDate={orderDate}
              subTotal={subTotal}
              tax={tax}
              total={total}
            />
          ))}
        </TableBody>
      </Table>
    </main>
  );
}

function OrderItem({ orderID, orderDate, subTotal, tax, total }: any) {
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
      <TableCell>{subTotal}</TableCell>
      <TableCell>%{tax}</TableCell>
      <TableCell>R{total}</TableCell>
      <TableCell>
        <Link href="/">Order PDF</Link>
      </TableCell>
    </TableRow>
  );
}
