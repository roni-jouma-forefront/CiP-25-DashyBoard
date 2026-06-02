import type { MessageBackend, MessageUI } from "../types/message.types";
import { formatDateTime } from "../utils/FormatTime";

type MessageApiResponse = MessageBackend & { postedBy?: string };

const getMsgStatus = (msg: MessageApiResponse): MessageUI["status"] => {
  if (msg.expiresAt && new Date(msg.expiresAt) < new Date()) return "expired";
  if (msg.isActive) return "posted";
  return "pending";
};

export const mapMessageFromApi = (msg: MessageApiResponse): MessageUI => {
  return {
    id: msg.id,
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: msg.bookingId ? msg.bookingId : null,
    title: msg.title,
    content: msg.content,
    recurring: msg.recurrenceType !== null && msg.recurrenceType !== "None",
    status: getMsgStatus(msg),
    postDateTime: msg.postAt ? formatDateTime(msg.postAt) : null,
    postAt: msg.postAt,
    expiresAtDateTime: msg.expiresAt ? formatDateTime(msg.expiresAt) : null,
    expiresAt: msg.expiresAt,
    isActive: msg.isActive,
    author: msg.postedBy ?? msg.author ?? "",
    recurrenceTimeStart: msg.recurrenceTimeStart
      ? msg.recurrenceTimeStart
      : null,
    recurrenceTimeEnd: msg.recurrenceTimeEnd ? msg.recurrenceTimeEnd : null,
    recurrenceDays: msg.recurrenceDays ? msg.recurrenceDays : null,
  };
};
