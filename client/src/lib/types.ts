export type JwtType = {
  token: string;
  expiryDate: Date;
};

export type UserType = {
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
  cart: any[];
};

export type ProductType = {
  productID: number;
  productName: string;
  price: number;
  stockQuantity: number;
  description?: string;
  Public: boolean;
  createdOn: Date;
  lastUpdate: Date;
  seller: UserType;
  images: string[];
};

export type ListType = {
  listID: number;
  name: string;
  createdOn: Date;
  lastUpdate: Date;
  items: ProductType[];
};
