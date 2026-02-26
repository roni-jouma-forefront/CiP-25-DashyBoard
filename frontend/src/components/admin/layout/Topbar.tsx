import { Box, Typography } from "@mui/material";

export const Topbar = () => {
  return (
    <Box sx={{ width: "100%", bgcolor: "#00316E", color: "white", p: 2, boxSizing:"border-box" }}>
      <Typography variant="h5">Logo</Typography>
    </Box>
  );
};
