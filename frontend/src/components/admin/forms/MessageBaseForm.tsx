import { Box, TextField } from "@mui/material";
import { DatePicker, TimePicker } from "@mui/x-date-pickers";

export const MessageBaseForm = () => {
  return (
    <>
      <TextField label="Title" name="title" fullWidth required />
      <TextField label="Message" name="message" multiline rows={4} required />
      <Box sx={{ display: "flex", gap: 3 }}>
        <DatePicker label="Post date" sx={{ flex: 1 }} />
        <TimePicker label="Post time" sx={{ flex: 1 }} />
      </Box>
      <Box sx={{ display: "flex", gap: 3 }}>
        <DatePicker label="Delete date" sx={{ flex: 1 }} />
        <TimePicker label="Delete time" sx={{ flex: 1 }} />
      </Box>
    </>
  );
};
