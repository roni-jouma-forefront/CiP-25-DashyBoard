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
  conditions: string | null;
};

export async function GetWeather(icao: string): Promise<MetarData> {
  const res = await fetch(
    `http://localhost:5000/api/CheckWx/${icao.toUpperCase()}`,
    {
      headers: { "Content-Type": "application/json" },
    },
  );

  if (!res.ok) {
    throw new Error(`Cound't get weather info for ${icao}`);
  }

  const json = await res.json();
  const item = Array.isArray(json)
    ? json[0]
    : (json?.metarData ?? json?.data ?? json);

  return item as MetarData;
}
