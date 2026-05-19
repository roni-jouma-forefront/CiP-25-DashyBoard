import { useQuery } from "@tanstack/react-query";
import { GetArrivalFlights, type ArrivalsData } from "../services/api/GetArrivalFlights";

interface ArrivalFlightsProps {
    airport?: string
}

export const useArrivalFlights = ({airport = "ARN"}: ArrivalFlightsProps = {}) => {
    return useQuery<ArrivalsData[]>({
    queryKey: ["arrivals", airport],
    queryFn: () => GetArrivalFlights(airport),
    enabled: Boolean(airport),
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    })
}