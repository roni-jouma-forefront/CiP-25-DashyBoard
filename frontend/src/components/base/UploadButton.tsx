import React from "react";
import Button from "@mui/material/Button";
import FileUploadIcon from "@mui/icons-material/FileUpload";

export default function UploadButton() {
  return (
    <Button
      variant="contained"
      color="secondary"
      startIcon={<FileUploadIcon />}
      sx={{ margin: 2, padding: "10px 20px" }}
    >
      Upload
    </Button>
  );
}
