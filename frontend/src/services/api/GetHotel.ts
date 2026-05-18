export type HotelData = {
  id: string;
  name: string;
  icaoCode: string;
};

export async function GetHotel(hotelId: string): Promise<HotelData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/Hotels/hotel/${hotelId}`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error("Couldn't get hotel info");
  }

  return (await res.json()) as HotelData;
}
