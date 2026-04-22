import type { MsgStatus } from "./theme.types";

export type MessageBackend = {
  id: string;
  hotelId: string;
  bookingId: string | null;
  title: string;
  content: string;
  recurring: boolean;
  postAt: string | null;
  expiresAt: string | null;
  isActive: boolean;
  author: string;
};

// Återkommande meddelanden? bocka i återkommande

export type MessageUI = {
  id: string;
  hotelId: string;
  bookingId: string | null;
  title: string;
  content: string;
  status: MsgStatus;
  postDateTime: string | null;
  expiresAtDateTime: string | null;
  isActive: boolean;
  author: string;
};
