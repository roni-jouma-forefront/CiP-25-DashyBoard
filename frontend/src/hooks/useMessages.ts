import { useQuery } from "@tanstack/react-query";
import { GetMessages, type MessagesData} from "../services/api/GetMessages";

interface MessagesProps  {
    hotelId: string, 
    bookingId: string
  }

export const useMessages = ({ hotelId, bookingId }: MessagesProps) => {
    return useQuery<MessagesData[]>({
    queryKey: ["hotelId", hotelId, "bookingId", bookingId],
    queryFn: () => GetMessages( hotelId, bookingId ),
    enabled: Boolean(hotelId),
    // staleTime: 5*60*1000,
    refetchInterval: 5*60*1000,
    refetchIntervalInBackground: true,
    })
}