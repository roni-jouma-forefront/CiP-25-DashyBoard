import { useState } from "react";
import {
  Box,
  Chip,
  Stack,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
} from "@mui/material";
import UploadForm from "../../components/admin/forms/UploadForm";
import {
  BookingFilter,
  type BookingStatusFilter,
} from "../../components/admin/BookingFilter";
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
  const backendStatus = filterToStatus(statusFilter);
  const { data: bookings = [], isLoading, error } = useAllBookings(backendStatus);

  return (
    <>
      <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h2">Bookings</Typography>
      </Stack>

      <Stack spacing={2}>
        <BookingFilter selected={statusFilter} onChange={setStatusFilter} />

        {isLoading && <Typography>Loading bookings...</Typography>}
        {error && <Typography color="error">Error loading bookings</Typography>}

        {!isLoading && !error && (
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
                  <TableCell><strong>Room ID</strong></TableCell>
                  <TableCell><strong>Guest ID</strong></TableCell>
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
                        <TableCell sx={{ fontFamily: "monospace", fontSize: "0.75rem" }}>
                          {b.roomId?.slice(0, 8) ?? "—"}
                        </TableCell>
                        <TableCell sx={{ fontFamily: "monospace", fontSize: "0.75rem" }}>
                          {b.guestId?.slice(0, 8) ?? "—"}
                        </TableCell>
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

        <UploadForm />
      </Stack>
    </>
  );
}