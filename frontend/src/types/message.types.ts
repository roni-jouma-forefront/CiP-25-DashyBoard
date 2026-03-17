import type { MsgStatus } from "./theme.types";

export type MessageScope = "all" | "checkingOut" | "checkingIn";


export type MessageBackend = {
  id: string;
  messageScope: MessageScope;
  title: string;
  content: string;
  postDate: string | null;
  postTime: string | null;
  deleteDate: string | null;
  deleteTime: string | null;
  isActive: boolean;
};

export type MessageUI = {
  id: string;
  messageScope: MessageScope;
  title: string;
  content: string;
  status: MsgStatus;
  postDateTime: string | null;
  deleteDateTime: string | null;
}