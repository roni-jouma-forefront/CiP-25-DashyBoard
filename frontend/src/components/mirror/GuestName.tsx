import { Box, Typography } from "@mui/material";
import { widgetTheme } from "../../theme";
import { useGuestName } from "../../hooks/useGuestName";

interface GuestNameProps {
  guestId: string;
}

export default function GuestName({ guestId }: GuestNameProps) {
  const { data, error, isLoading } = useGuestName({
    guestId,
  });

  if (error)
    return (
      <Typography
        sx={{
          m: 3,
          opacity: 0.9,
          color: `${widgetTheme.palette.primary.main}`,
        }}
      >
        Error: {error.message}
      </Typography>
    );
  if (isLoading)
    return (
      <Typography
        sx={{
          m: 3,
          opacity: 0.9,
          color: `${widgetTheme.palette.primary.main}`,
        }}
      >
        Loading guest name info...
      </Typography>
    );

  return (
    <Box
      sx={{
        display: "flex",
        justifyContent: "center",
        m: 2,
      }}
    >
      <Typography
        variant="h3"
        sx={{ color: `${widgetTheme.palette.primary.main}` }}
      >
        Welcome {data?.firstName} {data?.lastName}
      </Typography>
    </Box>
  );
}
