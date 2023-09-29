"use client";

import Image from "next/image";

import { BiLockAlt } from "react-icons/bi";
import { FiUserPlus } from "react-icons/fi";

import Form from "@/components/Form";
import Input from "@/components/Input";
import Button from "@/components/Button";
import { FormEvent, useRef } from "react";
import axios from "axios";

export default function Login() {
  const EmailUsernameRef = useRef<HTMLInputElement>(null);
  const PasswordRef = useRef<HTMLInputElement>(null);

  const handleLogin = async (e: FormEvent<HTMLFormElement>) => {
    try {
      e.preventDefault();

      const formData = new FormData();
      formData.append("username", EmailUsernameRef.current?.value || "");
      formData.append("password", PasswordRef.current?.value || "");

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/login`,
        data: formData,
      });

      console.log(data);
    } catch (err: any) {
      console.log(err.response.data);
    }
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
