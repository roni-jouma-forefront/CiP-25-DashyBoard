import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import { Typography } from "@mui/material";
import { postBookings } from "../../../services/api/postBookings";
import { useState } from "react";

export default function UploadForm() {
  const [uploadComplete, setUploadComplete] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [error, setError] = useState(false);
  const [uploadError, setUploadError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleFileUpload = async (e: React.SyntheticEvent<HTMLFormElement>) => {
    e.preventDefault();
    setUploadComplete(false);
    setUploadError(null);

    if (!file) {
      setError(true);
      return;
    }

    const formData = new FormData();
    formData.append("file", file);

    setLoading(true);
    try {
      await postBookings(formData);
      setUploadComplete(true);
    } catch (err) {
      setUploadError(err instanceof Error ? err.message : "Upload failed");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <form onSubmit={handleFileUpload}>
        <TextField
          type="file"
          variant="outlined"
          fullWidth
          margin="normal"
          onChange={(e) => {
            const input = e.target as HTMLInputElement;
            setFile(input.files?.[0] ?? null);
          }}
        />
        {error && (
          <Typography color="error" variant="body2" sx={{ mb: 2 }}>
            You must submit a file!
          </Typography>
        )}
        <Button
          variant="contained"
          color="primary"
          type="submit"
          disabled={loading}
        >
          {loading ? "Uploading..." : "Upload"}
        </Button>
      </form>
      {uploadComplete && (
        <Typography
          variant="body2"
          color="success.main"
          sx={{ display: "flex", alignItems: "center" }}
        >
          Upload Complete
        </Typography>
      )}
      {uploadError && (
        <Typography color="error" variant="body2" sx={{ mt: 1 }}>
          {uploadError}
        </Typography>
      )}
    </>
  );
}
