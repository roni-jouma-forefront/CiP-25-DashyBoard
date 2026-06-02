import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";

interface alertDialogProps {
  open: boolean;
  onClose: () => void;
}

export default function AlertDialog(prop: alertDialogProps) {
  return (
    <>
      <Dialog
        open={prop.open}
        onClose={prop.onClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">
          {"Are you sure ou want to checkout guest?"}
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            If you check out this guest all information will be deleted.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={prop.onClose}>Cancel</Button>
          <Button onClick={prop.onClose} autoFocus>
            Check out
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
