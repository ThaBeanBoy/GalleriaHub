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
};

export type ProductType = {
  productID: number;
  productName: string;
  price: number;
  stockQuantity: number;
  Public: boolean;
  createdOn: Date;
  lastUpdate: Date;
  Description: string;
  seller: UserType;
};
