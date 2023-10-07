import { cn } from "@/lib/utils";
import * as Dropdown from "@radix-ui/react-dropdown-menu";

export default function Tooltip({
  trigger,
  children,
  className,
  ...props
}: { trigger: React.ReactNode } & Dropdown.DropdownMenuContentProps) {
  return (
    <Dropdown.Root>
      <Dropdown.Trigger asChild>{trigger}</Dropdown.Trigger>

      <Dropdown.Portal>
        <Dropdown.Content
          {...props}
          className={cn(
            "border-active-light rounded-lg border bg-white p-2 text-sm drop-shadow-lg",
            className,
          )}
        >
          {children}
          <Dropdown.Arrow className="fill-active-light" />
        </Dropdown.Content>
      </Dropdown.Portal>
    </Dropdown.Root>
  );
}
