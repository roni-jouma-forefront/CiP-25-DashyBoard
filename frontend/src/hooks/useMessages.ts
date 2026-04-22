import { useQuery } from "@tanstack/react-query";
import { GetMessages, type MessagesData} from "../services/api/GetMessages.tsx";
 
interface MessagesProps  {
    hotelId: string,
    bookingId: string
  }
 
export const useMessages = ({ hotelId, bookingId }: MessagesProps) => {
    return useQuery<MessagesData[]>({
    queryKey: ["hotelId", hotelId, "bookingId", bookingId],
    queryFn: () => GetMessages( hotelId, bookingId ),
    enabled: Boolean(hotelId),
    refetchInterval: 5*60*1000,
    refetchIntervalInBackground: true,
    })
}