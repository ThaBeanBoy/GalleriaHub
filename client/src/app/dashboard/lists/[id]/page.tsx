"use client";

import Button from "@/components/Button";
import Grid, { ProductGrid } from "@/components/Grid";
import ProductCard from "@/components/ProductCard";
import { UserContext } from "@/contexts/auth";
import { ListType, ProductType } from "@/lib/types";
import axios from "axios";
import { LucideLoader2 } from "lucide-react";
import Link from "next/link";
import { useContext, useEffect, useState } from "react";
import { FiTrash } from "react-icons/fi";
import { SlOptionsVertical } from "react-icons/sl";
import StackGrid from "react-stack-grid";

export default function ListPage({ params }: { params: { id: number } }) {
  const Auth = useContext(UserContext);
  const [list, setList] = useState<ListType | undefined>(undefined);

  useEffect(() => {
    if (Auth?.auth) {
      axios<ListType>({
        method: "get",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/lists/${params.id}`,
        headers: {
          Authorization: `Bearer ${Auth.auth.jwt.token}`,
        },
      })
        .then(({ data }) => {
          data.createdOn = new Date(data.createdOn);
          data.lastUpdate = new Date(data.lastUpdate);

          setList(data);
        })
        .catch((error) => {
          console.log(error);
        });
    }
  }, [Auth?.auth?.jwt.token, params.id]);

  if (!list) {
    return <LucideLoader2 className="animate-spin" />;
  }

  if (list.items.length === 0) {
    return (
      <p>
        Empty list, shop for some products <Link href="/shop">here</Link>
      </p>
    );
  }

  return (
    <StackGrid columnWidth={300 - 16} gutterWidth={16} gutterHeight={16}>
      {list.items.map((product, key) => (
        <ProductCard key={key} {...product} />
      ))}
    </StackGrid>
  );
}
