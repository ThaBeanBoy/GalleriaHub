import { cn } from "@/lib/utils";
import * as RadixSwitch from "@radix-ui/react-switch";
import { RefAttributes } from "react";

export default function Switch({
  label,
  id,
  className,
  ...props
}: {
  label?: string;
  id?: string;
  className?: string;
} & RadixSwitch.SwitchProps &
  RefAttributes<HTMLButtonElement>) {
  return (
    <div className={cn("flex items-center gap-2", className)}>
      <label htmlFor={id} className="text-xs">
        {label}
      </label>
      <RadixSwitch.Root
        className="data-[state=checked]:bg-active data-[state=checked]:border-active relative h-[25px] w-[42px] cursor-default rounded-full border border-gray-500 bg-gray-200 shadow outline-none"
        id={id}
        {...props}
      >
        <RadixSwitch.Thumb className="block h-[21px] w-[21px] translate-x-[2px] rounded-full bg-black shadow-lg transition-transform duration-100 will-change-transform data-[state=checked]:translate-x-[17px] data-[state=checked]:bg-white" />
      </RadixSwitch.Root>
    </div>
  );
}
