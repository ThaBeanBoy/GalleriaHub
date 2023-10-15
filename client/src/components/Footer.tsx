import Link from "next/link";
import StackGrid from "react-stack-grid";

type FooterGroup = {
  groupTitle: string;
  links: { title: string; href: string }[];
};

const FooterGroups: FooterGroup[] = [
  {
    groupTitle: "Customer Car",
    links: [
      { title: "contact us", href: "/" },
      { title: "FAQs", href: "/" },
      { title: "returns & exchanges", href: "/" },
      { title: "shipping & Handling", href: "/" },
      { title: "damaged or defected items", href: "/" },
      { title: "cancelling or changing an order", href: "/" },
      { title: "terms of service", href: "/" },
      { title: "privacy policy", href: "/" },
    ],
  },
  {
    groupTitle: "Inside Galleria",
    links: [
      { title: "about us", href: "/" },
      { title: "GE magazine", href: "/" },
      { title: "why galleria", href: "/" },
      { title: "contact us", href: "/" },
      { title: "whole sale", href: "/" },
      { title: "careers", href: "/" },
    ],
  },
  {
    groupTitle: "Customer Car",
    links: [
      { title: "contact us", href: "/" },
      { title: "FAQs", href: "/" },
      { title: "returns & exchanges", href: "/" },
      { title: "shipping & Handling", href: "/" },
      { title: "damaged or defected items", href: "/" },
      { title: "cancelling or changing an order", href: "/" },
      { title: "terms of service", href: "/" },
      { title: "privacy policy", href: "/" },
    ],
  },
  {
    groupTitle: "Inside Galleria",
    links: [
      { title: "about us", href: "/" },
      { title: "GE magazine", href: "/" },
      { title: "why galleria", href: "/" },
      { title: "contact us", href: "/" },
      { title: "whole sale", href: "/" },
      { title: "careers", href: "/" },
    ],
  },
  {
    groupTitle: "My Account",
    links: [
      { title: "Sign Up/Register", href: "/authentication/" },
      { title: "Wishlists", href: "/dashboard/lists/" },
      { title: "cart", href: "/cart" },
    ],
  },
];

export default function Footer() {
  return (
    <div id="footer-container" className="bg-grey-light py-8">
      <footer className="max-width">
        <StackGrid columnWidth={284} gutterWidth={16} gutterHeight={24}>
          {FooterGroups.map(({ groupTitle, links }, key) => (
            <div key={`footer-group-${key}`}>
              <h4 className="text-black-x2 mb-4 text-sm font-bold">
                {groupTitle}
              </h4>

              <ul>
                {links.map(({ title, href }, key) => (
                  <li key={`${groupTitle}-${key}`} className="mb-2 last:mb-0">
                    <Link
                      href={href}
                      className="hover:text-active text-sm capitalize text-gray-500 duration-300"
                    >
                      {title}
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </StackGrid>
      </footer>
    </div>
  );
}
