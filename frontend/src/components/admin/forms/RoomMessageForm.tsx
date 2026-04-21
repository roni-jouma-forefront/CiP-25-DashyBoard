import { Box, Button, Stack, Typography } from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";
import { useState } from "react";
import type { Dayjs } from "dayjs";
import type { MessageBackend } from "../../../types/message.types";

interface RoomFormData extends Omit<
  MessageBackend,
  "postAt" | "expiresAt" | "id" | "isActive"
> {
  postDate: Dayjs | null;
  postTime: Dayjs | null;
  expiresTime: Dayjs | null;
  expiresDate: Dayjs | null;
}
interface RoomFormProps {
  onSubmit: (formData: MessageBackend) => void;
  bookingId?: string | null;
}

export const RoomMessageForm = ({ onSubmit, bookingId }: RoomFormProps) => {
  const [formData, setFormData] = useState<RoomFormData>({
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: bookingId ? bookingId : null,
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
      bookingId: formData.bookingId,
      hotelId: import.meta.env.VITE_HOTEL_ID,
      title: formData.title,
      content: formData.content,
      isActive: true,
      recurring: false,
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
        flex: 1,
        p: 2,
        borderRadius: 2,
        boxShadow: 1,
        background: "white",
      }}
      onSubmit={handleSubmit}
    >
      <Typography variant="h5" mb={3}>
        Message
      </Typography>
      <Stack spacing={3}>
        <MessageBaseForm
          handleChange={handleChange}
          onPostTimeChange={onPostTimeChange}
          onPostDateChange={onPostDateChange}
          onExpiresTimeChange={onExpiresTimeChange}
          onExpiresDateChange={onExpiresDateChange}
          title={formData.title}
          content={formData.content}
        />
        <Button variant="contained" type="submit">
          Post
        </Button>
      </Stack>
    </Box>
  );
};
