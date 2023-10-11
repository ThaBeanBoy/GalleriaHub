import Button from "@/components/Button";
import Grid from "@/components/Grid";
import { ProductType } from "@/lib/types";
import axios from "axios";
import Link from "next/link";

import { BsCartPlus, BsPlus } from "react-icons/bs";

export default async function Shop() {
  const { data } = await axios<ProductType[]>({
    method: "get",
    url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/`,
  });

  return (
    <main>
      <Grid className="grid-4 group">
        {data.map(({ productID, productName, images, price }) => (
          <div key={productID} className="group relative">
            <Link
              href={`shop/${productID}`}
              className="group block h-full w-full rounded-lg bg-white drop-shadow"
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
          </div>
        ))}
      </Grid>
    </main>
  );
}
