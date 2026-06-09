import type { BookingsData } from "./GetBookings";

export async function getAllBookings(
  bookingStatus?: number,
): Promise<BookingsData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const params = new URLSearchParams();
  if (bookingStatus !== undefined) {
    params.set("bookingStatus", String(bookingStatus));
  }

  const query = params.toString();
  const url = `${apiUrl}/api/Bookings${query ? `?${query}` : ""}`;

  const res = await fetch(url, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error("Couldn't get bookings");
  }

  const json = (await res.json()) as BookingsData[];
  return json;
}
