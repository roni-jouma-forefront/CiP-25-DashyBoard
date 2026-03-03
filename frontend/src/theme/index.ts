import { createTheme } from "@mui/material/styles";
import type { RoomStatus, StatusColor, MsgStatus } from "../types/theme.types";

const roomStatusColors: Record<RoomStatus, StatusColor> = {
  available: {
    background: "#DCFCE7",
    text: "#166534",
    border: "#22C55E",
  },
  occupied: {
    background: "#FEE2E2",
    text: "#991B1B",
    border: "#EF4444",
  },
};

const msgStatusColors: Record<MsgStatus, StatusColor> = {
  posted: {
    background: "#DCFCE7",
    text: "#166534",
    border: "#22C55E",
  },
  pending: {
    background: "#FEF3C7",
    text: "#92400E",
    border: "#F59E0B",
  },
  delete: {
    background: "#FEE2E2",
    text: "#991B1B",
    border: "#EF4444",
  },
};

export const theme = createTheme({
  palette: {
    primary: {
      main: "#2563EB",
      dark: "#1D4ED8",
      light: "#DBEAFE",
      contrastText: "#FFFFFF",
    },
    secondary: {
      light: "#80D8FF",
      main: "#0288D1",
      dark: "#015F8A",
      contrastText: "#FFFFFF",
    },
    background: {
      default: "#F8FAFC",
      paper: "#FFFFFF",
    },
    text: {
      primary: "#0F172A",
      secondary: "#475569",
    },
    divider: "#C8D8E8",

    roomStatus: roomStatusColors,
    msgStatus: msgStatusColors,

    topbar: {
      background: "#0B1220",
      text: "#CBD5E1",
      activeBackground: "#1B3F8B",
    },
    sidebar: {
      background: "#172036",
      text: "#CBD5E1",
      activeBackground: "#1B3F8B",
    },
  },

  typography: {
    fontFamily: '"Inter", "Roboto", "Helvetica", "Arial", sans-serif',
    h1: { fontWeight: 700, letterSpacing: "-0.5px" },
    h2: { fontWeight: 600 },
    h3: { fontWeight: 600 },
    button: { textTransform: "none", fontWeight: 600 },
  },

  custom: {
    spacing: {
      card: 24,
      section: 32,
      badge: "2px 10px",
    },
    borderRadius: {
      card: 12,
      badge: 20,
      button: 8,
    },
  },
});

export { type RoomStatus };
export const getRoomStatusColor = (status: RoomStatus): StatusColor =>
  roomStatusColors[status];

export { type MsgStatus };
export const getMsgStatusColor = (status: MsgStatus): StatusColor =>
  msgStatusColors[status];
