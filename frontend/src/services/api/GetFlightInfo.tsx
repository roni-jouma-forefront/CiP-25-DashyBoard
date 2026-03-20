export type FlightData = {
  flightId: string;
  departureAirportIcao: string;
  arrivalAirportIcao: string;
  locationAndStatus: {
    terminal: string;
    gate: string;
    flightLegStatusEnglish: string;
  };
  arrivalTime: {
    estimatedUtc: string;
    scheduledUtc: string;
  };
  departureTime: {
    estimatedUtc: string;
    scheduledUtc: string;
  };
};

export async function GetFlightInfo(
  airport: string,
  flight: string,
): Promise<FlightData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching flight info");

  const res = await fetch(
    `${apiUrl}/api/Departure/airport/${airport.toUpperCase()}/flight/${flight.toUpperCase()}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Cound't get flight info for ${flight} from ${airport}`);
  }

  const json = await res.json();
  const item = Array.isArray(json)
    ? json[0]
    : (json?.metarData ?? json?.data ?? json);

  return item as FlightData;
}
