import {
  Box,
  FormControlLabel,
  Stack,
  Switch,
  Typography,
} from "@mui/material";
import { Link } from "react-router";
import { AdditionalGuestList } from "../AdditionalGuestList";
import type { AdditionalGuest } from "../../../types/types";
import { updateGuestInfo } from "../../../services/api/updateGuest";
import { useQueryClient } from "@tanstack/react-query";

interface RoomDetailsProps {
  bookingId?: string | null;
  guestId: string;
  firstName: string;
  lastName: string;
  isPilot: boolean;
  departureFlight?: string;
  departureDate?: string;
  additionalGuests?: AdditionalGuest[];
}

export const RoomDetailsForm = ({
  bookingId,
  guestId,
  firstName,
  lastName,
  isPilot,
  departureFlight,
  departureDate,
  additionalGuests = [],
}: RoomDetailsProps) => {
  const queryClient = useQueryClient();

  const handleToggle = async (e: React.ChangeEvent<HTMLInputElement>) => {
    await updateGuestInfo({
      id: guestId,
      firstName: firstName,
      lastName: lastName,
      isPilot: e.target.checked,
    });

    queryClient.invalidateQueries({ queryKey: ["guestId", guestId] });
  };

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
            {bookingId ? (
              <Link to={`/mirror/${bookingId}`} style={{ color: "inherit" }}>
                {bookingId}
              </Link>
            ) : (
              "—"
            )}
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
            Is Pilot:
          </Typography>

          <Switch checked={isPilot} name="isPilot" onChange={handleToggle} />
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
