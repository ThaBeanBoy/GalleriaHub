"use client";

import axios from "axios";

import * as Dialog from "@radix-ui/react-dialog";

import Form from "@/components/Form";
import Button from "@/components/Button";
import Input from "@/components/Input";
import { FormEvent, useContext, useEffect, useRef, useState } from "react";
import { UserContext } from "@/contexts/auth";
import useProtectPage from "@/lib/protectPage";
import { ErrorDialog } from "@/components/dialogs";

import { BsPlus } from "react-icons/bs";

export default /* async */ function Products({
  children,
}: {
  children: React.ReactNode;
}) {
  const Auth = useContext(UserContext);

  const TitleRef = useRef<HTMLInputElement>(null);
  const PriceRef = useRef<HTMLInputElement>(null);
  const StockRef = useRef<HTMLInputElement>(null);

  const [newProductSubmissionError, setNewProductSubmissionError] =
    useState(false);
  const [submissionErrorMessage, setsubmissionErrorMessage] = useState<
    string | undefined
  >(undefined);

  const handleNewProduct = async (e: FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();

      const ProductData = new FormData();
      ProductData.append("name", TitleRef.current?.value || "");
      ProductData.append("price", PriceRef.current?.value || "");
      ProductData.append("stock", StockRef.current?.value || "");

      console.log("my auth:", Auth);

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/new-product`,
        data: ProductData,
        headers: {
          Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
        },
      });

      console.log(data);
    } catch (error: any) {
      console.log(error);
      setsubmissionErrorMessage(error.response.data);
      setNewProductSubmissionError(true);
    }
  };

  useEffect(() => {
    axios({
      method: "get",
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
      params: {
        userid: Auth?.auth?.user.userID,
      },
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/`,
    })
      .then(({ data }) => console.log(data))
      .catch((error) => console.log(error));
  }, []);

  return (
    <div>
      <h2 className="mb-5 text-3xl font-bold">Product Management</h2>

      <div className="flex gap-4">
        <aside>
          <div id="top" className="flex items-end gap-2">
            <Input label="Search" />
            <Dialog.Root>
              <Dialog.Trigger asChild>
                <Button icon={<BsPlus />} />
              </Dialog.Trigger>

              <Dialog.Portal>
                <Dialog.Overlay className="data-[state=open]:animate-overlayShow fixed inset-0 bg-black opacity-60" />

                <Dialog.Content className="data-[state=open]:animate-contentShow fixed left-[50%] top-[50%] max-h-[85vh] w-[90vw] max-w-[450px] translate-x-[-50%] translate-y-[-50%] rounded-3xl bg-white p-[25px] shadow-lg">
                  <h2 className="text-xl font-semibold">New Product</h2>

                  <hr className="my-2" />

                  <Form onSubmit={handleNewProduct}>
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
                        <Button
                          label="Cancel"
                          variant="hollow"
                          className="flex-1"
                        />
                      </Dialog.Close>

                      <Button
                        label="Make Product"
                        className="flex-1"
                        type="submit"
                      />
                      {/* <ErrorDialog
                    trigger={
                    }
                    title="New Product"
                    message={submissionErrorMessage || ""}
                  /> */}
                    </div>
                  </Form>
                </Dialog.Content>
              </Dialog.Portal>
            </Dialog.Root>
          </div>
        </aside>

        <div>{children}</div>
      </div>
    </div>
  );
}
