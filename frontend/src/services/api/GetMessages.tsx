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
  roomId: string,
): Promise<MessagesData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching messages");

  const res = await fetch(
    `${apiUrl}/api/Messages/hotel/${hotelId}/room/${roomId}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Cound't get hotelId ${hotelId} or roomId ${roomId}`);
  }

  const json = await res.json();
  const MessagesData = (json as MessagesData[]).filter(
    (message) => message?.isActive !== false,
  );

  return MessagesData;
}
