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
      <aside className="sidebar mr-2 border-r pr-2">
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

      <main className="md-viewer prose prose-a:text-active prose-a:no-underline max-w-[730px]">
        {children}
      </main>

      <aside className="prose sidebar ml-2 border-l pl-2">
        <h4 className="prose: capitalize">Table of Content</h4>
        <ul className="pl-0">
          <li>section 1</li>
          <li>section 2</li>
        </ul>
      </aside>
    </div>
  );
}
