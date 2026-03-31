// import { useState, useEffect } from "react";
import { Box, Typography, Paper, Stack, Chip } from "@mui/material";
import { useArrivalFlights } from "../../hooks";

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

function getStatus(statusText: string | null | undefined) {
  if (!statusText) {
    return STATUS.ON_TIME;
  }

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

  if (error) return <Typography>Error: {error.message}</Typography>;
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9 }}>
        Loading arrivals info...
      </Typography>
    );

  return (
    <Box
      sx={{
        backgroundImage: "url(/images/arrivals.jpg)",
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
            Arrivals
          </Typography>

          <Typography sx={{ fontWeight: 600, fontSize: "0.9rem" }}>
            {today}
          </Typography>
        </Box>
      </Box>
      <Stack spacing={1.2}>
        {arrivals.slice(0, 5).map((arrival) => {
          const statusText =
            arrival.locationAndStatus?.flightLegStatusEnglish ?? "Unknown";
          const status = getStatus(statusText);
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
                    {formatTime(arrival.arrivalTime?.scheduledUtc) ?? "-"}
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "0.7rem",
                      fontWeight: 600,
                      color: "#3b82f6",
                    }}
                  >
                    {arrival.locationAndStatus?.terminal ?? "-"}
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
