"use client";

import Image from "next/image";

import { useSearchParams, redirect } from "next/navigation";

import { BiLockAlt } from "react-icons/bi";
import { FiUserPlus } from "react-icons/fi";

import Form from "@/components/Form";
import Input from "@/components/Input";
import Button from "@/components/Button";
import { FormEvent, useContext, useRef } from "react";
import axios from "axios";
import { UserContext } from "@/contexts/auth";

export default function Login() {
  const searchParams = useSearchParams();

  const authContext = useContext(UserContext);

  const EmailUsernameRef = useRef<HTMLInputElement>(null);
  const PasswordRef = useRef<HTMLInputElement>(null);

  const handleLogin = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    authContext?.loginHandler({
      username: EmailUsernameRef.current?.value || "",
      password: PasswordRef.current?.value || "",
      success(data) {
        alert(`logged in`);

        // const callBack = searchParams?.get("callback") ?? "/";
        // console.log(`going to ${callBack}`);
        // redirect(callBack);
      },
      failed(error) {
        console.log(error);
      },
    });
  };

  return (
    <main className="prose flex w-full max-w-[500px] flex-col items-center pt-6 lg:pt-0">
      <h1 className="pl-4">Login</h1>

      <Form className="flex" onSubmit={handleLogin}>
        <Input
          label="username or email"
          wrapperClassName="col-span-2"
          icon={<FiUserPlus />}
          ref={EmailUsernameRef}
        />

        <Input
          label="password"
          type="password"
          name="password"
          icon={<BiLockAlt />}
          id="password"
          className="col-span-2"
          wrapperClassName="col-span-2"
          ref={PasswordRef}
        />

        <div className="col-span-2 flex flex-col-reverse gap-4 lg:flex-row">
          <Button
            label="go to sign up page"
            variant="hollow"
            className="flex-1"
          />
          <Button label="login" className="flex-1" />
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
