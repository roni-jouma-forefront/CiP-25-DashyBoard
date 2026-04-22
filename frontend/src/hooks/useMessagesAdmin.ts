import React, { useEffect, useState } from "react";
import type { MessageBackend, MessageUI } from "../types/message.types";
import { getMessages } from "../services/api/getMessagesAdmin";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { postMessage } from "../services/api/postMessage";

type UseMessageAccordionParams = {
  initialMessages?: MessageUI[];
  hotelId: string;
  bookingId?: string;
};

// Websocket??

export const useMessages = ({
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
    enabled: !!initialMessages,
    // staleTime: 2 * 5 * 1000,
  });

  const { mutate } = useMutation<string, Error, MessageBackend>({
    mutationFn: postMessage,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["messages", bookingId] });
    },
  });

  const onSubmit = (formData: MessageBackend) => {
    mutate(formData);
  };

  const [messages, setMessages] = useState<MessageUI[]>(data);
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

  useEffect(() => {
    setMessages(data);
  }, [data]);

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
    setMessages((prev) =>
      prev.map((msg) => {
        if (msg.id === id) {
          return {
            ...msg,
            title: formData.title,
            content: formData.content,
          };
        } else {
          return msg;
        }
      }),
    );
    setEditingId("");
  };

  const cancelEdit = () => {
    setEditingId("");
  };

  return {
    messages,
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
  };
};
