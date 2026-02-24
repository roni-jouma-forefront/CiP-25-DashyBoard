import { useEffect, useState } from "react";
import { Box, Typography } from "@mui/material";

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
    <Box sx={{ p: 2, borderRadius: 2, boxShadow: 1, background: "white" }}>
      <Typography variant="h4" fontFamily="monospace">
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
        <svg width="25" height="25" viewBox="0 0 24 24" fill="none">
          <path
            d="M12 22C12 22 20 14.5 20 9C20 5.13401 16.866 2 13 2H11C7.13401 2 4 5.13401 4 9C4 14.5 12 22 12 22Z"
            stroke="black"
            strokeWidth="2"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
          <circle cx="12" cy="9" r="3" stroke="black" strokeWidth="2" />
        </svg>
        <Typography sx={{ fontWeight: "700" }}>{location}</Typography>
      </Box>
    </Box>
  );
}
