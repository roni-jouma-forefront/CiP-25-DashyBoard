import { Stack, Typography, Box } from "@mui/material";
import { useFlightInfo } from "../../hooks";

interface FlightProps {
  airport: string;
  flight: string;
}

const flightRowStyling = {
  display: "flex",
  justifyContent: "space-between",
  backgroundColor: "white",
  p: 2,
  borderRadius: 2,
  opacity: 0.9,
  fontSize: "0.9rem",
};

function FlightInfoWidget({ airport, flight }: FlightProps) {
  const {
    data: flightData,
    error,
    isLoading,
  } = useFlightInfo({ airport, flight });

  // if (error) return <p>Error: {error.message}</p>;
  // if (isLoading)
  //   return (
  //     <Typography sx={{ m: 3, opacity: 0.9 }}>
  //       Loading flight info...
  //     </Typography>
  //   );

  return (
    <Box
      sx={{
        position: "relative",
        p: 2,
        m: 2,
        borderRadius: 2,
        boxShadow: 1,
        backgroundImage: "url(/images/airportimg.jpg)",
        backgroundSize: "cover",
        opacity: 0.9,
      }}
    >
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Typography sx={{ fontSize: "1.4rem", fontWeight: 700, mb: 2 }}>
          Flight info
        </Typography>
      </Box>
      {!flightData ? (
        <Typography>Loading flight info...</Typography>
      ) : (
        <Box>
          <Stack spacing={1} sx={{ borderRadius: 2 }}>
            <Typography sx={flightRowStyling}>
              <strong>Flight ID:</strong>
              <span>{flightData.flightId ?? "-"}</span>
            </Typography>
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
      )}
    </Box>
  );
}

export default FlightInfoWidget;
