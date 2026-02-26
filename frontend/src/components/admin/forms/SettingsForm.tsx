import { Box, Button, Stack, TextField, Typography } from "@mui/material";

export const SettingsForm = () => {
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
        Hotel Details
      </Typography>
      <Stack spacing={3}>
        <TextField label="Hotel Name" name="hotel-name" fullWidth required />
        <TextField
          label="Hotel Location"
          name="hotel-location"
          fullWidth
          required
        />
        <TextField type="number" label="Number of Rooms" />
        <Button variant="contained">Save</Button>
      </Stack>
    </Box>
  );
};
