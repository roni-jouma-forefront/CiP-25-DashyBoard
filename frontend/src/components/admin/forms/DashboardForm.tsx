import {
  Box,
  Button,
  MenuItem,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { DatePicker, TimePicker } from "@mui/x-date-pickers";

export const DashboardForm = () => {
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
        Message
      </Typography>
      <Stack spacing={3}>
        <TextField select label="Send to" name="group" required fullWidth>
          <MenuItem value="all">All residents</MenuItem>
          <MenuItem value="checkin">Checking in today</MenuItem>
          <MenuItem value="checkout">Checking out today</MenuItem>
        </TextField>
        <TextField label="Title" name="title" fullWidth required />
        <TextField label="Message" name="message" multiline rows={4} required />
        <Box sx={{ display: "flex", gap: 3 }}>
          <DatePicker label="Post date" sx={{ felx: 1 }} />
          <TimePicker label="Post time" sx={{ felx: 1 }} />
        </Box>
        <Box sx={{ display: "flex", gap: 3 }}>
          <DatePicker label="Delete date" />
          <TimePicker label="Delete time" />
        </Box>
        <Button variant="contained">Post</Button>
      </Stack>
    </Box>
  );
};
