import { Box, Typography } from "@mui/material";
import Stack from "@mui/material/Stack";
import { useWeather } from "../../hooks";
import { widgetTheme } from "../../theme";
import LocationPin from "../svg/LocationPinSVG";

interface WeatherProps {
  icao: string;
  pilotVersion: boolean;
}

const icaoRowStyling = {
  display: "flex",
  justifyContent: "space-between",
  p: 2,
  gap: 4,
  borderRadius: 2,
  border: `2px solid ${widgetTheme.palette.primary.light}`,
  fontSize: "0.9rem",
  color: `${widgetTheme.palette.primary.main}`,
  backgroundColor: `${widgetTheme.palette.primary.dark}`,
};

function WeatherWidget({ icao, pilotVersion }: WeatherProps) {
  const { data: metarData, error, isLoading } = useWeather({ icao });

  if (error)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Error: {error.message}
      </Typography>
    );
  if (isLoading)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: `${widgetTheme.palette.primary.main}` }}>
        Loading weather info...
      </Typography>
    );
  function getWeatherIconClass() {
    const weather = metarData?.weather;

    if (!weather) return "wi-day-sunny";
    if (weather.snow) return "wi-snow";
    if (weather.rain) return "wi-rain";
    if (weather.fog) return "wi-fog";
    switch (weather.cloud) {
      case "OVC":
        return "wi-cloudy";
      case "BKN":
        return "wi-cloudy";
      case "SCT":
        return "wi-day-cloudy";
      case "FEW":
        return "wi-day-cloudy";
      default:
        return "wi-day-sunny";
    }
  }

  function getWeatherLabel() {
    const weather = metarData?.weather;

    if (!weather) return "Clear";
    if (weather.snow) return `Snow (${weather.snow})`;
    if (weather.rain) return `Rain (${weather.rain})`;
    if (weather.fog) return `Fog (${weather.fog})`;
    switch (weather.cloud) {
      case "OVC":
        return "Overcast";
      case "BKN":
        return "Broken clouds";
      case "SCT":
        return "Scattered clouds";
      case "FEW":
        return "Few clouds";
      case "CLR":
      case "SKC":
        return "Clear";
      default:
        return "Clear";
    }
  }

  return (
    <>
      {pilotVersion ? (
        <Box sx={{ position: "relative" }}>
          {metarData && (
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
                    Weather
                  </Typography>

                  <Box sx={{ mb: 2, display: "flex", alignItems: "center" }}>
                    <i className={`icons wi ${getWeatherIconClass()}`}></i>
                  </Box>
                </Box>
                <Typography
                  sx={{
                    display: "flex",
                    alignItems: "center",
                  }}
                >
                  <Box sx={{ display: "flex", justifyContent: "start" }}>
                    <LocationPin />
                    <Typography sx={{ ml: "0.2rem", fontSize: "0.9rem" }}>
                      {metarData.station?.name ?? "-"}
                    </Typography>
                  </Box>
                </Typography>
              </Box>
              <Box>
                <Stack spacing={1} sx={{ borderRadius: 2 }}>
                  <Typography sx={{ ...icaoRowStyling, gap: 2 }}>
                    <strong> Observed: </strong>{" "}
                    {/* To display the correct observed time in correct format slice is used*/}
                    {metarData.observed?.slice(11, 16) ?? "-"} UTC
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
                    <strong>Conditions: </strong>
                    {getWeatherLabel()}
                  </Typography>
                </Stack>
              </Box>
            </Box>
          )}
        </Box>
      ) : (
        <Box sx={{ position: "relative" }}>
          {metarData && (
            <>
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
                  width: "12em",
                }}
              >
                <Box sx={{ position: "relative" }}>
                  <Box
                    sx={{
                      display: "flex",
                      flexDirection: "row",
                      justifyContent: "space-around",
                    }}
                  >
                    <i
                      className={`icons wi ${getWeatherIconClass()}`}
                    ></i>
                    <Typography
                      sx={{
                        fontSize: "2.5rem",
                        fontWeight: "600",
                        display: "flex",
                        alignItems: "center",
                      }}
                    >
                      {metarData.temperature?.celsius ?? "-"}°C
                    </Typography>
                  </Box>

                  <Typography
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "space-around",
                    }}
                  >
                    <Box
                      sx={{ display: "flex", justifyContent: "center", mt: 1 }}
                    >
                      <LocationPin />
                      <Typography sx={{ ml: "0.2rem", fontVariant: "h2" }}>
                        {metarData.station?.name ?? "-"}
                      </Typography>
                    </Box>
                  </Typography>
                </Box>
              </Box>
            </>
          )}
        </Box>
      )}
    </>
  );
}

export default WeatherWidget;
