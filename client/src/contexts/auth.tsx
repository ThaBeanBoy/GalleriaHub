import axios, { AxiosResponse } from "axios";
import { createContext, useEffect, useState } from "react";

export type AuthType = {
  jwt: {
    token: string;
    expiryDate: Date;
  };

  user: {
    userID: number;
    email: string;
    username: string;
    createdOn: Date;
    lastUpdate: Date;
    profilePicture: string | null;
    name: string | null;
    surname: string | null;
    phoneNumber: string | null;
    location: string | null;
  };
};

type loginProps = {
  username: string;
  password: string;
  success?: (data: any) => void;
  failed?: (error: any) => void;
};

type signUpProps = {
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
  success?: (data: any) => void;
  failed?: (error: any) => void;
};

export type UserContextType = {
  auth: AuthType | null;
  loginHandler: (loginDetails: loginProps) => void;
  signUpHandler: (signUpDetails: signUpProps) => void;
  logoutHandler: () => void;
};

export const UserContext = createContext<UserContextType | null>(null);

export default function AuthProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [auth, setAuth] = useState<AuthType | null>(null);

  const loginHandler = async ({
    username,
    password,
    success,
    failed,
  }: loginProps) => {
    try {
      const formData = new FormData();
      formData.append("username", username);
      formData.append("password", password);

      const { data } = await axios<AuthType>({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/login`,
        data: formData,
      });

      setAuth(ResponseDataToAuthType(data));

      if (success) success(data);
    } catch (error: any) {
      if (failed) failed(error);
    }
  };

  const signUpHandler = async ({
    email,
    username,
    password,
    confirmPassword,
    success,
    failed,
  }: signUpProps) => {
    try {
      const formData = new FormData();
      formData.append("email", email);
      formData.append("username", username);
      formData.append("password", password);
      formData.append("confirm-password", confirmPassword);

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/sign-up`,
        data: formData,
      });

      setAuth(ResponseDataToAuthType(data));

      if (success) success(data);
    } catch (error: any) {
      if (failed) failed(error);
    }
  };

  const logoutHandler = () => {
    //make the user object null
    // remove the jwt token from the cookies/internal storage
  };

  useEffect(() => {
    console.log(auth);
  }, [auth]);

  return (
    <UserContext.Provider
      value={{ auth, loginHandler, signUpHandler, logoutHandler }}
    >
      {children}
    </UserContext.Provider>
  );
}

function ResponseDataToAuthType(data: AuthType) {
  // Making dates
  data.jwt.expiryDate = new Date(data.jwt.expiryDate);
  data.user.createdOn = new Date(data.user.createdOn);
  data.user.lastUpdate = new Date(data.user.lastUpdate);

  return data;
}
