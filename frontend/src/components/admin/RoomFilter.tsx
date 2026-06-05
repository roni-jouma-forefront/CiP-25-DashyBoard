import { Button, Stack, TextField } from "@mui/material";
 
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
 
export const RoomFilter = ({
  filterOpen,
  filters,
  onToggle,
  onChange,
  onClear,
}: RoomFilterProps) => {
  return (
    <Stack direction="row" spacing={2} alignItems="center">
      {filterOpen && (
        <>
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
        </>
      )}
      <Button variant="outlined" onClick={onToggle}>
        {filterOpen ? "Hide Filters" : "Filter"}
      </Button>
    </Stack>
  );
};
 