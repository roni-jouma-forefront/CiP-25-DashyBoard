import type { Room } from "../../types/types";

export async function getRoomsWithBookings(): Promise<Room[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  const hotelId = import.meta.env.VITE_HOTEL_ID;

  const res = await fetch(
    `${apiUrl}/api/Rooms/hotel/${hotelId}/with-bookings`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Cound't get rooms for hotel with id ${hotelId}`);
  }

  const json = await res.json();
  const rooms = Array.isArray(json)
    ? json
    : (json?.rooms ?? json?.data ?? json);

  return rooms;
}
