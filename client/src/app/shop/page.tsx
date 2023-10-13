"use client";

import Button from "@/components/Button";
import Grid, { ProductGrid } from "@/components/Grid";
import ProductCard from "@/components/ProductCard";
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
        {data.map((product, key) => (
          <ProductCard key={key} {...product} />
        ))}
      </Grid>
    </main>
  );
}
