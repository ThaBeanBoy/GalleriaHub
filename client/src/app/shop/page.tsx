"use client";

import Button from "@/components/Button";
import Grid, { ProductGrid } from "@/components/Grid";
import ProductCard from "@/components/ProductCard";
import { ProductType } from "@/lib/types";
import axios from "axios";
import Link from "next/link";
import { useEffect, useState } from "react";

import StackGrid from "react-stack-grid";

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
      <StackGrid columnWidth={300 - 16} gutterWidth={16} gutterHeight={16}>
        {products.map((product, key) => (
          <ProductCard key={key} {...product} />
        ))}
      </StackGrid>
      {/* <Grid className="grid-4 group">
      </Grid> */}
    </main>
  );
}
