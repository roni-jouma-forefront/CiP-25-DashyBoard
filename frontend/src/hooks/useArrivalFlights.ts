import { useQuery } from "@tanstack/react-query";
import { refetchInterval } from "./index";
import {
  GetArrivalFlights,
  type ArrivalsData,
} from "../services/api/GetArrivalFlights";

interface ArrivalFlightsProps {
  airport?: string;
}

export const useArrivalFlights = ({
  airport = "ARN",
}: ArrivalFlightsProps = {}) => {
  return useQuery<ArrivalsData[]>({
    queryKey: ["arrivals", airport],
    queryFn: () => GetArrivalFlights(airport),
    enabled: Boolean(airport),
    staleTime: 1000 * 60 * 5,
    refetchInterval: refetchInterval,
  });
};
