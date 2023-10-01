"use client";

import axios from "axios";

import * as Dialog from "@radix-ui/react-dialog";

import Form from "@/components/Form";
import Button from "@/components/Button";
import Input from "@/components/Input";
import { useRef } from "react";

export default /* async */ function Products() {
  // await axios({
  //   method: 'get',
  //   url: '',
  //   params: {
  //     artistid: ''
  //   }
  // });

  const TitleRef = useRef<HTMLInputElement>(null);
  const PriceRef = useRef<HTMLInputElement>(null);
  const StockRef = useRef<HTMLInputElement>(null);

  const handleNewProduct = async () => {
    const ProductData = new FormData();
    ProductData.append("title", TitleRef.current?.value || "");
    ProductData.append("price", PriceRef.current?.value || "");
    ProductData.append("title", StockRef.current?.value || "");

    const { data } = await axios({
      method: "get",
      url: "",
      data: ProductData,
    });

    console.log(data);
  };

  return (
    <main>
      <h2 className="text-3xl font-bold">Product Management</h2>
      <Dialog.Root>
        <Dialog.Trigger asChild>
          <Button label="new product" />
        </Dialog.Trigger>

        <Dialog.Portal>
          <Dialog.Overlay className="data-[state=open]:animate-overlayShow fixed inset-0 bg-black opacity-60" />

          <Dialog.Content className="data-[state=open]:animate-contentShow fixed left-[50%] top-[50%] max-h-[85vh] w-[90vw] max-w-[450px] translate-x-[-50%] translate-y-[-50%] rounded-3xl bg-white p-[25px] shadow-lg">
            <h2 className="text-xl font-semibold">New Product</h2>

            <hr className="my-2" />

            <Form>
              <Input
                label="product title"
                wrapperClassName="col-span-2"
                ref={TitleRef}
              />
              <Input
                label="price"
                className="mb-3"
                type="number"
                ref={PriceRef}
              />
              <Input
                label="stock"
                className="mb-3"
                type="number"
                ref={StockRef}
              />

              <div className="col-span-2 flex gap-2">
                <Dialog.Close asChild>
                  <Button label="Cancel" variant="hollow" className="flex-1" />
                </Dialog.Close>

                <Button
                  label="Make Product"
                  className="flex-1"
                  onClick={handleNewProduct}
                />
              </div>
            </Form>
          </Dialog.Content>
        </Dialog.Portal>
      </Dialog.Root>
    </main>
  );
}
