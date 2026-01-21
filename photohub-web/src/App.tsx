
/*import React, { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('Loading...');

  // Paste the exact HTTPS forwarded URL from the Ports panel (no trailing slash)
  const apiBase = 'https://symmetrical-pancake-9jpxv7rq74pcxw5j-5000.app.github.dev';

  useEffect(() => {
    fetch(`${apiBase}/weatherforecast`)
      .then(res => res.json())
      .then(data => setMessage(JSON.stringify(data, null, 2)))
      .catch(err => setMessage(`Error: ${err.message}`));
  }, []);

  return (
    <div style={{ padding: 24 }}>
      <h1>PhotoHub Demo</h1>
      <pre>{message}</pre>
    </div>
  );
}


export default App;
*/


import React from "react";
import PhotoUploader from "./components/PhotoUploader";

function App() {
  return (
    <div style={{ padding: 24 }}>
      <h1>PhotoHub Demo</h1>
      <PhotoUploader />
    </div>
  );
}

export default App;
