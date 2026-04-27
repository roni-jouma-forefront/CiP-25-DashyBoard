export type BookingsData = {
  id: string;
  roomId: string;
  guestId: string;
  flightId: string;
  numberOfGuests: number;
  checkIn: string;
  checkOut: string;
  bookingStatus: number;
};

export async function GetBookings(
  roomId: string,
): Promise<BookingsData | undefined> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  //   const today = new Date().toLocaleDateString("sv-SE", {
  //     year: "numeric",
  //     month: "2-digit",
  //     day: "2-digit",
  //   });

  const res = await fetch(`${apiUrl}/api/Bookings`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Couldn't get bookings info`);
  }

  const json = (await res.json()) as BookingsData[];
  const bookingsFiltered = json.find((x) => x.roomId === roomId);

  return bookingsFiltered;
}
