import type { Config } from "tailwindcss";

const config: Config = {
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    extend: {
      backgroundImage: {
        "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
        "gradient-conic":
          "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
      },

      colors: {
        white: "#FFFAF5",
        black: {
          DEFAULT: "#475569",
          x2: "#27313F",
        },

        grey: {
          DEFAULT: "#B1B1B1",
          light: "#E2E8F0",
          control: "#DEE3E9",
          code: "rgba(1,61,228,.224)",
        },

        active: {
          DEFAULT: "#3667DC",
          light: "#7D9CE8",
        },
      },
    },
  },
  plugins: [],
};
export default config;
