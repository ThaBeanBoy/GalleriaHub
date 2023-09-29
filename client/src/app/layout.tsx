import "./globals.css";
import type { Metadata } from "next";
import { Poppins } from "next/font/google";

import Navigation from "@/components/LayoutNav";

const poppins = Poppins({
  weight: ["100", "200", "300", "400", "500", "600", "700", "800", "900"],
  subsets: ["latin"],
});

export const metadata: Metadata = {
  title: "Create Next App",
  description: "Generated by create next app",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en">
      <body className={`${poppins.className} `}>
        <Navigation />

        <div id="page-container" className="max-width min-h-screen py-8">
          {children}
        </div>

        <div id="footer-container" className="bg-grey-light py-8">
          <footer className="max-width ">
            <h1>Footer</h1>
          </footer>
        </div>
      </body>
    </html>
  );
}
