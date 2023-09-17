import Input from "../components/Input";
import { LuSearch } from "react-icons/lu";

export default function Home() {
  return (
    <main className="prose flex flex-col">
      <h1 className="mb-8">Hello World</h1>
      <Input
        icon={<LuSearch />}
        label="username"
        id="username"
        name="username"
        placeholder="username"
        readOnly
      />
    </main>
  );
}
