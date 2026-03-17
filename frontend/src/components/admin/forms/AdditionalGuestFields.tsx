import { Box, Button, TextField, Typography } from "@mui/material";
import { theme } from "../../../theme";

export const AdditionalGuestFields = () => {
  return (
    <Box
      sx={{
        flex: 1,
        p: 2,
        borderRadius: 2,
        border: `1px solid ${theme.palette.divider}`,
        background: "white",
        mt: 3,
      }}
    >
      <Typography>Add Additional Guest</Typography>
      <TextField
        label="First Name"
        name="first-name"
        fullWidth
        required
        sx={{ mt: 2 }}
      />
      <TextField
        label="Last Name"
        name="last-name"
        fullWidth
        required
        sx={{ mt: 2 }}
      />
      <Button sx={{ mt: 2 }}>Add Guest</Button>
    </Box>
  );
};
