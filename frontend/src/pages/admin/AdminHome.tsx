import { DashboardForm } from "../../components/admin/forms/DashboardForm";
import Watch from "../../components/base/watch";
import { Stack, Typography } from "@mui/material";
import { ActiveMessageAccordion } from "../../components/admin/ActiveMessageAccordion";

export default function AdminHome() {
  return (
    <>
      <Typography variant="h2">DashyBoard</Typography>
      <Watch location="Stockholm" timeZone="UTC"></Watch>
      <Stack direction="row" spacing={2}>
        <DashboardForm />
        <ActiveMessageAccordion />
      </Stack>
    </>
  );
}
