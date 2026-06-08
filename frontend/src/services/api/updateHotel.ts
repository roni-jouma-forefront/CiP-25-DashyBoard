import type { Hotel } from "../../types/types";

export async function updateHotel(hotelId: string, data: Hotel) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  const url = `${apiUrl}/api/Hotels/hotel/${hotelId}`;

  const res = await fetch(url, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: data.name,
      icaoCode: data.iataCode,
    }),
  });

  if (!res.ok) {
    throw new Error("Couldn't update hotel");
  }

  return "Updated";
}
