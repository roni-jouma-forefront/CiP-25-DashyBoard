import { Box, TextField } from "@mui/material";
import { DatePicker, TimePicker } from "@mui/x-date-pickers";
import type { Dayjs } from "dayjs";
import type React from "react";

interface MessageBaseFormProps {
  title: string;
  content: string;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onPostDateChange: (value: Dayjs | null) => void;
  onPostTimeChange: (value: Dayjs | null) => void;
  onExpiresDateChange: (value: Dayjs | null) => void;
  onExpiresTimeChange: (value: Dayjs | null) => void;
}

export const MessageBaseForm = ({
  handleChange,
  title,
  content,
  onPostDateChange,
  onPostTimeChange,
  onExpiresDateChange,
  onExpiresTimeChange,
}: MessageBaseFormProps) => {
  return (
    <>
      <TextField
        label="Title"
        name="title"
        value={title}
        onChange={handleChange}
        fullWidth
        required
      />
      <TextField
        label="Message"
        name="content"
        value={content}
        onChange={handleChange}
        multiline
        rows={4}
        required
      />
      <Box sx={{ display: "flex", gap: 3 }}>
        <DatePicker
          label="Post date"
          onChange={onPostDateChange}
          sx={{ flex: 1 }}
        />
        <TimePicker
          label="Post time"
          onChange={onPostTimeChange}
          sx={{ flex: 1 }}
        />
      </Box>
      <Box sx={{ display: "flex", gap: 3 }}>
        <DatePicker
          label="Delete date"
          onChange={onExpiresDateChange}
          sx={{ flex: 1 }}
        />
        <TimePicker
          label="Delete time"
          onChange={onExpiresTimeChange}
          sx={{ flex: 1 }}
        />
      </Box>
    </>
  );
};
