import { Button, Collapse, Stack, TextField } from "@mui/material";

type RoomFilters = {
  roomNumber: string;
  guestName: string;
  flightNumber: string;
};

interface RoomFilterProps {
  filterOpen: boolean;
  filters: RoomFilters;
  onToggle: () => void;
  onChange: (field: keyof RoomFilters, value: string) => void;
  onClear: () => void;
}

export const RoomFilterButton = ({
  filterOpen,
  onToggle,
}: Pick<RoomFilterProps, "filterOpen" | "onToggle">) => (
  <Button variant="outlined" onClick={onToggle}>
    {filterOpen ? "Hide Filters" : "Filter"}
  </Button>
);

export const RoomFilterPanel = ({
  filterOpen,
  filters,
  onChange,
  onClear,
}: Omit<RoomFilterProps, "onToggle">) => (
  <Collapse in={filterOpen}>
    <Stack direction="row" spacing={2} mt={2}>
      <TextField
        label="Room number"
        value={filters.roomNumber}
        onChange={(e) => onChange("roomNumber", e.target.value)}
        size="small"
      />
      <TextField
        label="Guest name"
        value={filters.guestName}
        onChange={(e) => onChange("guestName", e.target.value)}
        size="small"
      />
      <TextField
        label="Flight number"
        value={filters.flightNumber}
        onChange={(e) => onChange("flightNumber", e.target.value)}
        size="small"
      />
      <Button variant="text" onClick={onClear}>
        Clear
      </Button>
    </Stack>
  </Collapse>
);
