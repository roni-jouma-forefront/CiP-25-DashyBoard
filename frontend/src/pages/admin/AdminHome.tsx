import { DashboardForm } from "../../components/admin/forms/DashboardForm";
import { Box, Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";
import { useMessagesAdmin } from "../../hooks";

const hotelId = import.meta.env.VITE_HOTEL_ID;

export default function AdminHome() {
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
  } = useMessagesAdmin({ hotelId });

  return (
    <>
      <Typography variant="h2" sx={{ fontWeight: 700, color: "#0F172A", letterSpacing: "-0.5px" }}>DashyBoard</Typography>
      <Stack direction="column" spacing={2}>
        <DashboardForm onSubmit={onSubmit} />
        <Box sx={{ display: "grid", gridTemplateColumns: "1fr 1fr", gap: 2 }}>
          <MessageAccordion
            title="General Messages"
            messages={regularMessages}
            editingId={editingId}
            isLoading={isLoading}
            error={!!error}
            formData={formData}
            startEdit={startEdit}
            startTime={startTime}
            endTime={endTime}
            selectedDays={selectedDays}
            handleChange={handleChange}
            handleDateTimeChange={handleDateTimeChange}
            handleRecurrenceTimeChange={handleRecurrenceTimeChange}
            handleRecurrenceDaysChange={handleRecurrenceDaysChange}
            saveEdit={saveEdit}
            cancelEdit={cancelEdit}
            handleDelete={handleDelete}
          />
          <MessageAccordion
            title="Recurring Messages"
            messages={recurringMessages}
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
        </Box>
      </Stack>
    </>
  );
}
