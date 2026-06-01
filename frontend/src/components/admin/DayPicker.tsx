import {
  Checkbox,
  FormControlLabel,
  FormGroup,
  Typography,
} from "@mui/material";
import { Day } from "../../types/types";

interface DayPickerProps {
  selectedDays: Day[];
  onChange: (days: Day[]) => void;
}

export const DayPicker = ({ selectedDays, onChange }: DayPickerProps) => {
  const handleOnChange = (d: Day) => {
    if (selectedDays.includes(d)) {
      onChange(selectedDays.filter((day) => day !== d));
    } else {
      onChange([...selectedDays, d]);
    }
  };

  const handleAllChange = () => {
    if (!allSelected) {
      onChange(Object.values(Day));
    } else {
      onChange([]);
    }
  };

  const allSelected = Object.values(Day).every((d) => selectedDays.includes(d));

  return (
    <FormGroup>
      <Typography variant="subtitle2">Chose days to show message:</Typography>
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="All"
        checked={allSelected}
        onChange={() => handleAllChange()}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Monday"
        name="mon"
        checked={selectedDays.includes(Day.mon)}
        onChange={() => handleOnChange(Day.mon)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Tusday"
        name="tus"
        checked={selectedDays.includes(Day.tus)}
        onChange={() => handleOnChange(Day.tus)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Wednesday"
        name="wed"
        checked={selectedDays.includes(Day.wed)}
        onChange={() => handleOnChange(Day.wed)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Thursday"
        name="thu"
        checked={selectedDays.includes(Day.thu)}
        onChange={() => handleOnChange(Day.thu)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Friday"
        name="fri"
        checked={selectedDays.includes(Day.fri)}
        onChange={() => handleOnChange(Day.fri)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Saturday"
        name="sat"
        checked={selectedDays.includes(Day.sat)}
        onChange={() => handleOnChange(Day.sat)}
      />
      <FormControlLabel
        control={<Checkbox size="small" />}
        label="Sunday"
        name="sun"
        checked={selectedDays.includes(Day.sun)}
        onChange={() => handleOnChange(Day.sun)}
      />
    </FormGroup>
  );
};
