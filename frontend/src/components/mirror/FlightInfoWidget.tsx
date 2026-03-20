import { Stack, Typography, Box } from "@mui/material";
import { useEffect, useState } from "react";
import { useFlightInfo } from "../../hooks";
import { GetAirportNameByIcao } from "../../services/GetAirportNameByIcao";

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
};

function FlightInfoWidget({ airport, flight }: FlightProps) {
  const [arrivalAirportName, setArrivalAirportName] = useState<string | null>(
    null,
  );

  const {
    data: flightData,
    error,
    isLoading,
  } = useFlightInfo({ airport, flight });

  console.log("Hämtad flygdata, ", flightData);

  useEffect(() => {
    const icao = flightData?.arrivalAirportIcao;
    if (!icao) return;

    GetAirportNameByIcao(icao).then((name) => {
      setArrivalAirportName(name);
    });
  }, [flightData]);

  if (error) return <p>Error: {error.message}</p>;
  if (isLoading) return <p>Loading flight info...</p>;

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
      <Typography
        variant="h4"
        sx={{ fontWeight: "bold", marginBottom: "10px", color: "#001e41" }}
      >
        Flight Info
      </Typography>

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
                <span>Arlanda</span>
                <span> → </span>
                <span>{arrivalAirportName}</span>
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
