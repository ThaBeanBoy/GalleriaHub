import Image from "next/image";
import Link from "next/link";

export default function ResourceCard({
  name,
  resourceLink,
  ...props
}: { name: string; resourceLink: string } & React.ComponentProps<
  typeof Image
>) {
  return (
    <Link
      href={resourceLink}
      className="group flex w-min flex-col items-center gap-2 rounded-3xl border p-3"
      target="_blank"
    >
      <Image {...props} className="mb-4 mt-0" />
      <p className="group-hover:text-active my-0 text-black">{name}</p>
    </Link>
  );
}
