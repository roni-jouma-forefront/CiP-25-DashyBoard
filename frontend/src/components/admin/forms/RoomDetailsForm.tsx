import { Box, Stack, Typography } from "@mui/material";
import { AdditionalGuestList } from "../AdditionalGuestList";
import type { AdditionalGuest } from "../../../types/types";

interface RoomDetailsProps {
  bookingId?: string | null;
  firstName?: string;
  lastName?: string;
  title?: string;
  departureFlight?: string;
  departureDate?: string;
  additionalGuests?: AdditionalGuest[];
}


export const RoomDetailsForm = ({
  bookingId,
  firstName,
  lastName,
  title,
  departureFlight,
  departureDate,
  additionalGuests = [],
}: RoomDetailsProps) => {
  return (
    <Box
      sx={{
        flex: 1,
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
    >
      <Typography variant="h5" mb={3}>
        Room Details
      </Typography>
      <Stack>
        <Stack direction="row" spacing={2} alignItems="center">
          <Typography variant="body2" sx={{ flexShrink: 0 }}>
            Booking Id:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {bookingId ?? "—"}
          </Typography>
        </Stack>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mt: 3 }}>
          <Typography variant="body2" sx={{ flexShrink: 0 }}>
            First Name:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {firstName ?? "—"}
          </Typography>
        </Stack>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mt: 3 }}>
          <Typography sx={{ flexShrink: 0 }} variant="body2">
            Last Name:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {lastName ?? "—"}
          </Typography>
        </Stack>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mt: 3 }}>
          <Typography sx={{ flexShrink: 0 }} variant="body2">
            Title:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {title ?? "—"}
          </Typography>
        </Stack>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mt: 3 }}>
          <Typography sx={{ flexShrink: 0 }} variant="body2">
            Departure Flight:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {departureFlight ?? "—"}
          </Typography>
        </Stack>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mt: 3 }}>
          <Typography sx={{ flexShrink: 0 }} variant="body2">
            Departure Date:
          </Typography>
          <Typography sx={{ mt: 1, fontWeight: 600 }} variant="body1">
            {departureDate ?? "—"}
          </Typography>
        </Stack>
        <Typography sx={{ flexShrink: 0, mt: 3 }} variant="body2">
          Additional Guest(s)
        </Typography>
        <AdditionalGuestList guests={additionalGuests} />
      </Stack>
    </Box>
  );
};
