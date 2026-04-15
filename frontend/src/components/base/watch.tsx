import { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";
import { widgetTheme } from "../../theme";
import LocationPin from "../mirror/LocationPinSVG";

interface WatchProps {
  timeZone: string;
  location: string;
}

export default function Watch({ timeZone, location }: WatchProps) {
  const [time, setTime] = useState("");

  useEffect(() => {
    const updateTime = () => {
      const now = new Intl.DateTimeFormat("sv-SE", {
        timeZone,
        hour: "2-digit",
        minute: "2-digit",
      }).format(new Date());

      setTime(now);
    };

    updateTime();
    const interval = setInterval(updateTime, 1000);

    return () => clearInterval(interval);
  }, [timeZone]);

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
        width: "12em",
        display: "flex", 
        justifyContent: "center"
      }}
    >
      <Box sx={{ position: "relative" }}>
        <Typography variant="h2" fontFamily="monospace">
          {time}
        </Typography>
        <Box
          sx={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "center",
            m: "5px",
          }}
        >
          <LocationPin />
          <Typography sx={{ variant: "h2" }}>{location}</Typography>
        </Box>
      </Box>
    </Box>
  );
}
