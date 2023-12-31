"use client";

import "./globals.css";
import type { Metadata } from "next";
import { Poppins } from "next/font/google";

import Navigation from "@/components/LayoutNav";
import AuthProvider from "@/contexts/auth";
import { TooltipProvider } from "@/components/Tooltip";
import { Toaster } from "@/components/ui/toaster";
import Footer from "@/components/Footer";

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
      <AuthProvider>
        <TooltipProvider>
          <body className={`${poppins.className} `}>
            <Navigation />

            <div id="page-container" className="min-h-screen py-8">
              {children}
            </div>

            <Footer />

            <Toaster />
          </body>
        </TooltipProvider>
      </AuthProvider>
    </html>
  );
}
