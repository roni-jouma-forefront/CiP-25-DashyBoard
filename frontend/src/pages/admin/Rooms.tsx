import { Alert, Typography } from "@mui/material";
import { RoomsAccordion } from "../../components/admin/RoomsAccordion";
import { getRoomsWithBookings } from "../../services/api/getRoomsWithBooking";
import { useQuery } from "@tanstack/react-query";

export default function RoomAdminPage() {
  const {
    data: rooms = [],
    isLoading,
    error,
  } = useQuery({
    queryKey: ["rooms"],
    queryFn: getRoomsWithBookings,
  });

  return (
    <>
      <Typography variant="h2">Rooms</Typography>
      <Alert severity="info">
        Search and filtering features are currently in development
      </Alert>
      <RoomsAccordion rooms={rooms} error={error} isLoading={isLoading} />
    </>
  );
}
