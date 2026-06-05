import { Box, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { GetHotel } from "../../../services/api/GetHotel";
import { theme } from "../../../theme";

export const Topbar = () => {
  const hotelId = import.meta.env.VITE_HOTEL_ID;
  const [hotelName, setHotelName] = useState<string>("");
  const [time, setTime] = useState("");
  const timeZone = "Europe/Stockholm";

  useEffect(() => {
    if (!hotelId) return;
    GetHotel(hotelId)
      .then((hotel) => setHotelName(hotel.name))
      .catch(() => setHotelName("Hotel"));
  }, [hotelId]);

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
        width: "100%",
        background: "linear-gradient(90deg, #0B1220 0%, #172036 60%, #1B3F8B 100%)",
        color: theme.palette.topbar.text,
        px: 3,
        py: 1.5,
        boxSizing: "border-box",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        borderBottom: "1px solid rgba(255,255,255,0.08)",
        boxShadow: "0 4px 16px rgba(0,0,0,0.3)",
      }}
    >
      <Typography variant="h5" sx={{ color: "#F1F5F9", fontWeight: 600 }}>
        {hotelName}
      </Typography>
      <Box sx={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
        <Box component="img" src="/logo.jpg" alt="DashyBoard" sx={{ height: 45 }} />
        <Typography variant="h6">{time}</Typography>
      </Box>
    </Box>
  );
};
