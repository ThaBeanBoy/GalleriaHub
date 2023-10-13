"use client";

import Button from "@/components/Button";
import Grid, { ProductGrid } from "@/components/Grid";
import ProductCard from "@/components/ProductCard";
import { ProductType } from "@/lib/types";
import axios from "axios";
import Link from "next/link";
import { useEffect, useState } from "react";

import { BsCartPlus, BsPlus } from "react-icons/bs";

export default function Shop() {
  const [products, setProducts] = useState<ProductType[]>([]);

  useEffect(() => {
    axios<ProductType[]>({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/`,
    }).then(({ data }) => {
      setProducts(data);
    });
  }, []);

  return (
    <main>
      <Grid className="grid-4 group">
        {products.map((product, key) => (
          <ProductCard key={key} {...product} />
        ))}
      </Grid>
    </main>
  );
}
