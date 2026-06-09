import { useState, useMemo } from "react";
import {
  Box,
  FormControl,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
  Stack,
  Tooltip,
  Typography,
} from "@mui/material";
import ArrowBackIosNewIcon from "@mui/icons-material/ArrowBackIosNew";
import ArrowForwardIosIcon from "@mui/icons-material/ArrowForwardIos";
import dayjs from "dayjs";
import type { BookingsData } from "../../services/api/GetBookings";

const statusColors: Record<number, string> = {
  0: "#90caf9", // Confirmed - blue
  1: "#ef9a9a", // Cancelled - red
  2: "#bdbdbd", // Completed - grey
  3: "#a5d6a7", // Active - green
};

const statusLabels: Record<number, string> = {
  0: "Confirmed",
  1: "Cancelled",
  2: "Completed",
  3: "Active",
};

interface BookingCalendarProps {
  bookings: BookingsData[];
  rooms: string[];
}

export const BookingCalendar = ({ bookings, rooms }: BookingCalendarProps) => {
  const [currentMonth, setCurrentMonth] = useState(dayjs());
  const [selectedRoom, setSelectedRoom] = useState<string>(rooms[0] ?? "");

  const daysInMonth = currentMonth.daysInMonth();
  const monthStart = currentMonth.startOf("month");

  const roomBookings = useMemo(() => {
    return bookings.filter(
      (b) => (b.roomNumber ?? b.roomId) === selectedRoom
    );
  }, [bookings, selectedRoom]);

  const getBookingForDay = (day: number) => {
    const date = monthStart.add(day - 1, "day");
    return roomBookings.filter((b) => {
      const checkIn = dayjs(b.checkIn);
      const checkOut = dayjs(b.checkOut);
      return (
        (date.isSame(checkIn, "day") || date.isAfter(checkIn, "day")) &&
        (date.isSame(checkOut, "day") || date.isBefore(checkOut, "day"))
      );
    });
  };

  return (
    <Box
      sx={{
        p: 3,
        borderRadius: 3,
        boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
        border: "1px solid rgba(0,0,0,0.06)",
        background: "white",
      }}
    >
      <Stack direction="row" alignItems="center" spacing={2} mb={2}>
        <IconButton onClick={() => setCurrentMonth((m) => m.subtract(1, "month"))}>
          <ArrowBackIosNewIcon fontSize="small" />
        </IconButton>
        <Typography variant="h6" sx={{ minWidth: 160, textAlign: "center" }}>
          {currentMonth.format("MMMM YYYY")}
        </Typography>
        <IconButton onClick={() => setCurrentMonth((m) => m.add(1, "month"))}>
          <ArrowForwardIosIcon fontSize="small" />
        </IconButton>

        <FormControl size="small" sx={{ minWidth: 140 }}>
          <InputLabel>Room</InputLabel>
          <Select
            value={selectedRoom}
            label="Room"
            onChange={(e) => setSelectedRoom(e.target.value)}
          >
            {rooms.map((room) => (
              <MenuItem key={room} value={room}>
                {room}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
      </Stack>

      <Box
        sx={{
          display: "grid",
          gridTemplateColumns: "repeat(7, 1fr)",
          gap: 0.5,
        }}
      >
        {["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"].map((d) => (
          <Typography
            key={d}
            variant="caption"
            fontWeight="bold"
            textAlign="center"
          >
            {d}
          </Typography>
        ))}

        {/* Empty cells for days before month starts (Mon=0) */}
        {Array.from({ length: (monthStart.day() + 6) % 7 }).map((_, i) => (
          <Box key={`empty-${i}`} />
        ))}

        {Array.from({ length: daysInMonth }).map((_, i) => {
          const day = i + 1;
          const dayBookings = getBookingForDay(day);
          const hasBooking = dayBookings.length > 0;
          const topBooking = dayBookings[0];

          return (
            <Tooltip
              key={day}
              title={
                hasBooking
                  ? `${topBooking.guestName ?? "Guest"} — ${statusLabels[topBooking.bookingStatus] ?? "Unknown"}`
                  : "Available"
              }
            >
              <Box
                sx={{
                  aspectRatio: "1",
                  display: "flex",
                  alignItems: "center",
                  justifyContent: "center",
                  borderRadius: 1,
                  fontSize: "0.75rem",
                  fontWeight: hasBooking ? 600 : 400,
                  backgroundColor: hasBooking
                    ? statusColors[topBooking.bookingStatus] ?? "#e0e0e0"
                    : "transparent",
                  border: "1px solid",
                  borderColor: hasBooking ? "transparent" : "rgba(0,0,0,0.08)",
                  cursor: hasBooking ? "default" : "default",
                }}
              >
                {day}
              </Box>
            </Tooltip>
          );
        })}
      </Box>

      {/* Legend */}
      <Stack direction="row" spacing={2} mt={2} flexWrap="wrap">
        {Object.entries(statusLabels).map(([key, label]) => (
          <Stack key={key} direction="row" spacing={0.5} alignItems="center">
            <Box
              sx={{
                width: 12,
                height: 12,
                borderRadius: "50%",
                backgroundColor: statusColors[Number(key)],
              }}
            />
            <Typography variant="caption">{label}</Typography>
          </Stack>
        ))}
      </Stack>
    </Box>
  );
};
