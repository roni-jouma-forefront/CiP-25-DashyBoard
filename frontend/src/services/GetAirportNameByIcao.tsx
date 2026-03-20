import Papa from "papaparse";

interface Airport {
  id: number;
  type: string;
  name: string;
  icao_code: string | null;
}

export function GetAirportNameByIcao(icao: string): Promise<string | null> {
  return new Promise((resolve, reject) => {
    Papa.parse<Airport>("/data/airports.csv", {
      download: true,
      header: true,
      skipEmptyLines: true,
      complete: (results) => {
        const match = results.data.find(
          (a) => a.icao_code?.toUpperCase() === icao.toUpperCase(),
        );

        resolve(match?.name ?? null);
      },
      error: reject,
    });
  });
}
