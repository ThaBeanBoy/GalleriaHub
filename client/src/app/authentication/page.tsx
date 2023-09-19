"use client";

import Link from "next/link";
import Image from "next/image";

import { Checkbox } from "@/components/ui/checkbox";

import Input from "@/components/Input";
import Button from "@/components/Button";

export default function SignUpPage() {
  return (
    <main className="prose flex flex-col items-center pt-6 lg:pt-0">
      <h1 className="pl-4">Sign Up</h1>
      <form className="mb-4 flex max-w-[500px] grid-cols-2 flex-col gap-4 lg:grid">
        <Input label="email" name="email" id="email" />
        <Input label="username" name="username" id="username" />
        <div className="col-span-2">
          <Input
            label="password"
            type="password"
            name="password"
            id="password"
            className="col-span-2"
          />
        </div>
        <div className="col-span-2">
          <Input
            label="confirm password"
            type="password"
            name="confirm-password"
            id="confirm password"
          />
        </div>

        <div className="col-span-2 flex items-center space-x-2">
          <Checkbox
            id="accept-ts-cs"
            className="rounded-md border border-black"
            name="accept-ts-cs"
          />
          <label className="text-xs">
            You have read our <Link href="/">privacy policy</Link> and agree to
            our <Link href="/">terms & conditions</Link>
          </label>
        </div>

        <div className="col-span-2 flex flex-col-reverse gap-4 lg:flex-row">
          <Link
            href="authentication/login"
            className="block flex-1 no-underline"
          >
            <Button
              label="go to login page"
              variant="hollow"
              className="w-full"
            />
          </Link>
          <Button label="sign up" className="flex-1" />
        </div>
      </form>

      <div id="auth-providers" className="flex flex-col items-center gap-3">
        <label className="text-xs">or sign up with</label>
        <div className="flex gap-4">
          <Button
            icon={
              <Image
                src="facebook-logo.svg"
                alt="facebook"
                width={16}
                height={16}
              />
            }
          />
          <Button
            className="border-white bg-white"
            icon={
              <Image
                src="google-logo.svg"
                alt="facebook"
                width={16}
                height={16}
              />
            }
          />
          <Button
            className="border-black bg-black"
            icon={
              <Image
                src="apple-logo.svg"
                alt="facebook"
                width={16}
                height={16}
              />
            }
          />
        </div>
      </div>
    </main>
  );
}
