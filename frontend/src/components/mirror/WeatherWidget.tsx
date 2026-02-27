import { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";

interface WeatherProps {
  icao: string; 
}

type MetarData = {
  icao?: string;
  observed?: string;
  station?: {
    name?: string;
    location?: string;
  };
  temperature?: {
    celsius?: number;
    fahrenheit?: number;
  };
  humidity?: number;
  windSpeedMps?: number;
  conditions?: string | null;
};

function WeatherWidget({ icao }: WeatherProps) {
  const [metarData, setMetarData] = useState<MetarData | null>(null);


  useEffect(() => {
    const load = async () => {
      try {
        const response = await fetch(`http://localhost:5000/api/CheckWx/${icao}`);
        const json = await response.json();
        const item: MetarData | null = Array.isArray(json) ? (json[0] ?? null) : (json ?? null);

        console.log("raw json:", json);
        console.log("selected item:", item);

        setMetarData(item);
      } catch (err) {
        console.error(err);
        setMetarData(null);
      }
    };

    load();
  }, [icao]);

console.log("this is", metarData);

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
              {/* <Typography>ICAO: {metarData.icao ?? "-"}</Typography>
              <Typography>Station: {metarData.station?.name ?? "-"}</Typography>
              <Typography>Temp: {metarData.temperature?.celsius ?? "-"}°C</Typography>
              <Typography>Fukt: {metarData.humidity ?? "-"}%</Typography>
              <Typography>Vind: {metarData.windSpeedMps ?? "-"} m/s</Typography>
              <Typography>Villkor: {metarData.conditions ?? "-"}</Typography> */}
           
          
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

                {/* <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>
                    Observationstid:
                  </TableCell>
                  <TableCell>
                    {metarData.observationTime.toLocaleString("sv-SE")}
                  </TableCell>
                </TableRow> */}

                <TableRow>
                  <TableCell sx={{ fontWeight: "bold" }}>Wind (m/s):</TableCell>
                  <TableCell>
                   {metarData.windSpeedMps ?? "-"} m/s
                  </TableCell>
                </TableRow>

                {/* <TableRow sx={{ backgroundColor: "#f5f7fa" }}>
                  <TableCell sx={{ fontWeight: "bold" }}>Sikt:</TableCell>
                  <TableCell>
                    {metarData.visibility.value} {metarData.visibility.unit}
                  </TableCell>
                </TableRow> */}

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
                  >
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

