import dayjs from "dayjs";
import type { MessageBackend, MessageUI } from "../types/message.types";

export const mapMessageFromApi = (msg: MessageBackend): MessageUI => {
  return {
    id: msg.id,
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: msg.bookingId ? msg.bookingId : null,
    title: msg.title,
    content: msg.content,
    status: msg.isActive ? "posted" : "pending",
    postDateTime: msg.postAt
      ? dayjs(msg.postAt).format("YYYY.MM-DD HH:mm")
      : null,
    expiresAtDateTime: msg.expiresAt
      ? dayjs(msg.expiresAt).format("YYYY.MM-DD HH:mm")
      : null,
    isActive: msg.isActive,
    author: msg.author,
  };
};
