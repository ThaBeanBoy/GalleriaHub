import { ProductType } from "@/lib/types";
import Link from "next/link";
import Button from "./Button";
import { BsCartPlus, BsPlus } from "react-icons/bs";
import { ReactNode } from "react";
import Tooltip from "./Tooltip";
import { SlOptionsVertical } from "react-icons/sl";

export default function ProductCard({
  productID,
  productName,
  images,
  price,
  tooltip,
}: { tooltip?: ReactNode } & ProductType) {
  return (
    <div key={productID} className="group relative">
      <Link
        href={`/shop/${productID}`}
        className="group flex h-full w-full flex-col justify-between rounded-lg bg-white pb-2 drop-shadow"
      >
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
      <div className="absolute left-[50%] top-[50%] hidden w-[90%] translate-x-[-50%] translate-y-[-50%] gap-2 rounded-xl bg-white px-3 py-2 drop-shadow-xl group-hover:flex">
        <Button
          // label="Add to cart"
          className="p-0 text-sm"
          label="Add to cart"
          icon={<BsCartPlus />}
          variant="flat"
        />

        <Button icon={<BsPlus />} variant="flat" />
      </div>

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
