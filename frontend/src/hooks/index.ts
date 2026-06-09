export { useWeather } from "./useWeather";
export { useFlightInfo } from "./useFlightInfo";
export { useMessagesAdmin } from "./useMessagesAdmin";
export { useDepartureFlights } from "./useDepartureFlights";
export { useArrivalFlights } from "./useArrivalFlights";
export { useMessages } from "./useMessages";
export { useBookings } from "./useBookings";
export { useAllBookings } from "./useAllBookings";
export { useWaitTimes } from "./useWaitTimes";
export { useRoomFilter } from "./useRoomFilter";

export const refetchInterval: number | false = 1000 * 60 * 60 * 6; // Refetch every 6 hours (21,600,000 milliseconds)
//3 minutes in milliseconds: 1000 * 60 * 3 = 180_000
