import { useQuery } from "@tanstack/react-query";
import { GetWeather, type MetarData } from "../services/api/GetWeather";

interface WeatherProps {
  icao: string;
}

export const useWeather = ({ icao }: WeatherProps) => {
  return useQuery<MetarData>({
    queryKey: ["weather", icao],
    queryFn: () => GetWeather(icao),
    enabled: /^[A-Za-z]{4}$/.test(icao),
    staleTime: 1000 * 60 * 5,
    refetchInterval: 1000 * 60 * 15,
    refetchIntervalInBackground: false,
  });
};
