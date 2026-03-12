import { Box, Typography } from "@mui/material";
import Stack from "@mui/material/Stack";
import { useQuery } from "@tanstack/react-query";
import { GetWeather, type MetarData } from "../base/GetWeather";

interface WeatherProps {
  icao: string;
}

const icaoRowStyling = {
  display: "flex",
  justifyContent: "space-between",
  backgroundColor: "white",
  p: 2,
  borderRadius: 2,
};

function WeatherWidget({ icao }: WeatherProps) {
  const {
    data: metarData,
    isLoading,
    error,
  } = useQuery<MetarData>({
    queryKey: ["weather", icao],
    queryFn: () => GetWeather(icao),
    enabled: !!icao,
  });

  if (error) return <p>Error: {error.message}</p>;
  if (isLoading) return <p>Loading weather info...</p>;

  function getWeatherIconClass(conditions: string | null) {
    if (!conditions) {
      return "wi-na";
    }
    const c = conditions.toLowerCase();
    if (c.includes("sun") || c.includes("clear")) return "wi-day-sunny";
    if (c.includes("rain")) return "wi-rain";
    if (c.includes("snow")) return "wi-snow";
    if (c.includes("cloud")) return "wi-cloudy";
    if (c.includes("storm")) return "wi-thunderstorm";
    return "wi-na";
  }

  return (
    <>
      <Box
        sx={{
          position: "relative",
          p: 2,
          m: 2,
          borderRadius: 2,
          boxShadow: 1,
        }}
      >
        <Box
          sx={{
            position: "absolute",
            inset: 0,
            backgroundImage: "url(/images/weatherimg.jpg)",
            backgroundSize: "cover",
            opacity: 0.9,
            borderRadius: "inherit",
          }}
        />

        <Box sx={{ position: "relative" }}>
          <Typography
            variant="h4"
            sx={{ fontWeight: "bold", marginBottom: "10px" }}
          >
            Weather
          </Typography>

          {!metarData ? (
            <Typography>Loading weather...</Typography>
          ) : (
            <>
              <Box>
                <Stack spacing={1} sx={{ borderRadius: 2 }}>
                  <Typography sx={{ ...icaoRowStyling, gap: 2 }}>
                    <svg width="25" height="25" viewBox="0 0 24 24" fill="none">
                      <path
                        d="M12 22C12 22 20 14.5 20 9C20 5.13401 16.866 2 13 2H11C7.13401 2 4 5.13401 4 9C4 14.5 12 22 12 22Z"
                        stroke="black"
                        strokeWidth="2"
                        strokeLinecap="round"
                        strokeLinejoin="round"
                      />
                      <circle
                        cx="12"
                        cy="9"
                        r="3"
                        stroke="black"
                        strokeWidth="2"
                      />
                    </svg>{" "}
                    {metarData.station?.name ?? "-"}
                  </Typography>
                  <Typography sx={{ ...icaoRowStyling, gap: 2 }}>
                    <strong> Observed: </strong>{" "}
                    {metarData.observed?.slice(11, 16) ?? "-"} UTC
                  </Typography>
                  <Typography sx={icaoRowStyling}>
                    <strong> Icao: </strong> {metarData.icao ?? "-"}
                  </Typography>
                  <Typography sx={icaoRowStyling}>
                    <strong> Wind: </strong> {metarData.windSpeedMps ?? "-"} m/s
                  </Typography>
                  <Typography sx={icaoRowStyling}>
                    <strong> Temperature: </strong>{" "}
                    {metarData.temperature?.celsius ?? "-"}°C /{" "}
                    {metarData.temperature?.fahrenheit ?? "-"}°F
                  </Typography>
                  <Typography sx={icaoRowStyling}>
                    <strong> Humidity: </strong> {metarData.humidity ?? "-"}%
                  </Typography>
                  <Typography sx={icaoRowStyling}>
                    <strong> Conditions: </strong>{" "}
                    <i
                      className={`wi ${getWeatherIconClass(metarData.conditions)}`}
                    ></i>
                    <i className={"wi-cloudy"}></i>
                    {metarData.conditions ?? "-"}
                  </Typography>
                </Stack>
              </Box>
            </>
          )}
        </Box>
      </Box>
    </>
  );
}

export default WeatherWidget;
