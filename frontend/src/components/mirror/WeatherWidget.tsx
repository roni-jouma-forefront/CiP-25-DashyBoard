import { Box, Typography } from "@mui/material";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import { useQuery } from "@tanstack/react-query";
import { GetWeather, type MetarData} from '../base/GetWeather'

interface WeatherProps {
  icao: string; 
}

function WeatherWidget( { icao }: WeatherProps) {
  const { data: metarData, isLoading, error} = useQuery<MetarData> ({
    queryKey: ["weather", icao],
    queryFn: () => GetWeather(icao),
    enabled: !!icao,
  })

  console.log("VÄDERDATA SOM HÄMTATS: ", metarData)

  if (error) return <p>Fel: {(error).message}</p>;
  if (isLoading) return <p>Laddar väder...</p>;

function getWeatherIconClass(conditions: string | null) {
  if (!conditions){
    return "wi-na"
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
          
           <TableContainer component={Paper} sx={{ maxWidth: 700 }}>
            <Table>
              <TableBody>
                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold", width: "40%" }}>
                    Airport:
                  </TableCell>
                  <TableCell>{metarData.station?.name ?? "-"}</TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>ICAO:</TableCell>
                  <TableCell>{metarData.icao ?? "-"}</TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Wind (m/s):</TableCell>
                  <TableCell>
                   {metarData.windSpeedMps ?? "-"} m/s
                  </TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Temperature:</TableCell>
                  <TableCell>
                    {metarData.temperature?.celsius ?? "-"}°C /{" "}
                    {metarData.temperature?.fahrenheit ?? "-"}°F
                  </TableCell>
                </TableRow>

                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>Humidity:</TableCell>
                  <TableCell>
                    {metarData.humidity ?? "-"}%
                  </TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Conditions:</TableCell>
                  <TableCell
                    sx={{ display: "flex", alignItems: "center", gap: 1 }}
                  ><i className={ `wi ${getWeatherIconClass(metarData.conditions)}`}
              ></i>
              <i className={"wi-cloudy"}></i>
                    {metarData.conditions ?? "-"}
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer> 
           </>
          )}
        </Box> 
      </Box>
    </>
  );
}

export default WeatherWidget;

