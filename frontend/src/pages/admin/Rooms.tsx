import { Typography } from "@mui/material";
import { RoomsAccordion } from "../../components/admin/RoomsAccordion";

export default function RoomAdminPage() {
  return (
    <>
      <Typography variant="h2">Rooms</Typography>
      <RoomsAccordion />
    </>
  );
}
