import {
  Box,
  Button,
  FormControlLabel,
  MenuItem,
  Stack,
  Switch,
  TextField,
  Typography,
} from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";
import type { Staff } from "../../../types/types";
import React, { useState } from "react";
import { Dayjs } from "dayjs";
import type { MessageBackend } from "../../../types/message.types";

const mockStaff: Staff[] = [
  { name: "Emmi Quirin" },
  { name: "Anna C Hallberg" },
  { name: "Nikita Sjölander" },
];

interface DashboardFormData extends Omit<
  MessageBackend,
  "postAt" | "expiresAt" | "id" | "isActive"
> {
  postDate: Dayjs | null;
  postTime: Dayjs | null;
  expiresTime: Dayjs | null;
  expiresDate: Dayjs | null;
}

interface DashboardFormProps {
  onSubmit: (formData: MessageBackend) => void;
}

export const DashboardForm = ({ onSubmit }: DashboardFormProps) => {
  const [formData, setFormData] = useState<DashboardFormData>({
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: null,
    title: "",
    content: "",
    recurring: false,
    postDate: null,
    postTime: null,
    expiresTime: null,
    expiresDate: null,
    author: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const onPostDateChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, postDate: value }));
  };
  const onPostTimeChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, postTime: value }));
  };
  const onExpiresDateChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, expiresDate: value }));
  };
  const onExpiresTimeChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, expiresTime: value }));
  };

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    onSubmit({
      id: "",
      bookingId: null,
      hotelId: import.meta.env.VITE_HOTEL_ID,
      title: formData.title,
      content: formData.content,
      isActive: true,
      recurring: formData.recurring,
      postAt: formData.postDate
        ? formData.postDate
            .set("hour", formData.postTime?.hour() ?? 0)
            .set("minute", formData.postTime?.minute() ?? 0)
            .toISOString()
        : null,
      expiresAt: formData.expiresDate
        ? formData.expiresDate
            .set("hour", formData.expiresTime?.hour() ?? 0)
            .set("minute", formData.expiresTime?.minute() ?? 0)
            .toISOString()
        : null,
      author: formData.author,
    });
  };

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
      onSubmit={handleSubmit}
    >
      <Typography variant="h5" mb={3}>
        Post Message
      </Typography>
      <Stack spacing={3}>
        <FormControlLabel
          control={
            <Switch
              checked={formData.recurring}
              onChange={handleChange}
              name="Recurring"
            />
          }
          label="Recurring"
        />
        <MessageBaseForm
          handleChange={handleChange}
          onPostTimeChange={onPostTimeChange}
          onPostDateChange={onPostDateChange}
          onExpiresTimeChange={onExpiresTimeChange}
          onExpiresDateChange={onExpiresDateChange}
          title={formData.title}
          content={formData.content}
        />
        <TextField
          select
          label="Author"
          name="author"
          value={formData.author}
          onChange={handleChange}
          required
          fullWidth
        >
          {mockStaff.map((staff, index) => (
            <MenuItem key={index} value={staff.name}>
              {staff.name}
            </MenuItem>
          ))}
        </TextField>
        <Button variant="contained" type="submit">
          Post
        </Button>
      </Stack>
    </Box>
  );
};
