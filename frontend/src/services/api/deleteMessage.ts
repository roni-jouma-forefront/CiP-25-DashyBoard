
export async function deleteMessage(id :string) {
  const apiUrl = import.meta.env.VITE_BASE_URL || "http://localhost:5000";

  const res = await fetch(`${apiUrl}/api/messages/${id}`, {
    method: "DELETE",
  });

  if (!res.ok) {
    throw new Error(`Couldn't delete messages`);
  } else {
    return "Deleted";
  }
}
