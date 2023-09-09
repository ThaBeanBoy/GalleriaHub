import DocLink from "@/components/doc-link";
import { FaFigma } from "react-icons/fa";
import { VscTable } from "react-icons/vsc";
import { CiBoxes } from "react-icons/ci";
export type DocLink = {
  title: string;
  href: string;
  icon?: React.ReactNode;
};

export type LinkGroup = {
  goupName: string;
  links: DocLink[];
};

const DocsNavigations: LinkGroup[] = [
  {
    goupName: "General",
    links: [
      {
        title: "overview",
        href: "",
      },
      {
        title: "resources",
        href: "resources",
        icon: <CiBoxes />,
      },
    ],
  },
  {
    goupName: "design",
    links: [
      {
        title: "UI Design",
        href: "ui-design",
        icon: <FaFigma />,
      },
      {
        title: "Database Design",
        href: "db-design",
        icon: <VscTable />,
      },
    ],
  },
];
export default function DocumentationLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  // const currentURL = usePathname();

  return (
    // Container
    <div className="relative flex gap-4">
      <aside className="sticky top-8 mr-2 h-[calc(100vh-72.8px)] w-[284px] border-r pr-2">
        {/* <p className="text-black-x2 font-bold">Design</p> */}
        {/* <Link href="documentation/" className="text-active">
          Typgraphy
        </Link>
        <h4>Frontend</h4>
        <Link href="documentation/">Typgraphy</Link>
        <h4>Backend</h4> */}
        {DocsNavigations.map(({ goupName, links }, key) => (
          <>
            <p className="text-black-x2 mb-2 pl-3 font-bold capitalize">
              {goupName}
            </p>

            <ul className="mb-4 list-none last:mb-0">
              {links.map((link, key) => (
                <DocLink key={key} {...link} />
              ))}
            </ul>
          </>
        ))}
      </aside>

      <main className="md-viewer max-w-[730px]">{children}</main>
    </div>
  );
}
