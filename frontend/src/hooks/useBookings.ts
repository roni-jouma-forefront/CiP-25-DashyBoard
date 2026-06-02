import { useQuery } from "@tanstack/react-query";
import { GetBookings, type BookingsData } from "../services/api/GetBookings";

type BookingProp = {
    bookingId: string; 
}

export const useBookings = ( { bookingId } :BookingProp ) => {
    return useQuery<BookingsData>({
    queryKey: ["bookings", bookingId],
    queryFn: () => { if(!bookingId) {throw new Error("RoomId missing");
    }
        return GetBookings(bookingId);
    },
    enabled: !!bookingId,
    })
}