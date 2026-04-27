import { DashboardForm } from "../../components/admin/forms/DashboardForm";
import Watch from "../../components/base/watch";
import { Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";
import { useMessagesAdmin } from "../../hooks";

const hotelId = import.meta.env.VITE_HOTEL_ID;

export default function AdminHome() {
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
    onSubmit
  } = useMessagesAdmin({ hotelId });

  return (
    <>
      <Typography variant="h2">DashyBoard</Typography>
      <Watch location="Stockholm" timeZone="UTC"></Watch>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <DashboardForm onSubmit={onSubmit} />
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
      </Stack>
    </>
  );
}
