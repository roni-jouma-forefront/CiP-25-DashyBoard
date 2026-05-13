import { Stack, Typography } from "@mui/material";
import { RoomsAccordion } from "../../components/admin/RoomsAccordion";
import { RoomFilter } from "../../components/admin/RoomFilter";
import { getRoomsWithBookings } from "../../services/api/getRoomsWithBooking";
import { useQuery } from "@tanstack/react-query";
import { useRoomFilter } from "../../hooks";
 
export default function RoomAdminPage() {
  const {
    data: rooms = [],
    isLoading,
    error,
  } = useQuery({
    queryKey: ["rooms"],
    queryFn: getRoomsWithBookings,
  });
 
  const { filteredRooms, filters, filterOpen, handleFilterChange, clearFilters, toggleFilter } =
    useRoomFilter(rooms);
 
  return (
    <>
      <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h2">Rooms</Typography>
        <RoomFilter
          filterOpen={filterOpen}
          filters={filters}
          onToggle={toggleFilter}
          onChange={handleFilterChange}
          onClear={clearFilters}
        />
      </Stack>
      <RoomsAccordion rooms={filteredRooms} error={error} isLoading={isLoading} />
    </>
  );
}
 