import { Stack, Typography } from "@mui/material";
import { SettingsForm } from "../../components/admin/forms/SettingsForm";

export default function SettingsPAge() {
  return (
    <>
      <Typography variant="h2">Settings</Typography>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <SettingsForm />
      </Stack>
    </>
  );
}
