import { Box, Button, Stack, TextField, Typography } from "@mui/material";

export const RoomDetailsForm = () => {
  return (
    <Box
      component="form"
      sx={{
        width: "500px",
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
    >
      <Typography variant="h5" mb={3}>
        Room Details
      </Typography>
      <Stack spacing={3}>
        <TextField
          label="Guest First Name"
          name="guest-first-name"
          fullWidth
          required
        />
        <TextField
          label="Guest Last Name"
          name="guest-last-name"
          fullWidth
          required
        />
        <TextField
          label="Departing flight"
          name="departing-flight"
          fullWidth
          required
        />
        <Button variant="contained">Save</Button>
      </Stack>
    </Box>
  );
};
