export type MessagesData = {
  id: string;
  hotelId: string;
  bookingId: string;
  title: string;
  content: string;
  expiresAt: string;
  isActive: boolean;
  createdAt: string;
};

export async function GetMessages(
  hotelId: string,
  bookingId: string,
): Promise<MessagesData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching messages");

  const res = await fetch(`${apiUrl}/api/Messages`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Cound't get hotelId ${hotelId} or bookingId ${bookingId}`);
  }

  const json = await res.json();
  const MessagesData = json as MessagesData[];

  return MessagesData;
}
