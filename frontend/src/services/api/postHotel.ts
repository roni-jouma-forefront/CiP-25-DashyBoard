import type { Hotel } from "../../types/types";

export async function postHotel(data: Hotel) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  const url = `${apiUrl}/api/Hotels`;

  const res = await fetch(url, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      name: data.name,
      icaoCode: data.icaoCode,
    }),
  });

  if (!res.ok) {
    throw new Error(`Couldn't post hotel`);
  } else {
    return "Posted";
  }
}
