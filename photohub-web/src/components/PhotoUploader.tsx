
import React, { useState } from "react";
import { uploadPhoto } from "../api";

export default function PhotoUploader() {
  const [preview, setPreview] = useState<string>();
  const [serverUrl, setServerUrl] = useState<string>();
  const apiBase = process.env.REACT_APP_API_URL || "http://localhost:5000";
  console.log("API base is:", apiBase);

  async function onChange(e: React.ChangeEvent<HTMLInputElement>) {
    const file = e.target.files?.[0];
    if (!file) return;
    setPreview(URL.createObjectURL(file));
    const result = await uploadPhoto(file);
    setServerUrl(`${apiBase}${result.url}`); // result.url is relative
  }

  return (
    <div style={{ display: "grid", gap: 12 }}>
      <input type="file" accept="image/*" onChange={onChange} />
      {preview && <img src={preview} alt="preview" style={{ maxWidth: 400 }} />}
      {serverUrl && (
        <>
          <div>Uploaded to: {serverUrl}</div>
          <img src={serverUrl} alt="uploaded" style={{ maxWidth: 400 }} />
        </>
      )}
    </div>
  );
}
