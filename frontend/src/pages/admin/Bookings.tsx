import { useMemo, useState } from "react";
import {
  Box,
  Chip,
  IconButton,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Tooltip,
  Typography,
} from "@mui/material";
import CalendarMonthIcon from "@mui/icons-material/CalendarMonth";
import TableChartIcon from "@mui/icons-material/TableChart";
import UploadForm from "../../components/admin/forms/UploadForm";
import {
  BookingFilter,
  type BookingStatusFilter,
} from "../../components/admin/BookingFilter";
import { BookingCalendar } from "../../components/admin/BookingCalendar";
import { useAllBookings } from "../../hooks/useAllBookings";

const statusMap: Record<number, { label: string; color: "success" | "info" | "error" | "default" }> = {
  0: { label: "Confirmed", color: "info" },
  1: { label: "Cancelled", color: "error" },
  2: { label: "Completed", color: "default" },
  3: { label: "Active", color: "success" },
};

function filterToStatus(filter: BookingStatusFilter): number | undefined {
  switch (filter) {
    case "active":
      return 3;
    case "confirmed":
      return 0;
    case "cancelled":
      return 1;
    case "completed":
      return 2;
    default:
      return undefined;
  }
}

export default function BookingsPage() {
  const [statusFilter, setStatusFilter] = useState<BookingStatusFilter>("all");
  const [view, setView] = useState<"table" | "calendar">("table");
  const backendStatus = filterToStatus(statusFilter);
  const { data: bookings = [], isLoading, error } = useAllBookings(backendStatus);

  // For the calendar we also fetch all bookings (unfiltered) so room timelines are complete
  const { data: allBookings = [] } = useAllBookings(undefined);

  const roomNumbers = useMemo(() => {
    const set = new Set<string>();
    allBookings.forEach((b) => {
      const room = b.roomNumber ?? b.roomId;
      if (room) set.add(room);
    });
    return Array.from(set).sort((a, b) => a.localeCompare(b, undefined, { numeric: true }));
  }, [allBookings]);

  return (
    <>
      <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h2">Bookings</Typography>
        <Tooltip title={view === "table" ? "Calendar view" : "Table view"}>
          <IconButton onClick={() => setView((v) => (v === "table" ? "calendar" : "table"))}>
            {view === "table" ? <CalendarMonthIcon /> : <TableChartIcon />}
          </IconButton>
        </Tooltip>
      </Stack>

      <Stack spacing={2}>
        <BookingFilter selected={statusFilter} onChange={setStatusFilter} />

        {isLoading && <Typography>Loading bookings...</Typography>}
        {error && <Typography color="error">Error loading bookings</Typography>}

        {!isLoading && !error && view === "table" && (
          <TableContainer
            component={Box}
            sx={{
              borderRadius: 3,
              boxShadow: "0 4px 24px rgba(0,0,0,0.08)",
              border: "1px solid rgba(0,0,0,0.06)",
              background: "white",
            }}
          >
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell><strong>Room</strong></TableCell>
                  <TableCell><strong>Guest</strong></TableCell>
                  <TableCell><strong>Flight</strong></TableCell>
                  <TableCell><strong>Guests</strong></TableCell>
                  <TableCell><strong>Check-in</strong></TableCell>
                  <TableCell><strong>Check-out</strong></TableCell>
                  <TableCell><strong>Status</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {bookings.length === 0 ? (
                  <TableRow>
                    <TableCell colSpan={7} align="center">
                      No bookings found
                    </TableCell>
                  </TableRow>
                ) : (
                  bookings.map((b) => {
                    const status = statusMap[b.bookingStatus] ?? { label: "Unknown", color: "default" as const };
                    return (
                      <TableRow key={b.id}>
                        <TableCell>{b.roomNumber ?? "—"}</TableCell>
                        <TableCell>{b.guestName ?? "—"}</TableCell>
                        <TableCell>{b.flightNumber || "—"}</TableCell>
                        <TableCell>{b.numberOfGuests}</TableCell>
                        <TableCell>{new Date(b.checkIn).toLocaleDateString()}</TableCell>
                        <TableCell>{new Date(b.checkOut).toLocaleDateString()}</TableCell>
                        <TableCell>
                          <Chip label={status.label} color={status.color} size="small" />
                        </TableCell>
                      </TableRow>
                    );
                  })
                )}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        {!isLoading && !error && view === "calendar" && (
          <BookingCalendar bookings={allBookings} rooms={roomNumbers} />
        )}

        <UploadForm />
      </Stack>
    </>
  );
}