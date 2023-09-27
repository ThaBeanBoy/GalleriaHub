import { cn } from "@/lib/utils";
import { forwardRef } from "react";

export type FormProps = React.FormHTMLAttributes<HTMLFormElement>;

const Form = forwardRef<HTMLFormElement, FormProps>(
  ({ className, ...props }, ref) => (
    <form
      className={cn(
        "mb-4 flex w-full max-w-[500px] grid-cols-2 flex-col gap-4 lg:grid",
        className,
      )}
      {...props}
      ref={ref}
    />
  ),
);

Form.displayName = "Form";

export default Form;
