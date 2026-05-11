import { useQuery } from "@tanstack/react-query";
import { GetGuestName, type GuestNameData } from "../services/api/GetGuestName";

interface GuestNameProps {
 guestId: string; 
}

export const useGuestName = ({ guestId }: GuestNameProps) => {
  return useQuery<GuestNameData>({
    queryKey: ["guestId", guestId,],
    queryFn: () => GetGuestName(guestId),
    enabled: !!guestId,
  });
};
