export type RoomStatus = "available" | "occupied";
export type MsgStatus = "pending" | "posted" | "delete" | "expired";

export interface StatusColor {
  background: string;
  text: string;
  border: string;
}
