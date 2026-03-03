import React from "react";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
// import { Typography } from "@mui/material";

export default function UploadForm() {
//   const handleFileUpload = async (event) => {
//     event.preventDefault();
//     setUploadProgress(0);
//     setUploadComplete(false);

//     const formData = new FormData();
//     const file = event.target.elements.fileInput.files[0];
//     formData.append("file", file);

//     try {
//       await axios.post(
//         "https://api.escuelajs.co/api/v1/files/upload",
//         formData,
//         {
//           onUploadProgress: (progressEvent) => {
//             const percentCompleted = Math.round(
//               (progressEvent.loaded * 100) / progressEvent.total,
//             );
//             setUploadProgress(percentCompleted);
//           },
//         },
//       );
//       setUploadComplete(true);
//     } catch (error) {
//       console.error("Error uploading file:", error);
//     }
//   };

  return (
    <>
      <form>
        <TextField
          type="file"
          variant="outlined"
          // inputProps={{ accept: 'image/*' }}
          fullWidth
          margin="normal"
        />
        <Button variant="contained" color="primary" type="submit">
          Upload
        </Button>
      </form>
      {/* {uploadProgress > 0 && (
        <Typography variant="body2" color="textSecondary">
          Upload Progress: {uploadProgress}%
        </Typography>
      )}
      {uploadComplete && (
        <Typography
          variant="body2"
          color="success.main"
          sx={{ display: "flex", alignItems: "center" }}
        >
          Upload Complete <CheckCircleOutlineOutlined sx={{ ml: 1 }} />
        </Typography>
      )} */}
    </>
  );
}
