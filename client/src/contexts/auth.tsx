import axios from "axios";
import { createContext, useEffect, useState } from "react";

export type UserType = {
  userID: number;
  email: string;
  username: string;
  createdOn: Date;
  lastUpdate: Date;
  profilePicture?: string;
  name?: string;
  surname?: string;
  phoneNumber?: string;
  location?: string;
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
  user: UserType | null;
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
  const [user, setUser] = useState<UserType | null>(null);

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

      const { data } = await axios({
        method: "post",
        url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/login`,
        data: formData,
      });

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

      if (success) success(data);
    } catch (error: any) {
      if (failed) failed(error);
    }
  };

  const logoutHandler = () => {
    //make the user object null
    // remove the jwt token from the cookies/internal storage
  };

  // Get user on initial run
  useEffect(() => {
    axios({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/authentication/get-user`,
    })
      .then(({ data }) => console.log(data))
      .catch((error) => {
        /* Do nothing, the user is not logged in */
      });
  }, []);

  return (
    <UserContext.Provider
      value={{ user, loginHandler, signUpHandler, logoutHandler }}
    >
      {children}
    </UserContext.Provider>
  );
}
