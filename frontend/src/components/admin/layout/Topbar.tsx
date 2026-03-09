import { Box, Typography } from "@mui/material";
import { theme } from "../../../theme";

export const Topbar = () => {
  return (
    <Box
      sx={{
        width: "100%",
        bgcolor: theme.palette.topbar.background,
        color: theme.palette.topbar.text,
        p: 2,
        boxSizing: "border-box",
        boxShadow: `
      0 1px 0 rgba(255,255,255,0.05),
      0 6px 12px rgba(0,0,0,0.35)
    `,
      }}
    >
      <Typography variant="h5">Hotel Name</Typography>
    </Box>
  );
};
