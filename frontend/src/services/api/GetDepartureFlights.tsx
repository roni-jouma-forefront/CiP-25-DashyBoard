export type DepartureData = {
  flightId: string;
  departureAirportIcao: string;
  departureAirportSwedish: string;
  arrivalAirportIcao: string;
  arrivalAirportSwedish: string;
  locationAndStatus: {
    terminal: string | null;
    gate: string | null;
    flightLegStatusEnglish: string;
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
  const today = new Date().toISOString().split("T")[0];

  const res = await fetch(
    `${apiUrl}/api/Departure/airport/${airport}/${today}`,

    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Cound't get info for departure flights`);
  }

  const json = await res.json();
  const departuresFiltered = (json as DepartureData[]).filter(
    (flight) => flight.locationAndStatus?.flightLegStatusEnglish !== "Deleted",
  );
  console.log("DEPART ", departuresFiltered);
  return departuresFiltered;
}
