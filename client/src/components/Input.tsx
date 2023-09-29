import React, { useState } from "react";
import { cn } from "@/lib/utils";

import Button from "./Button";
import { EyeIcon, EyeOff } from "lucide-react";

export type InputProps = {
  label?: string;
  icon?: React.ReactElement;
  wrapperClassName?: string;
} & React.InputHTMLAttributes<HTMLInputElement>;

const Input = React.forwardRef<HTMLInputElement, InputProps>(
  (
    { className, wrapperClassName, label, icon, id, type, readOnly, ...props },
    ref,
  ) => {
    const [passwordRevealed, setPasswordRevealed] = useState(false);
    const switchPasswordReveal = (
      e?: any /* React.MouseEventHandler<HTMLButtonElement> */,
    ) => {
      e.preventDefault();
      setPasswordRevealed(!passwordRevealed);
    };

    return (
      <div className={cn("relative flex flex-col gap-1", wrapperClassName)}>
        {label && (
          <label htmlFor={id} className="flex items-center gap-1 pl-4 text-xs">
            {icon} {label}
          </label>
        )}

        <input
          className={cn(
            "border-grey-control focus:border-active rounded-3xl border bg-white px-4 py-2 outline-none",
            "bg-red" && readOnly,
            className,
          )}
          id={id}
          {...props}
          ref={ref}
          readOnly={readOnly}
          type={
            type === "password"
              ? passwordRevealed
                ? "text"
                : "password"
              : type
          }
        />

        {type === "password" && (
          <Button
            type="button"
            icon={passwordRevealed ? <EyeIcon /> : <EyeOff />}
            onClick={switchPasswordReveal}
            variant="hollow"
            className="absolute bottom-1 right-1 rounded-l-none border-0 shadow-none"
          />
        )}
      </div>
    );
  },
);

Input.displayName = "Input";

export default Input;
