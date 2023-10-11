import Button from "@/components/Button";
import Grid, { ProductGrid } from "@/components/Grid";
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
      <ProductGrid products={data} />
    </main>
  );
}
