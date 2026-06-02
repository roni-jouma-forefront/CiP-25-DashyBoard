import type { Dayjs } from "dayjs";

export type RoomStatus = "available" | "occupied";
export type Title = "Mrs" | "Ms" | "Mr" | "Mx" | null;

export type Guest = {
  id: string;
  firstName: string;
  lastName: string;
  isPilot: boolean;
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
  hotelId: string;
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

export type Hotel = {
  id: string;
  name: string;
  icaoCode: string;
};

export type DateTime = {
  field: "post" | "expires";
  value: Dayjs | null;
};

export const Day = {
  mon: "Mon",
  tue: "Tue",
  wed: "Wed",
  thu: "Thu",
  fri: "Fri",
  sat: "Sat",
  sun: "Sun",
} as const;

export type Day = (typeof Day)[keyof typeof Day];
