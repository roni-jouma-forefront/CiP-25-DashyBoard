import { useParams } from "react-router";
import { RoomDetailsForm } from "../../components/admin/forms/RoomDetailsForm";
import { RoomMessageForm } from "../../components/admin/forms/RoomMessageForm";
import { Button, Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";
import AlertDialog from "../../components/admin/AlertDialog";
import React from "react";
import { useBookings, useMessagesAdmin } from "../../hooks";
import { theme } from "../../theme";
import { useGuestName } from "../../hooks/useGuestName";

const hotelId = import.meta.env.VITE_HOTEL_ID;

export default function Room() {
  const { roomNumber, bookingId } = useParams();
  const [dialogOpen, setDialogOpen] = React.useState(false);
  const {
    messages,
    isLoading,
    error,
    editingId,
    formData,
    startEdit,
    handleChange,
    saveEdit,
    cancelEdit,
    onSubmit,
  } = useMessagesAdmin({ hotelId, bookingId });
  const {
    data: bookingsData,
    error: bookingsError,
    isLoading: bookingsIsLoading,
  } = useBookings({
    bookingId: bookingId as string,
  });
  const {
    data: guestData,
    error: guestError,
    isLoading: guestIsLoading,
  } = useGuestName({
    guestId: bookingsData?.guestId ?? "",
  });

  const handleDialog = () => {
    setDialogOpen(true);
  };

  const handleClose = () => {
    setDialogOpen(false);
  };

  if (bookingsIsLoading || guestIsLoading)
    return <Typography>Loading data...</Typography>;
  if (bookingsError || guestError)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: theme.palette.error.main }}>
        Error: {(bookingsError ?? guestError)?.message}
      </Typography>
    );
  if (!bookingsData) return <Typography>No booking data found</Typography>;

  return (
    <>
      <Stack
        direction="row"
        spacing={2}
        justifyContent="space-between"
        alignItems="flex-end"
      >
        <Typography variant="h2">Details for room {roomNumber}</Typography>
        <Button
          variant="contained"
          color="error"
          onClick={() => handleDialog()}
        >
          Check out
        </Button>
      </Stack>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <RoomDetailsForm
          bookingId={bookingId}
          firstName={guestData?.firstName}
          lastName={guestData?.lastName}
          departureDate={bookingsData.checkOut}
          departureFlight={bookingsData.flightNumber}
        />
        <RoomMessageForm onSubmit={onSubmit} bookingId={bookingId} />
      </Stack>
      {bookingId && (
        <MessageAccordion
          messages={messages}
          editingId={editingId}
          isLoading={isLoading}
          error={!!error}
          formData={formData}
          startEdit={startEdit}
          handleChange={handleChange}
          saveEdit={saveEdit}
          cancelEdit={cancelEdit}
        />
      )}
      <AlertDialog open={dialogOpen} onClose={handleClose} />
    </>
  );
}
