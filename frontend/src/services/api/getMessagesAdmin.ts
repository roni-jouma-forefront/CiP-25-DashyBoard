import { mapMessageFromApi } from "../../api/message.adapter";
import type { MessageUI } from "../../types/message.types";

interface GetMessagesProps {
  bookingId?: string;
}

export async function getMessages({
  bookingId,
}: GetMessagesProps): Promise<MessageUI[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  const hotelId = import.meta.env.VITE_HOTEL_ID;

  let url = `${apiUrl}/api/Messages/hotel/${hotelId}`;

  if (bookingId) {
    url += `?bookingId=${bookingId}`;
  }

  const res = await fetch(url, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Couldn't get messages`);
  }

  const json = await res.json();
  const items = Array.isArray(json)
    ? json
    : (json?.messages ?? json?.data ?? json);

  return items.map(mapMessageFromApi);
}
