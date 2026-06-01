import type { MsgStatus } from "./theme.types";

export type MessageBackend = {
  id: string;
  hotelId: string;
  bookingId: string | null;
  title: string;
  content: string;
  recurring: boolean;
  recurrenceType: string | null;
  postAt: string | null;
  expiresAt: string | null;
  isActive: boolean;
  recurrenceTimeStart: string | null;
  recurrenceTimeEnd: string | null;
  recurrenceDays: string | null;
  author: string;
};

export type MessageUI = {
  id: string;
  hotelId: string;
  bookingId: string | null;
  title: string;
  content: string;
  recurring: boolean;
  status: MsgStatus;
  postDateTime: string | null;
  postAt: string | null;
  expiresAtDateTime: string | null;
  expiresAt: string | null;
  recurrenceTimeStart: string | null;
  recurrenceTimeEnd: string | null;
  recurrenceDays: string | null;
  isActive: boolean;
  author: string;
};
