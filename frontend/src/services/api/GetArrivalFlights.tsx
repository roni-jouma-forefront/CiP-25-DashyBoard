export type ArrivalsData = {
  flightId: string;
  departureAirportIcao: string;
  departureAirportSwedish: string;
  arrivalAirportIcao: string;
  arrivalAirportSwedish: string;
  locationAndStatus: {
    terminal: string | null;
    gate: string | null;
    flightLegStatusEnglish: string | null;
  };
  arrivalTime: {
    estimatedUtc: string | null;
    scheduledUtc: string | null;
  };
  departureTime: string | null;
};

export async function GetArrivalFlights(
  airport: string,
): Promise<ArrivalsData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const today = new Date().toLocaleDateString("sv-SE", {
    year: "numeric",
    month: "2-digit",
    day: "2-digit",
  });

  const res = await fetch(
    `${apiUrl}/api/Arrivals/airport/${airport}/${today}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Couldn't get info for arrival flights`);
  }

  const json = await res.json();
  const arrivalsFiltered = (json as ArrivalsData[]).filter(
    (flight) => flight.locationAndStatus?.flightLegStatusEnglish !== "Deleted",
  );

  const getFlightTime = (flight: ArrivalsData): number => {
    const utc =
      flight.arrivalTime?.scheduledUtc ??
      flight.arrivalTime?.estimatedUtc ??
      flight.departureTime;

    if (!utc) return Number.POSITIVE_INFINITY;

    const parsed = Date.parse(utc);
    return Number.isNaN(parsed) ? Number.POSITIVE_INFINITY : parsed;
  };

  const now = Date.now();

  const sortedArrivals = [...arrivalsFiltered].sort((a, b) => {
    const aTime = getFlightTime(a);
    const bTime = getFlightTime(b);

    const aIsUpcoming = aTime >= now;
    const bIsUpcoming = bTime >= now;

    if (aIsUpcoming !== bIsUpcoming) {
      return aIsUpcoming ? -1 : 1;
    }

    if (aIsUpcoming) {
      return aTime - bTime;
    }

    return bTime - aTime;
  });

  return sortedArrivals;
}
