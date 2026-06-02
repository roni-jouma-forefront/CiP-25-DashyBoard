import { useParams } from "react-router";
import { RoomDetailsForm } from "../../components/admin/forms/RoomDetailsForm";
import { RoomMessageForm } from "../../components/admin/forms/RoomMessageForm";
import { Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";
import { useBookings, useMessagesAdmin } from "../../hooks";
import { theme } from "../../theme";
import { useGuestName } from "../../hooks/useGuestName";

const hotelId = import.meta.env.VITE_HOTEL_ID;

export default function Room() {
  const { roomNumber, bookingId } = useParams();
  const {
    regularMessages,
    recurringMessages,
    isLoading,
    error,
    editingId,
    formData,
    startTime,
    endTime,
    selectedDays,
    startEdit,
    handleChange,
    handleDateTimeChange,
    handleRecurrenceTimeChange,
    handleRecurrenceDaysChange,
    saveEdit,
    cancelEdit,
    onSubmit,
    handleDelete,
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

  if (bookingsIsLoading || guestIsLoading)
    return <Typography>Loading data...</Typography>;
  if (bookingsError || guestError)
    return (
      <Typography sx={{ m: 3, opacity: 0.9, color: theme.palette.error.main }}>
        Error: {(bookingsError ?? guestError)?.message}
      </Typography>
    );
  if (!bookingsData) return <Typography>No booking data found</Typography>;
  if (!guestData) return <Typography>No guest data found</Typography>;

  return (
    <>
      <Stack
        direction="row"
        spacing={2}
        justifyContent="space-between"
        alignItems="flex-end"
      >
        <Typography variant="h2">Details for room {roomNumber}</Typography>
      </Stack>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <RoomDetailsForm
          bookingId={bookingId}
          guestId={guestData.id}
          firstName={guestData.firstName}
          lastName={guestData.lastName}
          isPilot={guestData.isPilot}
          departureDate={bookingsData.checkOut}
          departureFlight={bookingsData.flightNumber}
        />
        <RoomMessageForm onSubmit={onSubmit} bookingId={bookingId} />
      </Stack>
      {bookingId && (
        <MessageAccordion
          title="Room Messages"
          messages={[...regularMessages, ...recurringMessages]}
          editingId={editingId}
          isLoading={isLoading}
          error={!!error}
          formData={formData}
          startTime={startTime}
          endTime={endTime}
          selectedDays={selectedDays}
          startEdit={startEdit}
          handleChange={handleChange}
          handleDateTimeChange={handleDateTimeChange}
          handleRecurrenceTimeChange={handleRecurrenceTimeChange}
          handleRecurrenceDaysChange={handleRecurrenceDaysChange}
          saveEdit={saveEdit}
          cancelEdit={cancelEdit}
          handleDelete={handleDelete}
        />
      )}
    </>
  );
}
