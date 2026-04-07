export type DepartureData = {
  flightId: string;
  departureAirportIcao: string | null;
  departureAirportSwedish: string | null;
  arrivalAirportIcao: string | null;
  arrivalAirportSwedish: string | null;
  locationAndStatus: {
    terminal: string | null;
    gate: string | null;
    flightLegStatusEnglish: string | null;
  };
  departureTime: {
    estimatedUtc: string | null;
    scheduledUtc: string | null;
  };
};

export async function GetDepartureFlights(
  airport: string,
): Promise<DepartureData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching departures");
  const today = new Date().toLocaleDateString("sv-SE", {
    timeZone: "Europe/Stockholm",
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });

  const res = await fetch(
    `${apiUrl}/api/Departure/airport/${airport}/${today}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Couldn't get info for departure flights`);
  }

  const json = await res.json();
  const departuresFiltered = (json as DepartureData[]).filter(
    (flight) => flight.locationAndStatus?.flightLegStatusEnglish !== "Deleted",
  );
  return departuresFiltered;
}
