import { Stack, Typography, Box } from "@mui/material";
import { useFlightInfo } from "../../hooks";
import { widgetTheme } from "../../theme";

interface FlightProps {
  airport: string;
  flight: string;
}

const flightRowStyling = {
  display: "flex",
  justifyContent: "space-between",
  backgroundColor: `${widgetTheme.palette.primary.dark}`,
  p: 2,
  borderRadius: 2,
  fontSize: "0.9rem",
  border: `2px solid ${widgetTheme.palette.primary.light}`,
};

function FlightInfoWidget({ airport, flight }: FlightProps) {
  const {
    data: flightData,
    error,
    isLoading,
  } = useFlightInfo({ airport, flight });

  if (error)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Error: {error.message}
      </Typography>
    );
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Loading flight info...
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
      {flightData && (
        <>
          <Box>
            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
              }}
            >
              <Typography sx={{ fontSize: "1.4rem", fontWeight: 700, mb: 2 }}>
                Flight <span>{flightData.flightId ?? "-"}</span>
              </Typography>
            </Box>
            <Stack spacing={1} sx={{ borderRadius: 2 }}>
              <Typography
                sx={{
                  ...flightRowStyling,
                  display: "flex",
                  flexDirection: "column",
                }}
              >
                {" "}
                <strong>
                  <span>{flightData.departureAirportSwedish ?? "-"}</span>
                  <span> → </span>
                  <span>{flightData.arrivalAirportSwedish ?? "-"}</span>
                </strong>
              </Typography>

              <Typography sx={flightRowStyling}>
                <strong>Gate:</strong>
                <span>{flightData.locationAndStatus?.gate ?? "-"}</span>
              </Typography>

              <Typography sx={flightRowStyling}>
                <strong>Terminal:</strong>
                <span>{flightData.locationAndStatus?.terminal ?? "-"}</span>
              </Typography>

              <Typography sx={{ ...flightRowStyling, gap: 3 }}>
                <strong>Departure (UTC):</strong>
                <span>
                  {flightData.departureTime?.scheduledUtc?.slice(11, 16) ?? "-"}
                </span>
              </Typography>

              <Typography sx={{ ...flightRowStyling, gap: 3 }}>
                <strong>Flight Status:</strong>
                <span>
                  {flightData.locationAndStatus?.flightLegStatusEnglish ?? "-"}
                </span>
              </Typography>
            </Stack>
          </Box>
        </>
      )}
    </Box>
  );
}

export default FlightInfoWidget;
