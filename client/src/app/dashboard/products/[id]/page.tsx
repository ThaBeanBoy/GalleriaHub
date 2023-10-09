"use client";

import Image from "next/image";

import { UserContext } from "@/contexts/auth";
import { ProductType } from "@/lib/types";
import axios from "axios";
import { LucideLoader2 } from "lucide-react";
import { useContext, useEffect, useState } from "react";
import { FiTrash } from "react-icons/fi";
import { SlOptionsVertical } from "react-icons/sl";
import { BsCloudUpload } from "react-icons/bs";

import ReactQuill from "react-quill";
// import "react-quill/dist/quill.snow.css";
import "react-quill/dist/quill.bubble.css";

import * as Tabs from "@radix-ui/react-tabs";
import * as Dropdown from "@radix-ui/react-dropdown-menu";

import Input from "@/components/Input";
import Button from "@/components/Button";
import Switch from "@/components/Switch";
import Tooltip from "@/components/Tooltip";
import openFilePicker from "@/lib/filePicker";

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
        console.log(data);
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

  const tabs: {
    label: string;
    value: string;
  }[] = [
    { label: "information", value: "info" },
    { label: "assets", value: "assets" },
  ];

  return (
    <main>
      <Tabs.Root defaultValue="info">
        <Tabs.List className="mb-4 flex min-w-[720px] justify-between">
          <div className="flex">
            {tabs.map(({ label, value }) => (
              <Tabs.Trigger value={value} key={`tab-${value}`} asChild>
                <Button
                  label={label}
                  variant="flat"
                  className="hover:text-active-light data-[state=active]:text-active data-[state=active]:border-b-active border-b text-black"
                />
              </Tabs.Trigger>
            ))}
          </div>
          <div className="flex gap-2">
            <Tooltip
              trigger={<Button icon={<SlOptionsVertical />} variant="flat" />}
            >
              Information
              <Dropdown.Separator className="bg-grey m-[5px] h-[1px]" />
              <Button
                label="delete"
                icon={<FiTrash />}
                className="px-1 py-2"
                variant="flat"
                desctructive
              />
            </Tooltip>

            <Button label="Update" />
          </div>
        </Tabs.List>

        <Tabs.Content value="info">
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
            placeholder="Product Description"
            value={description}
            onChange={setdescription}
          />
        </Tabs.Content>

        <Tabs.TabsContent value="assets">
          <Button
            label="Upload Asset"
            icon={<BsCloudUpload />}
            onClick={async () => {
              const files = await openFilePicker();
              console.log(files);
            }}
          />

          <h5 className="tesxt-base font-semibold">Unuploaded Assets</h5>
          <h5 className="tesxt-base font-semibold">Assets</h5>

          {product.images.map((path, key) => (
            <img
              key={`product-image-${key}`}
              src={path}
              alt={`product-image-${key}`}
              width={500}
              height={500}
            />
          ))}
        </Tabs.TabsContent>
      </Tabs.Root>
    </main>
  );
}
