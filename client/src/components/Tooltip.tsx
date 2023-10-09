import { cn } from "@/lib/utils";
import * as Dropdown from "@radix-ui/react-dropdown-menu";
import * as Menubar from "@radix-ui/react-menubar";

export default function Tooltip({
  trigger,
  children,
  className,
  ...props
}: { trigger: React.ReactNode } & Dropdown.DropdownMenuContentProps) {
  return (
    <Menubar.Root>
      <Menubar.Menu>
        <Menubar.Trigger asChild>{trigger}</Menubar.Trigger>

        <Menubar.Portal>
          <Menubar.Content
            {...props}
            className={cn(
              "border-active-light rounded-lg border bg-white p-2 text-sm drop-shadow-lg",
              className,
            )}
          >
            {children}
            <Menubar.Arrow className="fill-active-light" />
          </Menubar.Content>
        </Menubar.Portal>
      </Menubar.Menu>
    </Menubar.Root>
  );
}
