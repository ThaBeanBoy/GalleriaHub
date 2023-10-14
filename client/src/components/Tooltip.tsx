import { cn } from "@/lib/utils";
import * as RadixTooltip from "@radix-ui/react-tooltip";

export default function Tooltip({
  trigger,
  className,
  children,
  ...props
}: { trigger: React.ReactNode } & RadixTooltip.TooltipContentProps) {
  return (
    <RadixTooltip.Root>
      <RadixTooltip.Trigger asChild>{trigger}</RadixTooltip.Trigger>

      <RadixTooltip.Portal>
        <RadixTooltip.Content
          className={cn(
            "data-[state=delayed-open]:data-[side=top]:animate-slideDownAndFade data-[state=delayed-open]:data-[side=right]:animate-slideLeftAndFade data-[state=delayed-open]:data-[side=left]:animate-slideRightAndFade data-[state=delayed-open]:data-[side=bottom]:animate-slideUpAndFade text-violet11 select-none rounded-xl border border-gray-300 bg-white p-2 text-[15px] text-xs leading-none shadow-lg will-change-[transform,opacity]",
            className,
          )}
          {...props}
        >
          {children}
          <RadixTooltip.Arrow className="fill-gray-300" />
        </RadixTooltip.Content>
      </RadixTooltip.Portal>
    </RadixTooltip.Root>
  );
}

export function TooltipProvider({
  ...props
}: RadixTooltip.TooltipProviderProps) {
  return <RadixTooltip.Provider {...props} />;
}
