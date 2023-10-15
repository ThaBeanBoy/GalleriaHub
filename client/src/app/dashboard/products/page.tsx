"use client";

import { useContext } from "react";
import StackGrid from "react-stack-grid";
import { DashboardProductsLayoutContext } from "./layout";
import ProductCard from "@/components/ProductCard";

export default function ProductEditor() {
  const DashboardProoducts = useContext(DashboardProductsLayoutContext);

  return (
    <main className="w-full flex-1">
      <StackGrid columnWidth={300 - 16} gutterWidth={16} gutterHeight={16}>
        {DashboardProoducts?.products?.map((product, key) => (
          <ProductCard
            key={key}
            {...product}
            href={`/dashboard/products/${product.productID}`}
            displaySeller={false}
            displayActions={false}
          />
        ))}
      </StackGrid>
    </main>
  );
}
