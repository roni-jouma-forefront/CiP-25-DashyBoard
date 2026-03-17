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
    number: 102,
    status: "occupied",
    title: "Mx",
    guestFirstName: "Cookie",
    guestLastName: "Larsson",
    flight: "BA777",
  },
  {
    id: "3",
    number: 103,
    status: "occupied",
    title: "Ms",
    guestFirstName: "Karin",
    guestLastName: "Karinsson",
    flight: "BA777",
  },
  {
    id: "4",
    number: 104,
    status: "available",
    title: null,
    guestFirstName: null,
    guestLastName: null,
    flight: null,
  },
];

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
              {room.status === "occupied" ? (
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
                <Typography component="span" fontWeight="600">{room.number}</Typography>
                {room.status === "occupied" && (
                  <Typography variant="body2">
                    {room.guestFirstName} {room.guestLastName}
                  </Typography>
                )}
              </Stack>
              <Button component={NavLink} to={`/admin/rooms/${room.id}`}>
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
                Departing Flight: {room.flight}
              </Typography>
            </Stack>
          </AccordionDetails>
        </Accordion>
      ))}
    </Box>
  );
};
