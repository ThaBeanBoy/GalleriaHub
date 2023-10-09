"use client";

import Image from "next/image";

import { useRouter } from "next/navigation";

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

import { useToast } from "@/components/ui/use-toast";

import * as Tabs from "@radix-ui/react-tabs";
import * as Dropdown from "@radix-ui/react-dropdown-menu";

import Input from "@/components/Input";
import Button from "@/components/Button";
import Switch from "@/components/Switch";
import Tooltip from "@/components/Tooltip";
import openFilePicker from "@/lib/filePicker";
import { cn } from "@/lib/utils";
import { redirect } from "next/navigation";

export default function ProductEditorPage({
  params,
}: {
  params: { id: number };
}) {
  const router = useRouter();
  const Auth = useContext(UserContext);

  const [product, setProduct] = useState<ProductType | undefined | string>(
    undefined,
  );
  const [ununploadedAssets, setUnunploadedAssets] = useState<File[]>([]);

  const [description, setdescription] = useState("");

  const { toast } = useToast();

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
  }, [Auth?.auth?.jwt.token, params.id]);

  const handleUpdate = async () => {
    // Updating images
    const imagesFormData = new FormData();
    ununploadedAssets.forEach((file, fileNr) => {
      imagesFormData.append(`file${fileNr}`, file);
    });

    if (ununploadedAssets.length > 0) {
      try {
        toast({ title: "Asset Upload", description: "Uploading assets" });

        const { data } = await axios<ProductType>({
          method: "post",
          url: `${process.env.NEXT_PUBLIC_SERVER_URL}/assets/products/${params.id}`,
          headers: {
            "Content-Type": "multipart/form-data",
            Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
          },
          data: imagesFormData,
        });

        toast({
          title: "Asset Upload",
          description: "Successfully uploaded assets",
        });

        // setting the dates
        data.createdOn = new Date(data.createdOn);
        data.lastUpdate = new Date(data.lastUpdate);

        setProduct(data);

        setUnunploadedAssets([]);
      } catch (error: any) {
        toast({
          title: "Asset Upload",
          description: error.response.data,
          variant: "destructive",
        });
        console.log(error.response);
      }
    }
  };

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

  const handleDelete = async () => {
    const ToastTitle = "Product Delete";

    try {
      toast({
        title: ToastTitle,
        description: `Attempting to delete ${product.productName}`,
      });

      const { data } = await axios({
        method: "delete",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/Products/${params.id}`,
        headers: {
          Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
        },
      });

      console.log(data);

      toast({
        title: ToastTitle,
        description: "Succesfully deleted",
      });

      // redirect user
      router.back();
    } catch (error: any) {
      console.log(error);
      toast({
        title: ToastTitle,
        description: "Something went wrong",
        variant: "destructive",
      });
    }
  };

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
              <Button
                label="delete"
                icon={<FiTrash />}
                className="p-0"
                variant="flat"
                desctructive
                onClick={handleDelete}
              />
            </Tooltip>

            <Button label="Update" onClick={handleUpdate} />
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
            className="mb-4"
            icon={<BsCloudUpload />}
            onClick={async () => {
              const files = await openFilePicker();
              setUnunploadedAssets([...ununploadedAssets, ...files]);
            }}
          />

          {ununploadedAssets.length > 0 && (
            <h5 className="tesxt-base font-semibold">Unuploaded Assets</h5>
          )}
          {ununploadedAssets.map((file, key) => (
            <img
              key={`unuploaded-${key}`}
              src={URL.createObjectURL(file)}
              alt="unuploaded"
              width="500"
              height="500"
            />
          ))}

          {ununploadedAssets.length > 0 && (
            <h5 className="tesxt-base font-semibold">Assets</h5>
          )}
          {product.images.map((path) => (
            <ProductImage
              key={`product-image-${path}`}
              productID={params.id}
              src={path}
              afterDelete={(updatedProduct) => setProduct(updatedProduct)}
            />
          ))}
        </Tabs.TabsContent>
      </Tabs.Root>
    </main>
  );
}

function ProductImage({
  productID,
  src,
  afterDelete,
}: {
  productID: number;
  src: string;
  afterDelete?: (arg: ProductType) => void;
}) {
  const Auth = useContext(UserContext);
  const { toast } = useToast();
  const ToastTitle = "Asset Delete";

  const [deleting, setDeleting] = useState(false);

  const deleteImage = async () => {
    try {
      const pathContent = src.split("/");
      setDeleting(true);
      toast({
        title: ToastTitle,
        description: "Attempting to delete asset",
      });

      const { data } = await axios<ProductType>({
        method: "delete",
        url: `${
          process.env.NEXT_PUBLIC_SERVER_URL
        }/assets/products/${productID}/${pathContent[pathContent.length - 1]}`,
        headers: {
          Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
        },
      });

      toast({
        title: ToastTitle,
        description: "Successfully deleted asset",
      });

      if (afterDelete) {
        data.createdOn = new Date(data.createdOn);
        data.lastUpdate = new Date(data.lastUpdate);
        afterDelete(data);
      }
    } catch (error: any) {
      console.log(error);

      setDeleting(false);
      toast({
        title: ToastTitle,
        description: "Something went wrong",
        variant: "destructive",
      });
    }
  };

  return (
    <div
      className={cn("relative my-3 first:mt-0 last:mb-0", {
        "brightness-75": deleting,
      })}
    >
      {/* Delete button */}
      <Tooltip
        trigger={
          <Button
            icon={<SlOptionsVertical />}
            className="absolute left-3 top-3"
            variant="hollow"
          />
        }
      >
        <Button
          label="delete"
          icon={<FiTrash />}
          variant="flat"
          className="p-0"
          onClick={deleteImage}
          desctructive
        />
      </Tooltip>

      {/* Image */}
      <img src={src} alt={src} width={500} height={500} />
    </div>
  );
}
