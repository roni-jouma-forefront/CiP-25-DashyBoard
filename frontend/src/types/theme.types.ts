export type RoomStatus = "available" | "occupied";
export type MsgStatus = "pending" | "posted" | "delete";

export interface StatusColor {
  background: string;
  text: string;
  border: string;
}
