import type { Dayjs } from "dayjs";

export type RoomStatus = "available" | "occupied";
export type Title = "Mrs" | "Ms" | "Mr" | "Mx" | null;

export type Guest = {
  id: string;
  firstName: string;
  lastName: string;
};

export type ActiveBooking = {
  id: string;
  roomId: string;
  flightNumber: string;
  checkIn: string;
  checkOut: string;
  bookingStatus: number;
  guest: Guest;
  numberOfGuests: number;
};

export type Room = {
  id: string;
  hotelId: number;
  roomNumber: number;
  activeBooking: ActiveBooking | null;
};

export type AdditionalGuest = {
  firstName: string;
  lastName: string;
};

export type Staff = {
  name: string;
};

export type DateTime = {
  field: "post" | "expires";
  value: Dayjs | null;
}