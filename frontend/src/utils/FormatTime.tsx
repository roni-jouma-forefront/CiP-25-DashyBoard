export default function formatTime(utc: string | null | undefined) {
  if (!utc) return "-";
  return new Date(utc).toLocaleTimeString("sv-SE", {
    hour: "2-digit",
    minute: "2-digit",
    timeZone: "Europe/Stockholm",
  });
}
