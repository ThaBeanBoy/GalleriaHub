import ComponentDocumentation from "@/components/component-document";

import { ComponentDocumentationProps } from "@/components/component-document";

const documentedComponents: ComponentDocumentationProps[] = [
  {
    name: "Header 1",
    code: `<h1>Home Page</h1>`,
    render: <h1>Home Page</h1>,
    description: "Should be used sparingly",
  },
  {
    name: "Header 2",
    code: "<h2>h2</h2>",
    render: <h2>h2</h2>,
  },
];

export default function Home() {
  return (
    <main className="flex flex-col">
      <h1 className="mb-8">Typography</h1>

      {documentedComponents.map((props, key) => (
        <ComponentDocumentation
          key={key}
          {...props}
          className="border-grey mb-8 border-b-2 pb-8 last:mb-0"
        />
      ))}

      {/* <ComponentDocumentation
        name="Header 2"
        code={`<h1>h2</h1>`}
        render={<h2>h2</h2>}
      />

      <h2>h2</h2>
      <h3>h3</h3>
      <h4>h4</h4>
      <p>
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Architecto
        alias nam fuga iste cumque! Exercitationem molestiae quo porro eos
        incidunt qui natus earum laudantium, veritatis, perspiciatis unde
        eveniet rem atque!
      </p>
      <p className="small">Lorem ipsum dolor</p>
      <blockquote>
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Architecto
        alias nam fuga iste cumque! Exercitationem molestiae quo porro eos
        incidunt qui natus earum laudantium, veritatis, perspiciatis unde
        eveniet rem atque!
      </blockquote>

      <ul>
        <li>list item 1</li>
        <li>list item 2</li>
        <li>list item 3</li>
      </ul>

      <ol>
        <li>list item 1</li>
        <li>list item 2</li>
        <li>list item 3</li>
      </ol>

      <h1>Buttons</h1> */}
    </main>
  );
}
