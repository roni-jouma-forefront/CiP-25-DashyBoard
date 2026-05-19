export type GuestNameData = {
  id: string;
  firstName: string;
  lastName: string;
  isPilot: boolean;
};

export async function GetGuest(guestId: string): Promise<GuestNameData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/Guests/${guestId}`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Couldn't get name info for ${guestId}`);
  }

  const json = await res.json();

  return json;
}
