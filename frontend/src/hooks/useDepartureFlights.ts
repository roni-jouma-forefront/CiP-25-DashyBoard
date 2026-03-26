import { useQuery } from "@tanstack/react-query";
import { GetArrivalFlights, type DepartureData } from "../services/api/GetDepartureFlights";

interface DepartureFlightsProps {
    airport?: string
}

export const useDepartureFlights = ({airport = "ARN"}: DepartureFlightsProps = {}) => {
    return useQuery<DepartureData[]>({
    queryKey: ["arrivals", airport],
    queryFn: () => GetArrivalFlights(airport),
    enabled: Boolean(airport),
    staleTime: 5*60*1000,
    refetchInterval: 5*60*1000,
    refetchIntervalInBackground: true,
    })
}