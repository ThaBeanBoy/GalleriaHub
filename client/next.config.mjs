// import remarkGfm from "remark-gfm";
// import rehypeAutolinkHeadings from "rehype-autolink-headings";
// import createMDX from "@next/mdx";

// /** @type {import('next').NextConfig} */
// const nextConfig = {
//   experimental: {
//     mdxRs: true,
//   },
// };

// const withMDX = createMDX({
//   extension: /\.mdx?$/,

//   options: {
//     remarkPlugins: [remarkGfm],
//     rehypePlugins: [rehypeAutolinkHeadings],
//     providerImportSource: "@mdx-js/react",
//   },
// });

// export default withMDX(nextConfig);
/** @type {import('next').NextConfig} */
const nextConfig = {
  async rewrites() {
    return [
      {
        source: "/api/:path*",
        destination: `${process.env.NEXT_PUBLIC_SERVER_URL}:path*`,
      },
    ];
  },
};

export default nextConfig;
