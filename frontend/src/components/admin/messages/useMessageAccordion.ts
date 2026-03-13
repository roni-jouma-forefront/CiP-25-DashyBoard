import React, { useState } from "react";
import type { MessageBackend, MessageUI } from "../../../types/message.types";

type UseMessageAccordionParams = {
  initialMessages: MessageUI[];
};

const useMessageAccordion = ({
  initialMessages,
}: UseMessageAccordionParams) => {
  const [messages, setMessages] = useState(initialMessages);
  const [editingId, setEditingId] = useState("");
  const [formData, setFormData] = useState<MessageBackend>({
    id: "",
    messageScope: "all",
    title: "",
    content: "",
    postDate: "",
    postTime: "",
    deleteDate: "",
    deleteTime: "",
    isActive: false,
  });

  const startEdit = (msg: MessageUI) => {
    setEditingId(msg.id);

    setFormData((prev) => ({
      ...prev,
      id: msg.id,
      messageScope: msg.messageScope,
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
    editingId,
    formData,
    startEdit,
    saveEdit,
    cancelEdit,
    handleChange,
  };
};

export default useMessageAccordion;
