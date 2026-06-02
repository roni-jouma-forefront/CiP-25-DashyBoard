import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";

interface alertDialogProps {
  open: boolean;
  alertTitle: string;
  alertContent: string;
  buttonText: string;
  alertAction: () => void;
  onClose: () => void;
}

export default function AlertDialog({
  open,
  alertTitle,
  alertContent,
  buttonText,
  alertAction,
  onClose,
}: alertDialogProps) {
  return (
    <>
      <Dialog
        open={open}
        onClose={onClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">{alertTitle}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            {alertContent}
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={onClose}>Cancel</Button>
          <Button onClick={alertAction} autoFocus>
            {buttonText}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
