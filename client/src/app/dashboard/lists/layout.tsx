"use client";

import Button from "@/components/Button";
import Form from "@/components/Form";
import Input, { useInput } from "@/components/Input";
import Menubar from "@/components/Menubar";
import { DialogClose, useDialog } from "@/contexts/Dialog";
import { UserContext } from "@/contexts/auth";
import { ListType } from "@/lib/types";
import { cn } from "@/lib/utils";
import * as Dialog from "@radix-ui/react-dialog";
import axios from "axios";
import { LucideLoader2 } from "lucide-react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { BsPlus } from "react-icons/bs";
import { FiTrash } from "react-icons/fi";
import { SlOptionsVertical } from "react-icons/sl";

export default function ListsLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const path = usePathname();
  const searchField = useInput();
  const Auth = useContext(UserContext);

  const [lists, setlists] = useState<ListType[] | undefined>(undefined);

  useEffect(() => {
    axios<ListType[]>({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/lists/`,
      headers: {
        Authorization: `Bearer ${Auth?.auth?.jwt.token}`,
      },
    })
      .then(({ data }) => {
        const lists = data.map((data) => {
          data.createdOn = new Date(data.createdOn);
          data.lastUpdate = new Date(data.lastUpdate);

          data.items = data.items.map((item) => {
            (item.createdOn = new Date(item.createdOn)),
              (item.lastUpdate = new Date(item.lastUpdate));

            return item;
          });

          return data;
        });

        setlists(lists);
      })
      .catch((error) => console.log(error));
  }, [Auth?.auth?.jwt.token]);

  const newListHandler = useNewListDailog();

  return (
    <div className="flex gap-4">
      <aside>
        <div id="top" className="mb-4 flex items-end gap-2">
          <Input
            placeholder="search"
            className="w-[250px] flex-1"
            {...searchField.inputProps}
          />

          <Button icon={<BsPlus />} onClick={newListHandler} />
        </div>

        <ul>
          {lists ? (
            lists
              .filter(
                (list) =>
                  searchField.value.trim() === "" ||
                  list.name
                    .toLowerCase()
                    .includes(searchField.value.toLowerCase().trim()),
              )
              .map((list, key) => (
                <ListSidebarItem key={`List-item-${key}`} List={list} />
              ))
          ) : (
            <p className="flex items-center gap-2">
              <LucideLoader2 className="animate-spin" />
              <span>Loading</span>
            </p>
          )}
        </ul>
      </aside>
      <>{children}</>
    </div>
  );
}

function ListSidebarItem({ List }: { List: ListType }) {
  const href = `/dashboard/lists/${List.listID}`;
  const active = href === usePathname();

  return (
    <li className="relative">
      <Link
        href={href}
        className={cn(
          "hover:text-active-light block rounded-xl rounded-r-none border-r-2 px-4 py-3 text-black",
          { "text-active border-2 border-r-0": active },
        )}
      >
        {List.name}
      </Link>
      <Menubar
        trigger={
          <Button
            className="absolute right-[4px] top-[50%] translate-y-[-50%]"
            icon={<SlOptionsVertical />}
            variant="flat"
          />
        }
      >
        <Button
          label="delete"
          icon={<FiTrash />}
          className="p-0"
          variant="flat"
          // onClick={handleDelete}
          desctructive
        />
      </Menubar>
    </li>
  );
}

export function useNewListDailog() {
  const Dialogs = useDialog();

  return () => {
    Dialogs?.FormDialog({
      title: "New List",
      description: "Description text",
      // className: "flex flex-col",
      children: (
        <>
          <Input label="List Name" wrapperClassName="col-span-2" />
          <DialogClose asChild>
            <Button label="Cancel" variant="hollow" />
          </DialogClose>
          <Button label="Create" type="submit" />
        </>
      ),
    });
  };
}
