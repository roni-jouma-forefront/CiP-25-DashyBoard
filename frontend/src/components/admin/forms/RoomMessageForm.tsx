import { Box, Button, Stack, Typography } from "@mui/material";
import { MessageBaseForm } from "./MessageBaseForm";
import { useEffect, useState } from "react";
import type { Dayjs } from "dayjs";
import type { MessageBackend } from "../../../types/message.types";

interface RoomFormData extends Omit<
  MessageBackend,
  | "postAt"
  | "expiresAt"
  | "id"
  | "isActive"
  | "recurringType"
  | "recurrenceDays"
  | "recurrenceTimeStart"
  | "recurrenceTimeEnd"
> {
  postAt: Dayjs | null;
  expiresAt: Dayjs | null;
  postAtError: string | null;
  expiresAtError: string | null;
}

interface RoomFormProps {
  onSubmit: (formData: MessageBackend) => void;
  bookingId?: string | null;
  isPostPending?: boolean;
  isPostSuccess?: boolean;
}

export const RoomMessageForm = ({
  onSubmit,
  bookingId,
  isPostPending,
  isPostSuccess,
}: RoomFormProps) => {
  const [showSuccess, setShowSuccess] = useState(false);

  useEffect(() => {
    if (isPostSuccess) {
      const showTimer = setTimeout(() => setShowSuccess(true), 0);
      const hideTimer = setTimeout(() => setShowSuccess(false), 5000);
      return () => {
        clearTimeout(showTimer);
        clearTimeout(hideTimer);
      };
    }
  }, [isPostSuccess]);

  const [formData, setFormData] = useState<RoomFormData>({
    hotelId: import.meta.env.VITE_HOTEL_ID,
    bookingId: bookingId ? bookingId : null,
    title: "",
    content: "",
    recurring: false,
    recurrenceType: null,
    postAt: null,
    expiresAt: null,
    author: "",
    postAtError: null,
    expiresAtError: null,
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
      recurrenceType: null,
      postAt: formData.postAt ? formData.postAt.toISOString() : null,
      expiresAt: formData.expiresAt ? formData.expiresAt.toISOString() : null,
      author: formData.author,
      recurrenceDays: null,
      recurrenceTimeStart: null,
      recurrenceTimeEnd: null,
    });

    setFormData((prev) => ({
      ...prev,
      title: "",
      content: "",
      author: "",
      postAt: null,
      expiresAt: null,
    }));
  };

  return (
    <Box
      component="form"
      sx={{
        flex: 1,
        p: 3,
        borderRadius: 3,
        boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
        border: "1px solid rgba(0,0,0,0.06)",
        background: "white",
      }}
      onSubmit={handleSubmit}
    >
      <Typography variant="h5" mb={3}>
        Message
      </Typography>
      <Stack spacing={2}>
        <MessageBaseForm
          handleChange={handleChange}
          onPostDateTimeChange={onPostDateTimeChange}
          onExpiresDateTimeChange={onExpiresDateTimeChange}
          title={formData.title}
          content={formData.content}
          author={formData.author}
          postAtError={formData.postAtError}
          expiresAtError={formData.expiresAtError}
          postAt={formData.postAt}
          expiresAt={formData.expiresAt}
        />
        <Stack spacing={1} alignItems="flex-end">
          {showSuccess && (
            <Typography variant="body2" color="success.main">
              Message posted successfully
            </Typography>
          )}
          <Button variant="contained" type="submit" disabled={isPostPending}>
            {isPostPending ? "Posting..." : "Post"}
          </Button>
        </Stack>
      </Stack>
    </Box>
  );
};
