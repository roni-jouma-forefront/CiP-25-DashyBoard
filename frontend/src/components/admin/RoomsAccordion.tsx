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

type RoomStatus = "available" | "occupied";
type Title = "Mrs" | "Miss" | "MS" | "Mr" | null;

type Room = {
  id: string;
  number: number;
  status: RoomStatus;
  title: Title | null;
  guestFirstName: string | null;
  guestLastName: string | null;
  flight: string | null;
};

const mockRooms: Room[] = [
  {
    id: "1",
    number: 101,
    status: "occupied",
    title: "Mr",
    guestFirstName: "Arne",
    guestLastName: "Andersson",
    flight: "SK123",
  },
  {
    id: "2",
    number: 112,
    status: "available",
    title: null,
    guestFirstName: null,
    guestLastName: null,
    flight: null,
  },
];

const statusColor: Record<RoomStatus, string> = {
  occupied: "#f3797e",
  available: "#7978E9",
};

const badgeStyle = (status: RoomStatus) => ({
  display: "inline-block",
  lineHeight: 1,
  backgroundColor: statusColor[status],
  color: "white",
  py: 0.5,
  px: 1.5,
  borderRadius: 3,
  fontWeight: 600,
  fontSize: "0.75rem",
});

export const RoomsAccordion = () => {
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
      {mockRooms.map((room) => (
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
              <Typography component="span" sx={{ flex: 1 }}>
                {room.number}
              </Typography>
              <Stack direction="row" spacing={2} alignItems="center">
                <Button component={NavLink} to={`/admin/rooms/${room.id}`}>
                  Edit Room
                </Button>
                <Typography component="span" sx={badgeStyle(room.status)}>
                  {room.status}
                </Typography>
              </Stack>
            </Stack>
          </AccordionSummary>
          <AccordionDetails>
            <Stack direction="row" spacing={2} alignItems="flex-start">
              <Typography>
                Name: {room.guestFirstName} {room.guestLastName}
              </Typography>
            </Stack>
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
