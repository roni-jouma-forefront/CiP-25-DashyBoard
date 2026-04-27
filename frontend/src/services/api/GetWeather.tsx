export type Weather = {
  snow?: string;
  rain?: string;
  fog?: string;
  cloud?: "SKC" | "CLR" | "FEW" | "SCT" | "BKN" | "OVC";
};

export type MetarData = {
  icao: string;
  observed: string;
  station: {
    name: string;
    location: string;
  };
  temperature: {
    celsius: number;
    fahrenheit: number;
  };
  humidity: number;
  windSpeedMps: number;
  weather: Weather | null;
};

export async function GetWeather(icao: string): Promise<MetarData> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/CheckWx/${icao.toUpperCase()}`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Cound't get weather info for ${icao}`);
  }

  const json = await res.json();
  const item = Array.isArray(json)
    ? json[0]
    : (json?.metarData ?? json?.data ?? json);

  return item as MetarData;
}
