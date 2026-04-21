export type MessagesData = {
  id: string;
  hotelId: string;
  bookingId: string;
  content: string;
  expiresAt: Date;
  isActive: boolean;
  createdAt: Date;
};

export async function GetMessages(
  hotelId: string,
  bookingId: string,
): Promise<MessagesData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching messages");

  const res = await fetch(`${apiUrl}/api/Messages`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Cound't get hotelId ${hotelId} or bookingId ${bookingId}`);
  }

  const json = await res.json();
  const item = Array.isArray(json)
    ? json[0]
    : (json?.metarData ?? json?.data ?? json);

  return item as MessagesData;
}
