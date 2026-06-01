import { useQuery } from "@tanstack/react-query";
import { GetWeather, type MetarData } from "../services/api/GetWeather";

interface WeatherProps {
  icao: string;
}

export const useWeather = ({ icao }: WeatherProps) => {
  return useQuery<MetarData>({
    queryKey: ["weather", icao],
    queryFn: () => GetWeather(icao),
    enabled: !!icao,
    staleTime: 1000 * 60 * 5,
    refetchInterval: 60_000,
    refetchIntervalInBackground: true,
  });
};
