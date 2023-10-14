import { cn } from "@/lib/utils";
import * as RadixAvatar from "@radix-ui/react-avatar";

export default function Avatar({
  src,
  alt,
  fallback,
  className,
}: {
  src: string;
  alt: string;
  fallback: string;
} & RadixAvatar.AvatarProps) {
  return (
    <RadixAvatar.Root
      className={cn(
        "inline-flex h-[36px] w-[36px] select-none items-center justify-center overflow-hidden rounded-xl bg-white align-middle shadow",
        className,
      )}
    >
      <RadixAvatar.Image
        src={src}
        alt={alt}
        className="h-full w-full rounded-[inherit] object-cover"
      />
      <RadixAvatar.Fallback
        delayMs={600}
        className="leading-1 bg-active-light flex h-full w-full items-center justify-center text-[15px] font-medium text-white"
      >
        {fallback}
      </RadixAvatar.Fallback>
    </RadixAvatar.Root>
  );
}
