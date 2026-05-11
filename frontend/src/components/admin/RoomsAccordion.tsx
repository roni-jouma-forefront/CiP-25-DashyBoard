import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Box,
  Button,
  Stack,
  Typography,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { NavLink } from "react-router";
import RadioButtonCheckedIcon from "@mui/icons-material/RadioButtonChecked";
import CircleOutlinedIcon from "@mui/icons-material/CircleOutlined";
import type { Room } from "../../types/types";

interface RoomsAccordionProps {
  rooms: Room[];
  isLoading: boolean;
  error: Error | null;
}

export const RoomsAccordion = ({
  rooms,
  isLoading,
  error,
}: RoomsAccordionProps) => {
  if (isLoading) return <Typography>Loading rooms...</Typography>;
  if (error) return <Typography color="error">Error loading rooms</Typography>;

  return (
    <Box
      sx={{
        display: "flex",
        gap: 2,
        flexDirection: "column",
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
        marginRight: "10px",
      }}
    >
      {rooms.map((room) => (
        <Accordion key={room.id}>
          <AccordionSummary
            expandIcon={<ExpandMoreIcon />}
            aria-controls={`panel-${room.id}-content`}
            id={`panel-${room.id}-header`}
          >
            <Stack
              direction="row"
              spacing={2}
              alignItems="center"
              justifyContent="space-between"
              width={1}
            >
              {room.activeBooking ? (
                <RadioButtonCheckedIcon />
              ) : (
                <CircleOutlinedIcon />
              )}
              <Stack
                direction="row"
                spacing={2}
                alignItems="center"
                sx={{ flex: 1 }}
              >
                <Typography component="span" fontWeight="600">
                  {room.roomNumber}
                </Typography>
                {room.activeBooking?.guest && (
                  <Typography variant="body2">
                    {room.activeBooking.guest.firstName}{" "}
                    {room.activeBooking.guest.lastName}
                  </Typography>
                )}
              </Stack>
              <Button
                component={NavLink}
                to={`/admin/rooms/${room.id}/${room.roomNumber}${room.activeBooking?.id ? `/${room.activeBooking.id}` : ""}`}
              >
                Edit Room
              </Button>
            </Stack>
          </AccordionSummary>
          <AccordionDetails>
            <Stack
              direction="row"
              spacing={2}
              alignItems="flex-start"
              paddingTop={1}
            >
              <Typography>
                Departing Flight:{" "}
                {room.activeBooking?.flightNumber ?? "No departing flight"}
              </Typography>
            </Stack>
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
