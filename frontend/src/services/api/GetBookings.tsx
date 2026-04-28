export type BookingsData = {
  id: string;
  roomId: string;
  guestId: string;
  flightNumber: string;
  numberOfGuests: number;
  checkIn: string;
  checkOut: string;
  bookingStatus: number;
};

export async function GetBookings(bookingId: string): Promise<BookingsData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/Bookings/${bookingId}`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Couldn't get bookings info`);
  }
  const json = (await res.json()) as BookingsData;

  return json;
}
