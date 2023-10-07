import React from "react";
import { cn } from "@/lib/utils";
import Link from "next/link";

export type ButtonProps = {
  label?: string;
  icon?: React.ReactElement;
  variant?: "fill" | "hollow";
  desctructive?: boolean;
} & React.ButtonHTMLAttributes<HTMLButtonElement>;

const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  (
    {
      label,
      icon,
      variant = "fill",
      desctructive = false,
      className,
      ...props
    },
    ref,
  ) => (
    <button
      className={cn(
        "bg-active border-active flex items-center justify-center gap-2 rounded-3xl border px-4 py-2 text-base font-semibold capitalize text-white shadow-md",
        {
          // hollow variant
          "text-active bg-white ": variant === "hollow",

          // icon only
          "h-10 w-10 rounded-2xl p-0": !label,

          //desctructive
          "border-red-500 bg-red-500": desctructive,
          "text-red-500": desctructive && variant === "hollow",
        },
        className,
      )}
      ref={ref}
      {...props}
    >
      {label} {icon}
    </button>
  ),
);

Button.displayName = "Button";

export default Button;
