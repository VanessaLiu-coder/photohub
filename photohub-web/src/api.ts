
const API_URL = process.env.REACT_APP_API_URL || "https://symmetrical-pancake-9jpxv7rq74pcxw5j-5000.app.github.dev";

export async function uploadPhoto(file: File) {
  const form = new FormData();
  form.append("file", file);
  const res = await fetch(`${API_URL}/api/uploads`, { method: "POST", body: form });
  if (!res.ok) throw new Error(await res.text());
  return res.json() as Promise<{ url: string; contentType: string; size: number }>;
}
