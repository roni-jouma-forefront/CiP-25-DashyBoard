import {
  Box,
  Button,
  MenuItem,
  Stack,
  TextField,
  Typography,
} from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";
import type { Staff } from "../../../types/types";

const mockStaff: Staff[] = [
  { name: "Emmi Quirin" },
  { name: "Anna C Hallberg" },
  { name: "Nikita Sjölander" },
];

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
        Post Message
      </Typography>
      <Stack spacing={3}>
        <TextField select label="Send to" name="group" required fullWidth>
          <MenuItem value="all">All residents</MenuItem>
          <MenuItem value="checkin">Checking in today</MenuItem>
          <MenuItem value="checkout">Checking out today</MenuItem>
        </TextField>
        <MessageBaseForm />
        <TextField select label="Author" name="author" required fullWidth>
          {mockStaff.map((staff, index) => (
            <MenuItem key={index} value={staff.name}>
              {staff.name}
            </MenuItem>
          ))}
        </TextField>
        <Button variant="contained">Post</Button>
      </Stack>
    </Box>
  );
};
