import { DashboardForm } from "../../components/admin/forms/DashboardForm";
import Watch from "../../components/base/watch";
import { Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/messages/MessageAccordion";
import messageService from "../../components/admin/messages/messageService";

export default function AdminHome() {
  const initialMessages = messageService();

  return (
    <>
      <Typography variant="h2">DashyBoard</Typography>
      <Watch location="Stockholm" timeZone="UTC"></Watch>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <DashboardForm />
        <MessageAccordion initialMessages={initialMessages}/>
      </Stack>
    </>
  );
}
