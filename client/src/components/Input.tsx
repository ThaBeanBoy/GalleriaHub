import React from "react";
import { cn } from "@/lib/utils";

export type InputProps = {
  label?: string;
  icon?: React.ReactElement;
} & React.InputHTMLAttributes<HTMLInputElement>;

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  ({ className, label, icon, id, readOnly, ...props }, ref) => (
    <div className="flex flex-col gap-1">
      {label && (
        <label htmlFor={id} className="flex items-center gap-1 pl-4 text-xs">
          {icon} {label}
        </label>
      )}
      <input
        className={cn(
          "border-grey-control rounded-3xl border bg-white px-4 py-2",
          "bg-red" && readOnly,
          className,
        )}
        id={id}
        {...props}
        ref={ref}
        readOnly={readOnly}
      />
    </div>
  ),
);

Input.displayName = "Input";

export default Input;
