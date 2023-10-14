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
import Input, { useInput } from "@/components/Input";
import { formatDistance } from "date-fns";
import Link from "next/link";
import Button from "@/components/Button";

import { BiFilter } from "react-icons/bi";
import { SlCalender } from "react-icons/sl";
import Tooltip from "@/components/Tooltip";
import { LucideLoader2 } from "lucide-react";
import Menubar from "@/components/Menubar";

import { addDays, format } from "date-fns";
import { DateRange } from "react-day-picker";

import { cn } from "@/lib/utils";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";

export default function Sales() {
  const [date, setDate] = useState<DateRange | undefined>({
    from: new Date(2022, 0, 20),
    to: addDays(new Date(2022, 0, 20), 20),
  });

  const Auth = useContext(UserContext);

  const [sales, setSales] = useState<any[] | undefined>(undefined);

  const OrderIDFilterField = useInput();

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

      <Menubar
        trigger={
          <Button
            label="Filters"
            icon={<BiFilter />}
            variant="flat"
            className="py-0 pr-0 text-sm"
          />
        }
        className="flex flex-col gap-3 text-xs"
      >
        <div className="flex items-end gap-2">
          <label htmlFor="OrderID-filter" className="pb-3">
            Order ID
          </label>
          <Input
            id="OrderID-filter"
            placeholder="order id"
            type="number"
            className="rounded-none border-0 border-b px-0 text-sm"
            {...OrderIDFilterField.inputProps}
          />
        </div>

        <div className="flex items-end gap-2">
          <label htmlFor="DatesFilter" className="pb-2">
            Dates
          </label>
          <Popover>
            <PopoverTrigger asChild>
              <Button
                id="DatesFilter"
                label={
                  date?.from ? (
                    date.to ? (
                      <>
                        {format(date.from, "LLL dd, y")} -{" "}
                        {format(date.to, "LLL dd, y")}
                      </>
                    ) : (
                      format(date.from, "LLL dd, y")
                    )
                  ) : (
                    <span>Pick a date</span>
                  )
                }
                variant="flat"
                className={cn(
                  "justify-start text-left text-xs font-normal",
                  !date && "text-muted-foreground",
                )}
              >
                hello
              </Button>
            </PopoverTrigger>

            <PopoverContent className="w-auto p-0" side="right" align="start">
              <Calendar
                initialFocus
                mode="range"
                defaultMonth={date?.from}
                selected={date}
                onSelect={setDate}
                numberOfMonths={2}
              />
            </PopoverContent>
          </Popover>
        </div>

        <div className="flex items-end gap-2">
          <label htmlFor="User-filter" className="pb-3">
            User
          </label>
          <Input
            id="User-filter"
            placeholder="Username or Email"
            type="number"
            className="rounded-none border-0 border-b px-0 text-sm"
          />
        </div>

        <div className="flex items-end gap-2">
          <label htmlFor="User-filter" className="pb-3">
            Product
          </label>
          <Input
            id="User-filter"
            placeholder="Product Name or ID"
            type="number"
            className="rounded-none border-0 border-b px-0 text-sm"
          />
        </div>

        <div className="flex items-end gap-2">
          <label className="pb-3">Price</label>
          <Input
            placeholder="Minimum"
            type="number"
            className="rounded-none border-0 border-b px-0 text-sm"
          />
          <Input
            id="User-filter"
            placeholder="Maximum"
            type="number"
            className="rounded-none border-0 border-b px-0 text-sm"
          />
        </div>
      </Menubar>

      <Table className="mt-4">
        <TableCaption>Sales</TableCaption>
        <TableHeader className="sticky top-4">
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
          {sales
            .filter((OrderItem) =>
              // OrderIDFilterField.value.trim() === "" ||
              {
                console.log(OrderIDFilterField.value, OrderItem.orderID);
                if (OrderIDFilterField.value.trim() === "") return true;
                return OrderItem.orderID === Number(OrderIDFilterField.value);
              },
            )
            .map(({ orderID, orderDate, quantity, buyer, product }, key) => (
              <SaleItem
                key={`${orderID}`}
                orderID={orderID}
                orderDate={orderDate}
                product={product}
                buyer={buyer}
                price={product.price}
                quantity={quantity}
              />
            ))}
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
