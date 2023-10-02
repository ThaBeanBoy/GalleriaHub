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

type loginProps = { username: string; password: string };
type signUpProps = {
  email: string;
  username: string;
  password: string;
  confirmPassword: string;
};

export type UserContextType = {
  user: UserType | null;
  loginHandler: (loginDetails: loginProps) => void;
  signUpHandler: (signUpDetails: signUpProps) => void;
};

const UserContext = createContext<UserContextType | null>(null);

export default function AuthProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const [user, setUser] = useState<UserType | null>(null);

  const loginHandler = ({ username, password }: loginProps) => {};

  const signUpHandler = ({
    email,
    username,
    password,
    confirmPassword,
  }: signUpProps) => {};
  // Get user on initial run
  useEffect(() => {}, []);

  return (
    <UserContext.Provider value={{ user, loginHandler, signUpHandler }}>
      {children}
    </UserContext.Provider>
  );
}
