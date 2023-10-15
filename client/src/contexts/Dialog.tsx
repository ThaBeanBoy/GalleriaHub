import Form, { FormProps } from "@/components/Form";
import { cn } from "@/lib/utils";
import * as Dialog from "@radix-ui/react-dialog";
import { createContext, useContext, useState } from "react";

export type FormDialogProps = {
  title?: string;
  description?: string;
} & FormProps;

export type DialogProviderType = {
  FormDialog: (props: FormDialogProps) => void;
};

export const DialogContext = createContext<DialogProviderType | undefined>(
  undefined,
);

function Overlay() {
  return (
    <Dialog.Overlay className="data-[state=open]:animate-overlayShow fixed inset-0 z-40 bg-black opacity-60" />
  );
}

export function DialogClose(props: Dialog.DialogCloseProps) {
  return <Dialog.Close {...props} />;
}

export default function DialogProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [displayFormDialog, setDisplayFormDialog] = useState(false);
  const [formDialogProps, setFormDialogProps] = useState<FormDialogProps>({});
  const FormDialogHandler = (props: FormDialogProps) => {
    setFormDialogProps(props);
    setDisplayFormDialog(true);
  };

  return (
    <DialogContext.Provider value={{ FormDialog: FormDialogHandler }}>
      {children}
      <Dialog.Root open={displayFormDialog} onOpenChange={setDisplayFormDialog}>
        <Dialog.Portal>
          <Dialog.Content asChild>
            <Form
              className={cn(
                "data-[state=open]:animate-contentShow fixed left-[50%] top-[50%] z-50 max-h-[85vh] w-[90vw] max-w-[450px] translate-x-[-50%] translate-y-[-50%] rounded-2xl bg-white p-[25px] shadow-[hsl(206_22%_7%_/_35%)_0px_10px_38px_-10px,_hsl(206_22%_7%_/_20%)_0px_10px_20px_-15px] focus:outline-none",
                formDialogProps.className,
              )}
              {...formDialogProps}
            >
              {formDialogProps.title && (
                <p className="col-span-2 py-2 pl-4 font-bold">
                  {formDialogProps.title}
                </p>
              )}

              {formDialogProps.description && (
                <p className="col col-span-2 pl-4 text-sm text-gray-600">
                  {formDialogProps.description}
                </p>
              )}

              {formDialogProps.children}
            </Form>
          </Dialog.Content>

          <Overlay />
        </Dialog.Portal>
      </Dialog.Root>
    </DialogContext.Provider>
  );
}

export function useDialog() {
  const context = useContext(DialogContext);
  return context;
}
