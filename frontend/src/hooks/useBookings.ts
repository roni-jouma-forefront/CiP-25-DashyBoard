import { useQuery } from "@tanstack/react-query";
import { GetBookings, type BookingsData } from "../services/api/GetBookings";


export const useBookings = ({roomId = "20000000-0000-0000-0000-000000000101"}) => {
    return useQuery<BookingsData | undefined>({
    queryKey: ["bookings", roomId],
    queryFn: () => GetBookings(roomId),
    enabled: Boolean(roomId),
    // staleTime: 5*60*1000,
    // refetchInterval: 5*60*1000,
    // refetchIntervalInBackground: true,
    })
}