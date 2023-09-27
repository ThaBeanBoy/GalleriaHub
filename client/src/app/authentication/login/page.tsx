"use client";

import Image from "next/image";

import { BiLockAlt } from "react-icons/bi";
import { FiUserPlus } from "react-icons/fi";

import Form from "@/components/Form";
import Input from "@/components/Input";
import Button from "@/components/Button";

export default function Login() {
  return (
    <main className="prose flex w-full max-w-[500px] flex-col items-center pt-6 lg:pt-0">
      <h1 className="pl-4">Login</h1>

      <Form className="flex">
        <Input
          label="username or password"
          wrapperClassName="col-span-2"
          icon={<FiUserPlus />}
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
