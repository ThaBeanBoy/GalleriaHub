import type { Config } from "tailwindcss";

const config: Config = {
  mode: "jit",
  content: [
    "./src/pages/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/components/**/*.{js,ts,jsx,tsx,mdx}",
    "./src/app/**/*.{js,ts,jsx,tsx,mdx}",
  ],
  theme: {
    typography: {
      DEFAULT: {
        css: {
          color: "#475569",
          a: {
            color: "#3667DC",
          },
        },
      },
    },

    container: {
      center: true,
      padding: "2rem",
      screens: {
        "2xl": "1400px",
      },
    },

    extend: {
      gridTemplateRows: {
        4: "repeat(auto-fit, minmax(292px, 1fr))",
      },

      backgroundImage: {
        "gradient-radial": "radial-gradient(var(--tw-gradient-stops))",
        "gradient-conic":
          "conic-gradient(from 180deg at 50% 50%, var(--tw-gradient-stops))",
      },

      colors: {
        white: "#ffffff",
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

    keyframes: {
      "accordion-down": {
        from: { height: "0" },
        to: { height: "var(--radix-accordion-content-height)" },
      },
      "accordion-up": {
        from: { height: "var(--radix-accordion-content-height)" },
        to: { height: "0" },
      },

      // Radix UI Navigation Keyframes
      enterFromRight: {
        from: { opacity: "0", transform: "translateX(200px)" },
        to: { opacity: "1", transform: "translateX(0)" },
      },
      enterFromLeft: {
        from: { opacity: "0", transform: "translateX(-200px)" },
        to: { opacity: "1", transform: "translateX(0)" },
      },
      exitToRight: {
        from: { opacity: "1", transform: "translateX(0)" },
        to: { opacity: "0", transform: "translateX(200px)" },
      },
      exitToLeft: {
        from: { opacity: "1", transform: "translateX(0)" },
        to: { opacity: "0", transform: "translateX(-200px)" },
      },
      scaleIn: {
        from: { opacity: "0", transform: "rotateX(-10deg) scale(0.9)" },
        to: { opacity: "1", transform: "rotateX(0deg) scale(1)" },
      },
      scaleOut: {
        from: { opacity: "1", transform: "rotateX(0deg) scale(1)" },
        to: { opacity: "0", transform: "rotateX(-10deg) scale(0.95)" },
      },
      fadeIn: {
        from: { opacity: "0" },
        to: { opacity: "1" },
      },
      fadeOut: {
        from: { opacity: "1" },
        to: { opacity: "0" },
      },

      // Tooltip animation
      slideDownAndFade: {
        from: { opacity: "0", transform: "translateY(-2px)" },
        to: { opacity: "1", transform: "translateY(0)" },
      },
      slideLeftAndFade: {
        from: { opacity: "0", transform: "translateX(2px)" },
        to: { opacity: "1", transform: "translateX(0)" },
      },
      slideUpAndFade: {
        from: { opacity: "0", transform: "translateY(2px)" },
        to: { opacity: "1", transform: "translateY(0)" },
      },
      slideRightAndFade: {
        from: { opacity: "0", transform: "translateX(-2px)" },
        to: { opacity: "1", transform: "translateX(0)" },
      },

      // Toast keyframes
      hide: {
        from: { opacity: "1" },
        to: { opacity: "0" },
      },
      slideIn: {
        from: { transform: "translateX(calc(100% + var(--viewport-padding)))" },
        to: { transform: "translateX(0)" },
      },
      swipeOut: {
        from: { transform: "translateX(var(--radix-toast-swipe-end-x))" },
        to: { transform: "translateX(calc(100% + var(--viewport-padding)))" },
      },

      // Dialog keyframes
      overlayShow: {
        from: { opacity: "0" },
        to: { opacity: "1" },
      },
      contentShow: {
        from: { opacity: "0", transform: "translate(-50%, -48%) scale(0.96)" },
        to: { opacity: "1", transform: "translate(-50%, -50%) scale(1)" },
      },
    },
    animation: {
      "accordion-down": "accordion-down 0.2s ease-out",
      "accordion-up": "accordion-up 0.2s ease-out",

      // Radix UI Navigation animations
      scaleIn: "scaleIn 200ms ease",
      scaleOut: "scaleOut 200ms ease",
      fadeIn: "fadeIn 200ms ease",
      fadeOut: "fadeOut 200ms ease",
      enterFromLeft: "enterFromLeft 250ms ease",
      enterFromRight: "enterFromRight 250ms ease",
      exitToLeft: "exitToLeft 250ms ease",
      exitToRight: "exitToRight 250ms ease",

      // tooltip animations
      slideDownAndFade: "slideDownAndFade 400ms cubic-bezier(0.16, 1, 0.3, 1)",
      slideLeftAndFade: "slideLeftAndFade 400ms cubic-bezier(0.16, 1, 0.3, 1)",
      slideUpAndFade: "slideUpAndFade 400ms cubic-bezier(0.16, 1, 0.3, 1)",
      slideRightAndFade:
        "slideRightAndFade 400ms cubic-bezier(0.16, 1, 0.3, 1)",

      //Toast animations
      hide: "hide 100ms ease-in",
      slideIn: "slideIn 150ms cubic-bezier(0.16, 1, 0.3, 1)",
      swipeOut: "swipeOut 100ms ease-out",

      overlayShow: "overlayShow 150ms cubic-bezier(0.16, 1, 0.3, 1)",
      contentShow: "contentShow 150ms cubic-bezier(0.16, 1, 0.3, 1)",
    },
  },
  plugins: [
    require("@tailwindcss/typography"),
    require("tailwindcss-animate"),
    require("@kamona/tailwindcss-perspective"),
  ],
};
export default config;
