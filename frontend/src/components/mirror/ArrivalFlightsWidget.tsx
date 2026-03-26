// import { useState, useEffect } from "react";
import { Box, Typography, Paper, Stack, Chip } from "@mui/material";
import { useArrivalFlights } from "../../hooks";

const STATUS = {
  LANDED: { label: "Landed", color: "success" },
  ON_TIME: { label: "On Time", color: "info" },
  DELAYED: { label: "Delayed", color: "warning" },
  BOARDING: { label: "Boarding", color: "primary" },
};

// function Clock() {
//   const [time, setTime] = useState(new Date());
//   useEffect(() => {
//     const t = setInterval(() => setTime(new Date()), 1000);
//     return () => clearInterval(t);
//   }, []);

//   return (
//     <Typography sx={{ fontSize: "0.75rem", opacity: 0.8 }}>
//       {time.toLocaleTimeString("sv-SE", {
//         hour: "2-digit",
//         minute: "2-digit",
//       })}
//     </Typography>
//   );
// }

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

export default function ArrivalsWidget() {
  const today = new Date().toLocaleDateString("en-GB", {
    month: "long",
    day: "numeric",
  });

  const {
    data: arrivals = [],
    error,
    isLoading,
  } = useArrivalFlights({
    airport: "ARN",
  });

  if (error) return <p>Error: {error.message}</p>;
  if (isLoading) return <p>Loading arrivals info...</p>;

  return (
    <Box
      sx={{
        backgroundImage: "url(/images/arrivals.jpg)",
        backgroundSize: "cover",
        opacity: 0.9,
        width: 340,
        borderRadius: 3,
        p: 2,
        color: "#000000",
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
            Arrivals
          </Typography>
          <Typography sx={{ fontWeight: 700 }}>{today}</Typography>
        </Box>

        <Box sx={{ display: "flex", alignItems: "center", mt: 0.5 }}>
          <Typography sx={{ fontSize: "0.8rem" }}>Stockholm Arlanda</Typography>
        </Box>
      </Box>
      <Stack spacing={1.2}>
        {arrivals.slice(0, 5).map((arrival) => {
          const status = getStatus(
            arrival.locationAndStatus.flightLegStatusEnglish,
          );
          return (
            <Paper
              key={arrival.flightId}
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
                    {arrival.flightId}
                  </Typography>
                  <Typography sx={{ fontSize: "0.75rem", color: "#555" }}>
                    {arrival.departureAirportSwedish} to{" "}
                    {arrival.arrivalAirportSwedish}
                  </Typography>
                </Box>

                <Box sx={{ textAlign: "right" }}>
                  <Typography sx={{ fontSize: "0.8rem" }}>
                    {arrival.arrivalTime?.estimatedUtc ?? "-"}
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "0.7rem",
                      fontWeight: 600,
                      color: "#3b82f6",
                    }}
                  >
                    {arrival.locationAndStatus.terminal ?? "-"}
                  </Typography>
                </Box>
              </Stack>

              <Box sx={{ mt: 0.5 }}>
                <Chip
                  size="small"
                  label={arrival.locationAndStatus.flightLegStatusEnglish}
                  color={status.color as any}
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
