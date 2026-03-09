import { Stack, Typography } from "@mui/material";
import { SettingsForm } from "../../components/admin/forms/SettingsForm";
import { AddStaffForm } from "../../components/admin/forms/AddStaffForm";

export default function SettingsPAge() {
  return (
    <>
      <Typography variant="h2">Settings</Typography>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <SettingsForm />
        <AddStaffForm />
      </Stack>
    </>
  );
}
