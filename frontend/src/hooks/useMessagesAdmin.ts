import React, { useState } from "react";
import type { MessageBackend, MessageUI } from "../types/message.types";
import { getMessages } from "../services/api/getMessagesAdmin";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { postMessage } from "../services/api/postMessage";
import { deleteMessage } from "../services/api/deleteMessage";

type UseMessageAccordionParams = {
  initialMessages?: MessageUI[];
  hotelId: string;
  bookingId?: string;
};

// Websocket??

export const useMessagesAdmin = ({
  initialMessages = [],
  bookingId,
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
    enabled: !!bookingId,
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

  const onSubmit = (formData: MessageBackend) => {
    mutate(formData);
  };

  const [editingId, setEditingId] = useState("");
  const [formData, setFormData] = useState<MessageBackend>({
    id: "",
    bookingId: null,
    hotelId: import.meta.env.VITE_HOTEL_ID,
    title: "",
    content: "",
    recurring: false,
    postAt: null,
    expiresAt: null,
    isActive: false,
    author: "",
  });

  const startEdit = (msg: MessageUI) => {
    setEditingId(msg.id);

    setFormData((prev) => ({
      ...prev,
      id: msg.id,
      title: msg.title,
      content: msg.content,
    }));
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    e.preventDefault();

    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const saveEdit = (id: string) => {
    queryClient.setQueryData<MessageUI[]>(
      ["messages", bookingId],
      (prev = []) =>
        prev.map((msg) =>
          msg.id === id
            ? { ...msg, title: formData.title, content: formData.content }
            : msg,
        ),
    );
    setEditingId("");
  };

  const cancelEdit = () => {
    setEditingId("");
  };

  const handleDelete = (id: string) => {
    deleteMutate(id);
  };

  return {
    messages: data,
    isLoading,
    isPending,
    error,
    editingId,
    formData,
    startEdit,
    saveEdit,
    cancelEdit,
    handleChange,
    onSubmit,
    handleDelete,
  };
};
