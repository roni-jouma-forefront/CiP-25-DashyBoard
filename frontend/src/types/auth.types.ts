export type AdminDto = {
  id: string;
  firstName: string | null;
  lastName: string | null;
  role: string | null;
  hotelId: string | null;
};

export type LoginRequest = {
  hotelId: string;
  password: string;
};

export type LoginResponse = {
  token: string;
  admin: AdminDto;
};
