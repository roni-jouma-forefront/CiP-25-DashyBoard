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
    staleTime: 5*60*1000,
    refetchInterval: 5*60*1000,
    refetchIntervalInBackground: true,
    })
}