import { Box, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { GetHotel } from "../../../services/api/GetHotel";
import { theme } from "../../../theme";

export const Topbar = () => {
  const hotelId = import.meta.env.VITE_HOTEL_ID;
  const [hotelName, setHotelName] = useState<string>("");

  useEffect(() => {
    if (!hotelId) return;
    GetHotel(hotelId)
      .then((hotel) => setHotelName(hotel.name))
      .catch(() => setHotelName("Hotel"));
  }, [hotelId]);

  return (
    <Box
      sx={{
        width: "100%",
        bgcolor: theme.palette.topbar.background,
        color: theme.palette.topbar.text,
        p: 2,
        boxSizing: "border-box",
        display: "flex",
        alignItems: "center",
        justifyContent: "space-between",
        boxShadow: `
      0 1px 0 rgba(255,255,255,0.05),
      0 6px 12px rgba(0,0,0,0.35)
    `,
      }}
    >
      <Typography variant="h5">{hotelName}</Typography>
      <Box
        component="img"
        src="/logo.jpg"
        alt="DashyBoard"
        sx={{ height: 40 }}
      />
    </Box>
  );
};
