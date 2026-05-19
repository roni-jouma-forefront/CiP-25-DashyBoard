import { useQuery } from "@tanstack/react-query";
import { GetMessages, type MessagesData} from "../services/api/GetMessages.tsx";
 
interface MessagesProps  {
    hotelId: string,
    roomId: string
  }
 
export const useMessages = ({ hotelId, roomId }: MessagesProps) => {
    return useQuery<MessagesData[]>({
    queryKey: ["hotelId", hotelId, "roomId", roomId, "messages"],
    queryFn: () => GetMessages( hotelId, roomId ),
    enabled: Boolean(hotelId),
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    })
}