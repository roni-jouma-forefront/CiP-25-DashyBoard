import type { MessageBackend, MessageUI } from "../types/message.types";

export const mapMessageFromApi = (msg: MessageBackend): MessageUI => {
  return {
    id: msg.id,
    messageScope: msg.messageScope,
    title: msg.title,
    content: msg.content,
    status: msg.isActive ? "posted" : "pending",
    PostDateTime:
      msg.postDate && msg.postTime
        ? `Posted: ${msg.postDate} - ${msg.postDate}`
        : null,
    deleteDateTime:
      msg.deleteDate && msg.deleteTime
        ? `Posted: ${msg.deleteDate} - ${msg.deleteTime}`
        : null,
  };
};
