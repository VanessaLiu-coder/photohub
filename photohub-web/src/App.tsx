
import React, { useEffect, useState } from 'react';

function App() {
  const [message, setMessage] = useState('Loading...');
  const apiBase = 'https://automatic-dollop-4wxr4j5gw972ggp-5255.github.dev'; // e.g., https://<codespace-host>:5001

  useEffect(() => {
    fetch(`${apiBase}/weatherforecast`)
      .then(res => res.json())
      .then(data => setMessage(JSON.stringify(data, null, 2)))
      .catch(err => setMessage(`Error: ${err.message}`));
  }, []);

  return (
    <div style={{ padding: 24, fontFamily: 'sans-serif' }}>
      <h1>PhotoHub Demo</h1>
      <pre>{message}</pre>
    </div>
  );
}

export default App;
