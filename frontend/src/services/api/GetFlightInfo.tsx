export type FlightData = {
  flightId: string;
  departureAirportIcao: string;
  departureAirportSwedish: string;
  arrivalAirportIcao: string;
  arrivalAirportSwedish: string;
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

  const res = await fetch(
    `${apiUrl}/api/Departure/airport/${airport.toUpperCase()}/flight/${flight.toUpperCase()}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Couldn't get flight info for ${flight} from ${airport}`);
  }

  const json = await res.json();
  console.log("FLIGHTINFO", json);
  const item = json[0] ?? [];

  return item as FlightData;
}
