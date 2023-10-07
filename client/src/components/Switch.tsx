import { cn } from "@/lib/utils";
import * as RadixSwitch from "@radix-ui/react-switch";

export default function Switch({
  label,
  id,
  className,
}: {
  label?: string;
  id?: string;
  className?: string;
}) {
  return (
    <div className={cn("flex items-center gap-2", className)}>
      <label htmlFor={id} className="text-xs">
        {label}
      </label>
      <RadixSwitch.Root
        className="data-[state=checked]:bg-active bg-active-light relative h-[25px] w-[42px] cursor-default rounded-full border border-black shadow outline-none"
        id={id}
      >
        <RadixSwitch.Thumb className="block h-[21px] w-[21px] translate-x-0.5 rounded-full bg-white shadow-lg transition-transform duration-100 will-change-transform data-[state=checked]:translate-x-[19px]" />
      </RadixSwitch.Root>
    </div>
  );
}
