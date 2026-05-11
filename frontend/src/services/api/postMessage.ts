import type { MessageBackend } from "../../types/message.types";

export async function postMessage(data: MessageBackend) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  console.log(import.meta.env.VITE_HOTEL_ID)

  const res = await fetch(`${apiUrl}/api/messages`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
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
    throw new Error(`Couldn't post messages`);
  } else {
    return "Posted";
  }
}
