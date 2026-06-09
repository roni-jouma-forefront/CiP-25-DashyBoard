import { useQuery } from "@tanstack/react-query";
import { getAllBookings } from "../services/api/getAllBookings";
import type { BookingsData } from "../services/api/GetBookings";

export const useAllBookings = (bookingStatus?: number) => {
  return useQuery<BookingsData[]>({
    queryKey: ["allBookings", bookingStatus],
    queryFn: () => getAllBookings(bookingStatus),
  });
};
