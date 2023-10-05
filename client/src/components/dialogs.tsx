import * as AlertDialog from "@radix-ui/react-alert-dialog";

import { BiError } from "react-icons/bi";

import Button from "@/components/Button";
import { cn } from "@/lib/utils";
import { FC } from "react";

function Overlay({ className }: { className?: string }) {
  return (
    <AlertDialog.Overlay
      className={cn(
        "data-[state=open]:animate-overlayShow fixed inset-0 bg-black opacity-60",
        className,
      )}
    />
  );
}

export function ErrorDialog({
  trigger,
  title,
  message,
  buttonLabel,
  buttonIcon,
  ...props
}: {
  trigger: React.ReactNode;
  title?: string;
  message: string;
  buttonLabel?: string;
  buttonIcon?: React.ReactElement;
} & FC<AlertDialog.AlertDialogProps>) {
  return (
    <AlertDialog.Root {...props}>
      <AlertDialog.Trigger asChild>{trigger}</AlertDialog.Trigger>

      <AlertDialog.Portal>
        <Overlay className="bg-red-50" />

        <AlertDialog.Content className="data-[state=open]:animate-contentShow fixed left-[50%] top-[50%] flex max-h-[85vh] w-[90vw] max-w-[450px] translate-x-[-50%] translate-y-[-50%] flex-col gap-4 rounded-3xl bg-white p-[25px] shadow-lg">
          <h1 className="flex items-center gap-2 text-2xl font-semibold">
            <BiError className="text-red-500" /> <span>Error</span>
          </h1>

          <hr />

          {title && (
            <AlertDialog.Title>
              <h2 className="text-xl font-bold">{title}</h2>
            </AlertDialog.Title>
          )}

          <p className="text-gray-500">{message}</p>

          <AlertDialog.Cancel asChild>
            <Button label={buttonLabel || "Okay"} icon={buttonIcon} />
          </AlertDialog.Cancel>
        </AlertDialog.Content>
      </AlertDialog.Portal>
    </AlertDialog.Root>
  );
}
