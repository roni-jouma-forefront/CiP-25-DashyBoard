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
import { DayPicker } from "../DayPicker";
import type { Day, Staff } from "../../../types/types";
import React, { useState } from "react";
import { Dayjs } from "dayjs";
import type { MessageBackend } from "../../../types/message.types";
import { TimePicker } from "@mui/x-date-pickers";

const mockStaff: Staff[] = [
  { name: "Emmi Quirin" },
  { name: "Anna C Hallberg" },
  { name: "Nikita Sjölander" },
];

interface DashboardFormProps {
  onSubmit: (formData: MessageBackend) => void;
}
interface FormErrors {
  postAt: string | null;
  expiresAt: string | null;
  recurrenceTimeStart: boolean;
  recurrenceTimeEnd: boolean;
  recurrenceDays: boolean;
}

export const DashboardForm = ({ onSubmit }: DashboardFormProps) => {
  const [formData, setFormData] = useState<MessageBackend>({
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: null,
    id: "",
    isActive: false,
    title: "",
    content: "",
    recurring: false,
    recurrenceType: null,
    postAt: null,
    expiresAt: null,
    author: "",
    recurrenceDays: null,
    recurrenceTimeStart: null,
    recurrenceTimeEnd: null,
  });

  const [error, setError] = useState<FormErrors>({
    postAt: null,
    expiresAt: null,
    recurrenceTimeStart: false,
    recurrenceTimeEnd: false,
    recurrenceDays: false,
  });

  const [selectedDays, setSelectedDays] = useState<Day[]>([]);
  const [startTime, setStartTime] = useState<Dayjs | null>(null);
  const [endTime, setEndTime] = useState<Dayjs | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleToggle = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = e.target;

    setFormData((prev) => ({
      ...prev,
      [name]: checked,
    }));
  };

  const onPostDateTimeChange = (value: Dayjs | null) => {
    if (formData.expiresAt && value && !value.isBefore(formData.expiresAt)) {
      setError((prev) => ({
        ...prev,
        postAt: "Date and time must be before expires at",
      }));
      return;
    } else {
      setError((prev) => ({
        ...prev,
        postAt: null,
        expiresAt: null,
      }));
      setFormData((prev) => ({
        ...prev,
        postAt: value ? value.toISOString() : null,
      }));
    }
  };

  const onExpiresDateTimeChange = (value: Dayjs | null) => {
    if (formData.postAt && value && !value.isAfter(formData.postAt)) {
      setError((prev) => ({
        ...prev,
        expiresAt: "Exeration date and time must be after post date",
      }));
      return;
    } else {
      setError((prev) => ({
        ...prev,
        postAt: null,
        expiresAt: null,
      }));
      setFormData((prev) => ({
        ...prev,
        expiresAt: value ? value.toISOString() : null,
      }));
    }
  };

  const onStartTimeChange = (value: Dayjs | null) => {
    if (endTime && value && !value.isBefore(endTime)) {
      setError((prev) => ({ ...prev, recurrenceTimeStart: true }));
      return;
    } else {
      setError((prev) => ({
        ...prev,
        recurrenceTimeStart: false,
        recurrenceTimeEnd: false,
      }));
      setStartTime(value);
      setFormData((prev) => ({
        ...prev,
        recurrenceTimeStart: value ? value.format("HH:mm:ss") : null,
      }));
    }
  };

  const onEndTimeChange = (value: Dayjs | null) => {
    if (startTime && value && !value.isAfter(startTime)) {
      setError((prev) => ({ ...prev, recurrenceTimeEnd: true }));
      return;
    } else {
      setError((prev) => ({
        ...prev,
        recurrenceTimeStart: false,
        recurrenceTimeEnd: false,
      }));
      setEndTime(value);
      setFormData((prev) => ({
        ...prev,
        recurrenceTimeEnd: value ? value.format("HH:mm:ss") : null,
      }));
    }
  };

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (error.postAt || error.expiresAt) {
      return;
    }

    if (formData.recurring) {
      const hasErrors =
        !formData.recurrenceTimeStart ||
        !formData.recurrenceTimeEnd ||
        selectedDays.length === 0;
      if (hasErrors) {
        setError({
          postAt: formData.postAt,
          expiresAt: formData.expiresAt,
          recurrenceTimeStart: !formData.recurrenceTimeStart,
          recurrenceTimeEnd: !formData.recurrenceTimeEnd,
          recurrenceDays: selectedDays.length === 0,
        });
        return;
      }
    }

    onSubmit({
      id: "",
      bookingId: null,
      hotelId: import.meta.env.VITE_HOTEL_ID,
      title: formData.title,
      content: formData.content,
      isActive: true,
      recurring: formData.recurring,
      recurrenceType: formData.recurring ? "Weekly" : "None",
      postAt: formData.postAt ? formData.postAt : null,
      expiresAt: formData.expiresAt ? formData.expiresAt : null,
      author: formData.author,
      recurrenceDays: selectedDays.length > 0 ? selectedDays.join(",") : null,
      recurrenceTimeStart: formData.recurrenceTimeStart
        ? formData.recurrenceTimeStart
        : null,
      recurrenceTimeEnd: formData.recurrenceTimeEnd
        ? formData.recurrenceTimeEnd
        : null,
    });

    setError({
      postAt: null,
      expiresAt: null,
      recurrenceTimeStart: false,
      recurrenceTimeEnd: false,
      recurrenceDays: false,
    });
    setSelectedDays([]);
    setStartTime(null);
    setEndTime(null);
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
          label="Recurring"
          control={
            <Switch
              checked={formData.recurring}
              onChange={handleToggle}
              name="recurring"
            />
          }
        />
        <MessageBaseForm
          handleChange={handleChange}
          onPostDateTimeChange={onPostDateTimeChange}
          onExpiresDateTimeChange={onExpiresDateTimeChange}
          title={formData.title}
          content={formData.content}
          postAtError={error.postAt}
          expiresAtError={error.expiresAt}
        />
        {formData.recurring && (
          <>
            <Stack spacing={1}>
              <TimePicker
                label="Start Time"
                sx={{ flex: 1 }}
                value={startTime}
                onChange={onStartTimeChange}
              />
              {error.recurrenceTimeStart && (
                <Typography color="error" variant="caption">
                  Start time is required and must be before end time
                </Typography>
              )}
            </Stack>
            <Stack spacing={1}>
              <TimePicker
                label="End Time"
                sx={{ flex: 1 }}
                value={endTime}
                onChange={onEndTimeChange}
              />
              {error.recurrenceTimeEnd && (
                <Typography color="error" variant="caption">
                  End time is required and must be after start time
                </Typography>
              )}
            </Stack>
            <Stack spacing={1}>
              <DayPicker
                selectedDays={selectedDays}
                onChange={setSelectedDays}
              />
              {error.recurrenceDays && (
                <Typography color="error" variant="caption" mt="0">
                  Days are required
                </Typography>
              )}
            </Stack>
          </>
        )}
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
