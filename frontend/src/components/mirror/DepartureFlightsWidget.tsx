import { Box, Typography, Paper, Stack, Chip } from "@mui/material";
import { useDepartureFlights } from "../../hooks";

type ChipColor = "success" | "info" | "warning" | "primary";

const STATUS: Record<string, { label: string; color: ChipColor }> = {
  LANDED: { label: "Landed", color: "success" },
  ON_TIME: { label: "On Time", color: "info" },
  DELAYED: { label: "Delayed", color: "warning" },
  BOARDING: { label: "Boarding", color: "primary" },
};

function formatTime(utc: string | null | undefined) {
  if (!utc) return "-";
  return new Date(utc).toLocaleTimeString("sv-SE", {
    hour: "2-digit",
    minute: "2-digit",
    timeZone: "Europe/Stockholm",
  });
}

function getStatus(statusText: string) {
  const normalized = statusText.toLowerCase();

  if (normalized.includes("on time")) return STATUS.ON_TIME;
  if (normalized.includes("landed")) return STATUS.LANDED;
  if (normalized.includes("delay")) return STATUS.DELAYED;
  if (normalized.includes("boarding")) return STATUS.BOARDING;

  return STATUS.ON_TIME;
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

  if (error) return <p>Error: {error.message}</p>;
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9 }}>
        Loading departures info...
      </Typography>
    );

  return (
    <Box
      sx={{
        backgroundImage: "url(/images/departures.jpg)",
        backgroundSize: "cover",
        opacity: 0.9,
        width: 250,
        color: "#000000",
        position: "relative",
        p: 2,
        m: 2,
        borderRadius: 2,
        boxShadow: 1,
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
          const statusText =
            departure.locationAndStatus?.flightLegStatusEnglish ?? "Unknown";
          const status = getStatus(statusText);
          return (
            <Paper
              key={departure.flightId}
              sx={{
                p: 1.2,
                borderRadius: 2,
                bgcolor: "rgba(255,255,255,0.9)",
                color: "#111",
              }}
            >
              <Stack direction="row" justifyContent="space-between">
                <Box>
                  <Typography sx={{ fontWeight: 700, fontSize: "0.9rem" }}>
                    {departure.flightId}
                  </Typography>
                  <Typography sx={{ fontSize: "0.75rem", color: "#555" }}>
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

              <Box sx={{ mt: 0.5 }}>
                <Chip
                  size="small"
                  label={statusText}
                  color={status.color}
                  sx={{ fontSize: "0.65rem", height: 20 }}
                />
              </Box>
            </Paper>
          );
        })}
      </Stack>
    </Box>
  );
}
