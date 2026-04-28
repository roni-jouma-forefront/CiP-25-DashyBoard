export async function postBookings(FormData: FormData) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  const url = `${apiUrl}/api/Bookings/import`;

  const res = await fetch(url, {
    method: "POST",
    body: FormData,
  });

  if (!res.ok) {
    throw new Error(`Couldn't post bookings file`);
  } else {
    return "Posted";
  }
}
