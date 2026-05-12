import { Box, TextField } from "@mui/material";
import { DateTimePicker } from "@mui/x-date-pickers";
import type { Dayjs } from "dayjs";
import type React from "react";

interface MessageBaseFormProps {
  title: string;
  content: string;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onPostDateTimeChange: (value: Dayjs | null) => void;
  onExpiresDateTimeChange: (value: Dayjs | null) => void;
}

export const MessageBaseForm = ({
  handleChange,
  title,
  content,
  onPostDateTimeChange,
  onExpiresDateTimeChange,
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
        <DateTimePicker
          label="Post at"
          sx={{ flex: 1 }}
          onChange={onPostDateTimeChange}
        />
      </Box>
      <Box sx={{ display: "flex", gap: 3 }}>
        <DateTimePicker
          label="Expires at"
          sx={{ flex: 1 }}
          onChange={onExpiresDateTimeChange}
        />
      </Box>
    </>
  );
};
