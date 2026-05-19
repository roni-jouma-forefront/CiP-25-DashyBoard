import { useQuery } from "@tanstack/react-query";
import { GetDepartureFlights, type DepartureData } from "../services/api/GetDepartureFlights";

interface DepartureFlightsProps {
    airport?: string
}

export const useDepartureFlights = ({airport = "ARN"}: DepartureFlightsProps = {}) => {
    return useQuery<DepartureData[]>({
    queryKey: ["departures", airport],
    queryFn: () => GetDepartureFlights(airport),
    enabled: Boolean(airport),
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    })
}