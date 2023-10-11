import { ProductType } from "@/lib/types";
import axios from "axios";

export default async function Shop() {
  const { data } = await axios<ProductType[]>({
    method: "get",
    url: `${process.env.NEXT_PUBLIC_SERVER_URL}/products/`,
  });

  return (
    <main>
      <ul>
        {data.map(({ productID, productName }) => (
          <li key={productID}>{productName}</li>
        ))}
      </ul>
    </main>
  );
}
