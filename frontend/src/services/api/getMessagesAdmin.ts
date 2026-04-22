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

  let res;

  if (bookingId) {
    res = await fetch(
      `${apiUrl}/api/messages?hotelId=${hotelId}&bookingId=${bookingId}`,
      {
        headers: { "Content-Type": "application/json" },
      },
    );
  } else {
    res = await fetch(`${apiUrl}/api/messages/`, {
      headers: { "Content-Type": "application/json" },
    });
  }

  if (!res.ok) {
    throw new Error(`Cound't get messages`);
  }

  const json = await res.json();
  const items = Array.isArray(json)
    ? json
    : (json?.messages ?? json?.data ?? json);

  console.log(items.map(mapMessageFromApi));
  return items.map(mapMessageFromApi);
}
