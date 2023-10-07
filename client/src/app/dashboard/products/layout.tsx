"use client";

import axios from "axios";

import Link from "next/link";

import * as Dialog from "@radix-ui/react-dialog";
import { useToast } from "@/components/ui/use-toast";

import { formatDistance, subDays } from "date-fns";

import Form from "@/components/Form";
import Button from "@/components/Button";
import Input from "@/components/Input";
import {
  FormEvent,
  useContext,
  useEffect,
  useRef,
  useState,
  useTransition,
} from "react";
import { UserContext } from "@/contexts/auth";
import useProtectPage from "@/lib/protectPage";
import { ErrorDialog } from "@/components/dialogs";

import { BsPlus } from "react-icons/bs";
import { ProductType } from "@/lib/types";
import { EyeIcon, EyeOff, LucideLoader2 } from "lucide-react";
import { usePathname } from "next/navigation";
import { cn } from "@/lib/utils";

export default /* async */ function Products({
  children,
}: {
  children: React.ReactNode;
}) {
  const { toast } = useToast();

  const Auth = useContext(UserContext);

  const TitleRef = useRef<HTMLInputElement>(null);
  const PriceRef = useRef<HTMLInputElement>(null);
  const StockRef = useRef<HTMLInputElement>(null);

  const [products, setProducts] = useState<ProductType[] | undefined>(
    undefined,
  );

  const [newProductSubmissionError, setNewProductSubmissionError] =
    useState(false);
  const [submissionErrorMessage, setsubmissionErrorMessage] = useState<
    string | undefined
  >(undefined);

  const handleNewProduct = async (e: FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();

      if (!products) {
        toast({
          title: "Products",
          description: "Still loading products",
          variant: "destructive",
        });
        return;
      }

      const ProductData = new FormData();
      ProductData.append("name", TitleRef.current?.value || "");
      ProductData.append("price", PriceRef.current?.value || "");
      ProductData.append("stock", StockRef.current?.value || "");

      console.log("my auth:", Auth);

      const { data } = await axios<ProductType>({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/new-product`,
        data: ProductData,
        headers: {
          Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
        },
      });

      // Cleaning up dates
      data.createdOn = new Date(data.createdOn);
      data.lastUpdate = new Date(data.lastUpdate);

      // Add product to list of products
      setProducts([...products, data]);

      // toast the product & visit button
      toast({
        title: "New Product",
        description: `${data.productName} has been created`,
        action: (
          <Link href={`/dashboard/products/${data.productID}`}>View Now</Link>
        ),
      });
    } catch (error: any) {
      console.log(error);
      setsubmissionErrorMessage(error.response.data);
      setNewProductSubmissionError(true);
    }
  };

  useEffect(() => {
    axios<ProductType[]>({
      method: "get",
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
      params: {
        userid: Auth?.auth?.user.userID,
      },
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/`,
    })
      .then(({ data }) => {
        data = data.map((product) => {
          const temp = product;

          temp.createdOn = new Date(temp.createdOn);
          temp.lastUpdate = new Date(temp.lastUpdate);

          return temp;
        });

        setProducts(data);
      })
      .catch((error) => console.log(error));
  }, []);

  return (
    <div className="flex gap-4">
      <aside className="sticky top-3 resize-x">
        <div id="top" className="mb-4 flex items-end gap-2">
          <Input placeholder="search" className="w-[250px] flex-1" />
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

        {/* User products */}
        <ul>
          {products ? (
            products.map((product, key) => (
              <li key={key}>
                <ProductItem {...product} />
              </li>
            ))
          ) : (
            <p className="flex items-center gap-2">
              <LucideLoader2 className="animate-spin" />
              <span>Loading</span>
            </p>
          )}
        </ul>
      </aside>

      <div>{children}</div>
    </div>
  );
}

function ProductItem({
  productID,
  productName,
  lastUpdate,
  createdOn,
}: ProductType) {
  const href = `/dashboard/products/${productID}`;
  const active = href === usePathname();

  const dateLabel =
    lastUpdate.getTime() === createdOn.getTime() ? "Created" : "Last updated";

  const [dateDisplayed, setdateDisplayed] = useState(
    formatDistance(lastUpdate, new Date(), {
      addSuffix: true,
    }),
  );

  setInterval(
    () =>
      setdateDisplayed(
        formatDistance(lastUpdate, new Date(), {
          addSuffix: true,
        }),
      ),
    1000,
  );

  return (
    <Link
      className={cn(
        "hover:text-active-light block rounded-xl rounded-r-none border-r-2 px-4 py-3 text-black",
        { "text-active border-2 border-r-0": active },
      )}
      href={href}
    >
      <h3 className="mb-1 font-semibold">{productName}</h3>
      <p className="text-xs">
        {dateLabel} {dateDisplayed}
      </p>
    </Link>
  );
}
