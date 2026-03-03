import type { RoomStatus, StatusColor } from "./types";

declare module "@mui/material/styles" {
  interface Palette {
    roomStatus: Record<RoomStatus, StatusColor>;
    sidebar: {
      background: string;
      text: string;
      activeBackground: string;
    };
  }

  interface PaletteOptions {
    roomStatus?: Partial<Record<RoomStatus, Partial<StatusColor>>>;
    sidebar?: Partial<Palette["sidebar"]>;
  }

  interface Theme {
    custom: {
      spacing: {
        card: number;
        section: number;
        badge: string;
      };
      borderRadius: {
        card: number;
        badge: number;
        button: number;
      };
    };
  }

  interface ThemeOptions {
    custom?: {
      spacing?: Partial<Theme["custom"]["spacing"]>;
      borderRadius?: Partial<Theme["custom"]["borderRadius"]>;
    };
  }
}
