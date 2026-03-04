import { DashboardForm } from "../../components/admin/forms/DashboardForm";
import Watch from "../../components/base/watch";
import { Stack, Typography } from "@mui/material";
import { MessageAccordion } from "../../components/admin/MessageAccordion";

export default function AdminHome() {
  return (
    <>
      <Typography variant="h2">DashyBoard</Typography>
      <Watch location="Stockholm" timeZone="UTC"></Watch>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <DashboardForm />
        <MessageAccordion />
      </Stack>
    </>
  );
}
