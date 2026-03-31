import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
// Importera väderikonerna
import "./weather-icons.css";
import App from "./App.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <App />
  </StrictMode>,
);
