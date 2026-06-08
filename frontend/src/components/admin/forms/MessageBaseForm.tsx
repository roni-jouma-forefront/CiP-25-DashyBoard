import { Stack, TextField, Typography } from "@mui/material";
import { DateTimePicker } from "@mui/x-date-pickers";
import type { Dayjs } from "dayjs";
import type React from "react";

interface MessageBaseFormProps {
  title: string;
  content: string;
  postAtError: string | null;
  expiresAtError: string | null;
  author: string;
  postAt?: Dayjs | null;
  expiresAt?: Dayjs | null;
  handleChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onPostDateTimeChange: (value: Dayjs | null) => void;
  onExpiresDateTimeChange: (value: Dayjs | null) => void;
}

export const MessageBaseForm = ({
  handleChange,
  title,
  content,
  author,
  postAtError,
  expiresAtError,
  postAt,
  expiresAt,
  onPostDateTimeChange,
  onExpiresDateTimeChange,
}: MessageBaseFormProps) => {
  return (
    <Stack direction={{ xs: "column", md: "row" }} spacing={2}>
      <Stack spacing={2} flex={1}>
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
      </Stack>
      <Stack flex={1}>
        <Stack spacing={2}>
          <DateTimePicker
            label="Post at"
            value={postAt ?? null}
            sx={{ flex: 1 }}
            onChange={onPostDateTimeChange}
          />
          <Typography color="error" variant="caption">
            {postAtError}
          </Typography>
        </Stack>
        <Stack spacing={2}>
          <DateTimePicker
            label="Expires at"
            value={expiresAt ?? null}
            sx={{ flex: 1 }}
            onChange={onExpiresDateTimeChange}
          />
          <Typography color="error" variant="caption">
            {expiresAtError}
          </Typography>
        </Stack>
        <TextField
          label="Author"
          name="author"
          value={author}
          onChange={handleChange}
          required
          fullWidth
        ></TextField>
      </Stack>
    </Stack>
  );
};
