import React, { useState } from "react";
import type { MessageBackend, MessageUI } from "../types/message.types";
import { getMessages } from "../services/api/getMessagesAdmin";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { postMessage } from "../services/api/postMessage";
import { deleteMessage } from "../services/api/deleteMessage";
import { updateMessage } from "../services/api/updateMessage";
import { Day, type DateTime } from "../types/types";
import dayjs, { Dayjs } from "dayjs";

type UseMessageAccordionParams = {
  initialMessages?: MessageUI[];
  hotelId: string;
  bookingId?: string;
};

export const useMessagesAdmin = ({
  initialMessages = [],
  bookingId,
  hotelId,
}: UseMessageAccordionParams) => {
  const queryClient = useQueryClient();
  const {
    data = initialMessages,
    isLoading,
    isPending,
    error,
  } = useQuery<MessageUI[]>({
    queryKey: ["messages", bookingId],
    queryFn: () => getMessages({ bookingId }),
    enabled: !!hotelId,
    refetchInterval: 10_000,
    refetchIntervalInBackground: true,
  });

  const { mutate } = useMutation<string, Error, MessageBackend>({
    mutationFn: postMessage,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages", bookingId] });
    },
  });

  const { mutate: deleteMutate } = useMutation<string, Error, string>({
    mutationFn: deleteMessage,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages", bookingId] });
    },
  });

  const { mutate: updateMutate } = useMutation<string, Error, MessageBackend>({
    mutationFn: updateMessage,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages", bookingId] });
    },
  });

  const onSubmit = (formData: MessageBackend) => {
    mutate(formData);
  };

  const [editingId, setEditingId] = useState("");
  const [startTime, setStartTime] = useState<Dayjs | null>(null);
  const [endTime, setEndTime] = useState<Dayjs | null>(null);
  const [selectedDays, setSelectedDays] = useState<Day[]>([]);

  const [formData, setFormData] = useState<MessageBackend>({
    id: "",
    bookingId: null,
    hotelId: import.meta.env.VITE_HOTEL_ID,
    title: "",
    content: "",
    recurring: false,
    recurrenceType: null,
    postAt: null,
    expiresAt: null,
    isActive: false,
    recurrenceDays: null,
    recurrenceTimeStart: null,
    recurrenceTimeEnd: null,
    author: "",
  });

  const startEdit = (msg: MessageUI) => {
    setEditingId(msg.id);

    setFormData((prev) => ({
      ...prev,
      id: msg.id,
      title: msg.title,
      content: msg.content,
      postAt: msg.postAt,
      expiresAt: msg.expiresAt,
      recurring: msg.recurring,
      recurrenceTimeStart: msg.recurrenceTimeStart,
      recurrenceTimeEnd: msg.recurrenceTimeEnd,
      recurrenceDays: msg.recurrenceDays,
    }));

    setStartTime(
      msg.recurrenceTimeStart
        ? dayjs(msg.recurrenceTimeStart, "HH:mm:ss")
        : null,
    );
    setEndTime(
      msg.recurrenceTimeEnd ? dayjs(msg.recurrenceTimeEnd, "HH:mm:ss") : null,
    );
    setSelectedDays(
      msg.recurrenceDays ? (msg.recurrenceDays.split(",") as Day[]) : [],
    );
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    e.preventDefault();

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleDateTimeChange = ({ field, value }: DateTime) => {
    if (field === "post") {
      setFormData((prev) => ({
        ...prev,
        postAt: value ? value.toISOString() : null,
      }));
    } else {
      setFormData((prev) => ({
        ...prev,
        expiresAt: value ? value.toISOString() : null,
      }));
    }
  };

  const handleRecurrenceTimeChange = (
    field: "recurrenceTimeStart" | "recurrenceTimeEnd",
    value: Dayjs | null,
  ) => {
    if (field === "recurrenceTimeStart") {
      setStartTime(value);
      setFormData((prev) => ({
        ...prev,
        recurrenceTimeStart: value ? value.format("HH:mm:ss") : null,
      }));
    } else {
      setEndTime(value);
      setFormData((prev) => ({
        ...prev,
        recurrenceTimeEnd: value ? value.format("HH:mm:ss") : null,
      }));
    }
  };

  const handleRecurrenceDaysChange = (value: Day[]) => {
    setSelectedDays(value);
    setFormData((prev) => ({
      ...prev,
      recurrenceDays: value.length > 0 ? value.join(",") : null,
    }));
  };

  const saveEdit = (id: string) => {
    updateMutate({ ...formData, id });
    setEditingId("");
    setStartTime(null);
    setEndTime(null);
    setSelectedDays([]);
  };

  const cancelEdit = () => {
    setEditingId("");
    setStartTime(null);
    setEndTime(null);
    setSelectedDays([]);
  };

  const handleDelete = (id: string) => {
    deleteMutate(id);
  };

  const regularMessages = data.filter((m) => !m.recurring);
  const recurringMessages = data.filter((m) => m.recurring);

  return {
    regularMessages,
    recurringMessages,
    isLoading,
    isPending,
    error,
    editingId,
    formData,
    startTime,
    endTime,
    selectedDays,
    startEdit,
    saveEdit,
    cancelEdit,
    handleChange,
    handleDateTimeChange,
    handleRecurrenceTimeChange,
    handleRecurrenceDaysChange,
    onSubmit,
    handleDelete,
  };
};
