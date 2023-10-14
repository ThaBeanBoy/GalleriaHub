import { cn } from "@/lib/utils";
import * as Dropdown from "@radix-ui/react-dropdown-menu";
import * as RadixMenubar from "@radix-ui/react-menubar";

export default function Menubar({
  trigger,
  children,
  className,
  ...props
}: { trigger: React.ReactNode } & RadixMenubar.MenuContentProps) {
  return (
    <RadixMenubar.Root>
      <RadixMenubar.Menu>
        <RadixMenubar.Trigger asChild>{trigger}</RadixMenubar.Trigger>

        <RadixMenubar.Portal>
          <RadixMenubar.Content
            {...props}
            className={cn(
              "border-active-light rounded-lg border bg-white p-2 text-sm drop-shadow-lg",
              className,
            )}
          >
            {children}
            <RadixMenubar.Arrow className="fill-active-light" />
          </RadixMenubar.Content>
        </RadixMenubar.Portal>
      </RadixMenubar.Menu>
    </RadixMenubar.Root>
  );
}
