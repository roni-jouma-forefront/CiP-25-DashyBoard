import { useQuery } from "@tanstack/react-query";
import { refetchInterval } from "./index";
import {
  GetDepartureFlights,
  type DepartureData,
} from "../services/api/GetDepartureFlights";

interface DepartureFlightsProps {
  airport?: string;
}

export const useDepartureFlights = ({
  airport = "ARN",
}: DepartureFlightsProps = {}) => {
  return useQuery<DepartureData[]>({
    queryKey: ["departures", airport],
    queryFn: () => GetDepartureFlights(airport),
    enabled: Boolean(airport),
    staleTime: 1000 * 60 * 5,
    refetchInterval: refetchInterval,
    refetchIntervalInBackground: true,
  });
};
