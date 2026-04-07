import { useQuery } from "@tanstack/react-query";
import { GetFlightInfo, type FlightData } from "../services/api/GetFlightInfo";

interface FlightProps {
  airport: string;
  flight: string;
}

export const useFlightInfo = ({ airport, flight }: FlightProps) => {
  return useQuery<FlightData>({
    queryKey: ["airport", airport, "flight", flight],
    queryFn: () => GetFlightInfo(airport, flight),
    enabled: !!airport,
    staleTime: 2*5*1000,
  });
};
