import { Box, Typography } from "@mui/material";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";

const metarData = {
  rawMetar: "ESSA 251220Z 18005KT 9999 FEW030 07/03 Q1021 NOSIG",
  icaoCode: "ESSA",
  airport: "Arlanda Airport",
  observationTime: new Date(),
  wind: { direction: 180, speed: 5, unit: "KT" },
  visibility: { value: 9999, unit: "m" },
  temperature: { celsius: 7, dewpoint: 3 },
  pressure: { value: 1021, unit: "hPa" },
  clouds: [{ coverage: "FEW", altitude: 3000 }],
  weather: "Sunny",
};

function WeatherWidget() {
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

          <TableContainer component={Paper} sx={{ maxWidth: 700 }}>
            <Table>
              <TableBody>
                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold", width: "40%" }}>
                    Flygplats:
                  </TableCell>
                  <TableCell>{metarData.airport}</TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>ICAO:</TableCell>
                  <TableCell>{metarData.icaoCode}</TableCell>
                </TableRow>

                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>
                    Observationstid:
                  </TableCell>
                  <TableCell>
                    {metarData.observationTime.toLocaleString("sv-SE")}
                  </TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Vind:</TableCell>
                  <TableCell>
                    {metarData.wind.direction}° {metarData.wind.speed}{" "}
                    {metarData.wind.unit}
                  </TableCell>
                </TableRow>

                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>Sikt:</TableCell>
                  <TableCell>
                    {metarData.visibility.value} {metarData.visibility.unit}
                  </TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Temperatur:</TableCell>
                  <TableCell>
                    {metarData.temperature.celsius}°C /{" "}
                    {metarData.temperature.dewpoint}°C
                  </TableCell>
                </TableRow>

                <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>Tryck:</TableCell>
                  <TableCell>
                    {metarData.pressure.value} {metarData.pressure.unit}
                  </TableCell>
                </TableRow>

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Väder:</TableCell>
                  <TableCell
                    sx={{ display: "flex", alignItems: "center", gap: 1 }}
                  >
                    ☀️ ( {metarData.weather} )
                  </TableCell>
                </TableRow>
              </TableBody>
            </Table>
          </TableContainer>
        </Box>
      </Box>
    </>
  );
}

export default WeatherWidget;
