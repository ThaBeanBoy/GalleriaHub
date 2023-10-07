"use client";

import { UserContext } from "@/contexts/auth";
import { ProductType } from "@/lib/types";
import axios from "axios";
import { LucideLoader2 } from "lucide-react";
import { useContext, useEffect, useState } from "react";

import ReactQuill from "react-quill";
// import "react-quill/dist/quill.snow.css";
import "react-quill/dist/quill.bubble.css";

import Input from "@/components/Input";
import Button from "@/components/Button";
import Switch from "@/components/Switch";

export default function ProductEditorPage({
  params,
}: {
  params: { id: number };
}) {
  const Auth = useContext(UserContext);

  const [product, setProduct] = useState<ProductType | undefined | string>(
    undefined,
  );

  const [description, setdescription] = useState("");

  useEffect(() => {
    axios<ProductType>({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/${params.id}`,
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
    })
      .then(({ data }) => {
        data.createdOn = new Date(data.createdOn);
        data.lastUpdate = new Date(data.lastUpdate);
        setProduct(data);
      })
      .catch((error: any) => setProduct(error.response.data));
  }, []);

  if (!product) {
    // Return loading screen
    return (
      <main>
        <LucideLoader2 className="animate-spin" />
      </main>
    );
  }

  if (typeof product === "string") {
    return (
      <main className="prose">
        <h1>Error: {product}</h1>
      </main>
    );
  }

  return (
    <main>
      <Button label="Update" />
      <Input
        label="Product Name"
        className="focus:border-b-active mb-4 mt-0 w-full max-w-none rounded-none border-x-0 border-b-2 border-t-0 bg-white px-0 py-2 text-5xl font-bold outline-none"
        type="text"
        defaultValue={product.productName}
      />
      <div className="mb-4 flex gap-3">
        <Input label="price" type="number" defaultValue={product.price} />
        <Input
          label="stock"
          type="number"
          defaultValue={product.stockQuantity}
        />
        <Switch label="Public" className="flex-col text-sm" />
      </div>

      <label className="mb-4 pl-4 text-xs">Description</label>
      <ReactQuill
        theme="bubble"
        style={{
          fontSize: 16,
          minHeight: 500,
        }}
        placeholder="Product Description"
        value={description}
        onChange={setdescription}
      />
    </main>
  );
}
