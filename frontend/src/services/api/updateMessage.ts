import type { MessageBackend } from "../../types/message.types";

export async function updateMessage(data: MessageBackend) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/messages/${data.id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      id: data.id,
      hotelId: import.meta.env.VITE_HOTEL_ID,
      bookingId: data.bookingId,
      title: data.title,
      content: data.content,
      postedBy: data.author,
      postAt: data.postAt,
      expiresAt: data.expiresAt,
    }),
  });

  if (!res.ok) {
    throw new Error(`Couldn't update messages`);
  } else {
    return "Updated";
  }
}
