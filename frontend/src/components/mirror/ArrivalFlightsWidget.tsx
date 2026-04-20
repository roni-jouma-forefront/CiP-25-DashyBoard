import { Box, Typography, Paper, Stack } from "@mui/material";
import { useArrivalFlights } from "../../hooks";
import { widgetTheme } from "../../theme/index.ts";
import formatTime from "./FormatTime.tsx";

interface ArrivalProps {
  airport: string, 
  timezone: string
}

export default function ArrivalsWidget( { airport, timezone }: ArrivalProps) {
  const today = new Date().toLocaleDateString("en-GB", {
    month: "long",
    day: "numeric",
  });

  /////////////////////////////////////////
  const test = timezone; 
  console.log(test)

  const {
    data: arrivals = [],
    error,
    isLoading,
  } = useArrivalFlights({
    airport,
  });

  if (error)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Error: {error.message}
      </Typography>
    );
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Loading arrivals info...
      </Typography>
    );

  return (
    <Box
      sx={{
        position: "relative",
        p: 2,
        m: 2,
        borderRadius: 2,
        border: `5px solid ${widgetTheme.palette.primary.main}`,
        boxShadow: 1,
        color: `${widgetTheme.palette.primary.main}`,
        backgroundColor: `${widgetTheme.palette.primary.dark}`,
        width: "15em",
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
          return (
            <Paper
              key={arrival.flightId}
              sx={{
                p: 1.2,
                borderRadius: 2,
                bgcolor: `${widgetTheme.palette.primary.dark}`,
                color: `${widgetTheme.palette.primary.main}`,
                border: `2px solid ${widgetTheme.palette.primary.light}`,
              }}
            >
              <Stack direction="row" justifyContent="space-between">
                <Box>
                  <Typography sx={{ fontWeight: 700, fontSize: "0.9rem" }}>
                    {arrival.flightId}
                  </Typography>
                  <Typography
                    sx={{
                      fontSize: "0.75rem",
                      color: `${widgetTheme.palette.primary.main}`,
                    }}
                  >
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
