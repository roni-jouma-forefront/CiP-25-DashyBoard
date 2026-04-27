import { useQuery } from "@tanstack/react-query";
import { GetMessages, type MessagesData} from "../services/api/GetMessages.tsx";
 
interface MessagesProps  {
    hotelId: string,
    roomId: string
  }
 
export const useMessages = ({ hotelId, roomId }: MessagesProps) => {
    return useQuery<MessagesData[]>({
    queryKey: ["hotelId", hotelId, "bookingId", roomId],
    queryFn: () => GetMessages( hotelId, roomId ),
    enabled: Boolean(hotelId),
    refetchInterval: 5*60*1000,
    refetchIntervalInBackground: true,
    })
}