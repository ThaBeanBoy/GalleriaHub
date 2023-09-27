"use client";

import Link from "next/link";
import Image from "next/image";

import { Checkbox } from "@/components/ui/checkbox";

import { BiLockAlt } from "react-icons/bi";
import { FiMail, FiUserPlus } from "react-icons/fi";

import Form from "@/components/Form";
import Input from "@/components/Input";
import Button from "@/components/Button";

export default function SignUpPage() {
  return (
    <main className="prose flex flex-col items-center pt-6 lg:pt-0">
      <h1 className="pl-4">Sign Up</h1>

      <Form>
        <Input label="email" icon={<FiMail />} name="email" id="email" />
        <Input
          label="username"
          icon={<FiUserPlus />}
          name="username"
          id="username"
        />

        <Input
          label="password"
          type="password"
          name="password"
          icon={<BiLockAlt />}
          id="password"
          wrapperClassName="col-span-2"
        />

        <Input
          label="confirm password"
          icon={<BiLockAlt />}
          type="password"
          name="confirm-password"
          id="confirm password"
          wrapperClassName="col-span-2"
        />

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
            href="/authentication/login"
            className="block flex-1 no-underline"
          >
            <Button
              label="go to login page"
              variant="hollow"
              className="w-full"
              disabled
            />
          </Link>
          <Button label="sign up" className="flex-1" />
        </div>
      </Form>

      <div id="auth-providers" className="flex flex-col items-center gap-3">
        <label className="text-xs">or sign up with</label>
        <div className="flex gap-4">
          <Button
            icon={
              <Image
                src="/facebook-logo.svg"
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
                src="/google-logo.svg"
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
                src="/apple-logo.svg"
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