import axios from "axios";

export default async function Developers() {
  const developers = await fetchDevelopers();

  return (
    <main className="flex min-h-screen w-full flex-col items-center justify-around">
      Developers
      <ul>
        {developers.map(({ name, studentNr, initials }, key) => (
          <li
            className="mb-3 cursor-pointer rounded-xl bg-[#fffaf5] p-4 text-center drop-shadow-xl last:mb-0"
            key={key}
          >
            {name}, {initials} <br /> {studentNr}
          </li>
        ))}
      </ul>
    </main>
  );
}

async function fetchDevelopers() {
  if (process.env.NEXT_PUBLIC_SERVER_URL) {
    // Fetch developers
    const { data } = await axios<developer[]>({
      method: "get",
      url: `${process.env.NEXT_PUBLIC_SERVER_URL}/developers/`,
    });

    return data;
  } else {
    throw new Error("Server URL undefined");
  }
}

type developer = {
  name: string;
  studentNr: string;
  initials: string;
};
