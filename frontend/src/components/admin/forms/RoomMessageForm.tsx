import { Box, Button, Stack, Typography } from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";
import { useState } from "react";
import type { Dayjs } from "dayjs";
import type { MessageBackend } from "../../../types/message.types";

interface RoomFormData extends Omit<MessageBackend, "postAt" | "expiresAt" | "id" | "isActive"> {
  postAt: Dayjs | null;
  expiresAt: Dayjs | null;
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
    postAt: null,
    expiresAt: null,
    author: "",
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const onPostDateTimeChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, postAt: value }));
  };

  const onExpiresDateTimeChange = (value: Dayjs | null) => {
    setFormData((prev) => ({ ...prev, expiresAt: value }));
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
      postAt: formData.postAt ? formData.postAt.toISOString() : null,
      expiresAt: formData.expiresAt ? formData.expiresAt.toISOString() : null,
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
          onPostDateTimeChange={onPostDateTimeChange}
          onExpiresDateTimeChange={onExpiresDateTimeChange}
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
