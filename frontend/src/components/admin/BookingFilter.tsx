import { Chip, Stack } from "@mui/material";

export type BookingStatusFilter = "all" | "active" | "confirmed" | "cancelled" | "completed";

const statusOptions: { label: string; value: BookingStatusFilter }[] = [
  { label: "All", value: "all" },
  { label: "Active", value: "active" },
  { label: "Confirmed", value: "confirmed" },
  { label: "Cancelled", value: "cancelled" },
  { label: "Completed", value: "completed" },
];

interface BookingFilterProps {
  selected: BookingStatusFilter;
  onChange: (status: BookingStatusFilter) => void;
}

export const BookingFilter = ({ selected, onChange }: BookingFilterProps) => {
  return (
    <Stack direction="row" spacing={1} flexWrap="wrap">
      {statusOptions.map((opt) => (
        <Chip
          key={opt.value}
          label={opt.label}
          variant={selected === opt.value ? "filled" : "outlined"}
          color={selected === opt.value ? "primary" : "default"}
          onClick={() => onChange(opt.value)}
        />
      ))}
    </Stack>
  );
};
