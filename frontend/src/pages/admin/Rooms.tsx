import { Alert, Typography } from "@mui/material";
import { RoomsAccordion } from "../../components/admin/RoomsAccordion";

export default function RoomAdminPage() {
  return (
    <>
      <Typography variant="h2">Rooms</Typography>
      <Alert severity="info">Search and filtering features are currently in development</Alert>
      <RoomsAccordion />
    </>
  );
}
