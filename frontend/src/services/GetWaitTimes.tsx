export type WaitTimesData = {
  queueName?: string;
  terminal?: string;
  currentTime?: string;
  currentProjectedWaitTime?: string;
  isFastTrack?: boolean;
};

export async function GetWaitTimes(airport: string): Promise<WaitTimesData[]> {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";
  console.log("Fetching waiting times");

  const res = await fetch(`${apiUrl}/api/WaitTimes/${airport}`, {
    headers: { "Content-Type": "application/json" },
  });

  if (!res.ok) {
    throw new Error(`Couldn't get info for the terminals waiting times`);
  }

  const json = await res.json();
  const waitTimesFiltered = (json as WaitTimesData[]).filter(
    (waitTime) => waitTime.isFastTrack !== true,
  );
  return waitTimesFiltered;
}
