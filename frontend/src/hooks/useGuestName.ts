import { useQuery } from "@tanstack/react-query";
import { GetGuest, type GuestNameData } from "../services/api/GetGuest";

interface GuestNameProps {
 guestId: string; 
}

export const useGuestName = ({ guestId }: GuestNameProps) => {
  return useQuery<GuestNameData>({
    queryKey: ["guestId", guestId,],
    queryFn: () => GetGuest(guestId),
    enabled: !!guestId,
  });
};
