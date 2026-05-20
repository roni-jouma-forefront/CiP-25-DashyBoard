import type { GuestNameData } from "./GetGuest";

export async function updateGuestInfo({id, firstName, lastName, isPilot} : GuestNameData) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/Guests/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      id: id,
      firstName: firstName,
      lastName: lastName,
      isPilot: isPilot,
    }),
  });

  if (!res.ok) {
    throw new Error(`Couldn't update guest info`);
  } else {
    return "Updated";
  }
}
