"use client";

import axios from "axios";

import { FormEvent, useRef } from "react";

import Link from "next/link";
import Image from "next/image";

import { Checkbox } from "@/components/ui/checkbox";

import { BiLockAlt } from "react-icons/bi";
import { FiMail, FiUserPlus } from "react-icons/fi";

import Form from "@/components/Form";
import Input from "@/components/Input";
import Button from "@/components/Button";

export default function SignUpPage() {
  const UsernameRef = useRef<HTMLInputElement>(null);
  const EmailRef = useRef<HTMLInputElement>(null);
  const PasswordRef = useRef<HTMLInputElement>(null);
  const ConfirmPasswordRef = useRef<HTMLInputElement>(null);
  const TsCsRef = useRef<HTMLButtonElement>(null);

  const handleSignUp = async (e: FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();

      const formData = new FormData();
      formData.append("email", EmailRef.current?.value || "");
      formData.append("username", UsernameRef.current?.value || "");
      formData.append("password", PasswordRef.current?.value || "");
      formData.append(
        "confirm-password",
        ConfirmPasswordRef.current?.value || "",
      );

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/sign-up`,
        data: formData,
      });

      console.log(data);
    } catch (err: any) {
      console.log(err.response.data);
    }

    // console.log(data);
  };

  return (
    <main className="prose flex flex-col items-center pt-6 lg:pt-0">
      <h1 className="pl-4">Sign Up</h1>

      <Form onSubmit={handleSignUp}>
        <Input
          label="email"
          icon={<FiMail />}
          name="email"
          id="email"
          ref={EmailRef}
        />
        <Input
          label="username"
          icon={<FiUserPlus />}
          name="username"
          id="username"
          ref={UsernameRef}
        />

        <Input
          label="password"
          type="password"
          name="password"
          icon={<BiLockAlt />}
          id="password"
          wrapperClassName="col-span-2"
          ref={PasswordRef}
        />

        <Input
          label="confirm password"
          icon={<BiLockAlt />}
          type="password"
          name="confirm-password"
          id="confirm password"
          wrapperClassName="col-span-2"
          ref={ConfirmPasswordRef}
        />

        <div className="col-span-2 flex items-center space-x-2">
          <Checkbox
            id="accept-ts-cs"
            className="rounded-md border border-black"
            name="accept-ts-cs"
            ref={TsCsRef}
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

          <Button label="sign up" className="flex-1" type="submit" />
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
