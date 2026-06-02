import { Box, Button, TextField, Typography } from "@mui/material";

export const AdditionalGuestFields = () => {
  return (
    <Box
      sx={{
        flex: 1,
        p: 3,
        borderRadius: 3,
        boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
        border: "1px solid rgba(0,0,0,0.06)",
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
