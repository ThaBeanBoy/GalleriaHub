"use client";

import SyntaxHighlighter from "react-syntax-highlighter";
import { a11yLight } from "react-syntax-highlighter/dist/esm/styles/hljs";
import { twMerge } from "tailwind-merge";
import { useState } from "react";
import { BsClipboard } from "react-icons/bs";

export type ComponentDocumentationProps = {
  name: string;
  code: string;
  language?: string;
  render: React.ReactNode | string;
  description?: React.ReactNode | string;
  className?: string;
};

export default function ComponentDocumentation({
  name,
  code,
  language = "typescript",
  render,
  description,
  className,
}: ComponentDocumentationProps) {
  const [formatedCode, setformatedCode] = useState(code);
  // prettier.format(code).then((res) => setformatedCode(res));

  const copyHandler = () => {
    navigator.clipboard.writeText(code);
    alert("Copied To Clipboard");
  };
  return (
    <div className={twMerge("max-w-[730px]", className)}>
      <h4 className="font-bold">{name}</h4>

      {/* render container */}
      <div>{render}</div>

      <div className="bg-grey-[#E2E8F0] relative rounded-2xl">
        <SyntaxHighlighter
          language={language}
          style={a11yLight}
          customStyle={{
            backgroundColor: "#FFF8F0",
            borderBottomLeftRadius: 16,
            borderBottomRightRadius: 16,
            minHeight: 60,
            padding: 16,
          }}
        >
          {formatedCode}
        </SyntaxHighlighter>

        <button
          onClick={copyHandler}
          className="bg-active absolute right-2 top-2 rounded-2xl p-[13px] text-white drop-shadow-xl"
        >
          <BsClipboard />
        </button>
      </div>

      <p>{description}</p>
    </div>
  );
}
