import { Box, Typography, Paper, Stack } from "@mui/material";
import { useDepartureFlights } from "../../hooks";

function formatTime(utc: string | null | undefined) {
  if (!utc) return "-";
  return new Date(utc).toLocaleTimeString("sv-SE", {
    hour: "2-digit",
    minute: "2-digit",
    timeZone: "Europe/Stockholm",
  });
}

export default function DeparturesWidget() {
  const today = new Date().toLocaleDateString("en-GB", {
    month: "long",
    day: "numeric",
  });

  const {
    data: departures = [],
    error,
    isLoading,
  } = useDepartureFlights({
    airport: "ARN",
  });

  if (error) return <Typography>Error: {error.message}</Typography>;
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9 }}>
        Loading departures info...
      </Typography>
    );

  return (
    <Box
      sx={{
        position: "relative",
        p: 2,
        m: 2,
        borderRadius: 2,
        border: "5px solid white",
        boxShadow: 1,
        color: "white",
        backgroundColor: "black",
      }}
    >
      <Box sx={{ mb: 2 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
          }}
        >
          <Typography sx={{ fontSize: "1.4rem", fontWeight: 700 }}>
            Departures
          </Typography>
          <Typography sx={{ fontWeight: 700 }}>{today}</Typography>
        </Box>
      </Box>
      <Stack spacing={1.2}>
        {departures.slice(0, 5).map((departure) => {
          return (
            <Paper
              key={departure.flightId}
              sx={{
                p: 1.2,
                borderRadius: 2,
                bgcolor: "rgba(0, 0, 0, 0.9)",
                color: "#ffffff",
                border: "2px solid grey",
              }}
            >
              <Stack direction="row" justifyContent="space-between">
                <Box>
                  <Typography sx={{ fontWeight: 700, fontSize: "0.9rem" }}>
                    {departure.flightId}
                  </Typography>
                  <Typography sx={{ fontSize: "0.75rem", color: "#ffffff" }}>
                    {departure.departureAirportSwedish} to{" "}
                    {departure.arrivalAirportSwedish}
                  </Typography>
                </Box>

                <Box sx={{ textAlign: "right" }}>
                  <Typography sx={{ fontSize: "0.8rem" }}>
                    {formatTime(departure.departureTime?.scheduledUtc) ?? "-"}
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "0.7rem",
                      fontWeight: 600,
                      color: "#3b82f6",
                    }}
                  >
                    {departure.locationAndStatus?.terminal ?? "-"}
                  </Typography>
                </Box>
              </Stack>
            </Paper>
          );
        })}
      </Stack>
    </Box>
  );
}
