import { useQuery } from "@tanstack/react-query";
import { GetWaitTimes, type WaitTimesData } from "../services/api/GetWaitTimes";

interface WaitTimesProps {
    airport?: string
}

export const useWaitTimes = ({airport = "ARN"}: WaitTimesProps = {}) => {
    return useQuery<WaitTimesData[]>({
    queryKey: ["waittimes", airport],
    queryFn: () => GetWaitTimes(airport),
    enabled: Boolean(airport),
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
    })
}