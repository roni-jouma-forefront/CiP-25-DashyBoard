import type { MessageBackend, MessageUI } from "../types/message.types";
import { formatDateTime } from "../utils/FormatTime";

export const mapMessageFromApi = (msg: MessageBackend): MessageUI => {
  return {
    id: msg.id,
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: msg.bookingId ? msg.bookingId : null,
    title: msg.title,
    content: msg.content,
    status: msg.isActive ? "posted" : "pending",
    postDateTime: msg.postAt ? formatDateTime(msg.postAt) : null,
    expiresAtDateTime: msg.expiresAt ? formatDateTime(msg.expiresAt) : null,
    isActive: msg.isActive,
    author: msg.author,
  };
};
