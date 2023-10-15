import Button from "@/components/Button";
import { ProductType } from "@/lib/types";
import axios from "axios";
import Link from "next/link";

import { BsBookmarks, BsCart2 } from "react-icons/bs";
import { AiOutlinePlus } from "react-icons/ai";
import { useContext, useEffect, useState } from "react";
import { UserContext } from "@/contexts/auth";
import { ProductActions } from "@/components/ProductCard";

export default async function ProductPage({
  params,
}: {
  params: { id: number };
}) {
  // const Auth = useContext(UserContext);

  // const [inCart, setIntCart] = useState(false);

  // useEffect(() => {
  //   if (Auth) {
  //     console.log(Auth?.cart);
  //     setIntCart(
  //       Auth?.cart.some(
  //         (CartItem) => CartItem.product.productID === params.id,
  //       ) || false,
  //     );
  //   }
  // }, [Auth, Auth?.cart, params.id]);

  const { data } = await axios<ProductType>({
    method: "get",
    url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/${params.id}`,
  });

  return (
    <main className="flex gap-4">
      {/* eslint-disable-next-line @next/next/no-img-element */}
      <img
        src={data.images.length > 0 ? data.images[0] : ""}
        alt={`${data.productName} image`}
        className="sticky top-4 h-min max-w-[582px] rounded-2xl drop-shadow-lg"
      />

      {/* Product info */}
      <div>
        <h1 className="mb-2 text-5xl font-extrabold">{data.productName}</h1>

        <Link className="mb-6 block" href={`users/${data.seller.userID}`}>
          {data.seller.username}
        </Link>

        <div className="flex items-center gap-4">
          <h2 className="text-3xl font-bold">R{data.price}</h2>

          <ProductActions {...data} />
        </div>

        {/* Categories */}
        <h3 className="mb-2 font-semibold">Categories</h3>
        <div className="mb-6">
          <span className="bg-active-light rounded-full px-2 py-1 text-xs font-semibold text-white">
            Category
          </span>
        </div>

        <h3 className="font-semibold">Description</h3>
        <hr className="my-2" />
        <div
          id="product-description"
          dangerouslySetInnerHTML={{ __html: data.description || "" }}
        />
      </div>
    </main>
  );
}
