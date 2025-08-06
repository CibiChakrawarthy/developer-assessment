
import { useState, useEffect } from 'react';

const BACKENDS = [
  { name: 'Java (Spring Boot)', url: 'http://localhost:8080/api/items' },
  { name: '.NET', url: 'http://localhost:5005/api/items' },
];

function App() {
    const [backend, setBackend] = useState(BACKENDS[0].url);
    const [role, setRole] = useState("User");
    const [username, setUsername] = useState("Guest");

  const [items, setItems] = useState<string[]>([]);
  const [input, setInput] = useState('');
  const [loading, setLoading] = useState(false);

  const fetchItems = async () => {
    setLoading(true);
    const response = await fetch(backend);
    const data = await response.json();
    setItems(data);
    setTimeout(() => setLoading(false), 100);
  };

  useEffect(() => {
    fetchItems();
  }, [backend]);

  const addItem = async () => {
    if (!input.trim()) return;
    
    setLoading(true);
    await fetch(backend, {
      method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'role': role,
            'username': username, 
        },
        body: JSON.stringify({ value: input.trim() }),
    });
    setInput('');
    setItems([...items, input.trim()]);
    setLoading(false);
  };

  const deleteItem = async (item: string) => {
    setLoading(true);
    await fetch(backend, {
      method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'role': role,
            'username': username
        },
        body: JSON.stringify({ value: item }),
    });
    setItems(items.filter(i => i !== item));
    setLoading(false);
  };

  return (
    <div style={{ 
      maxWidth: '600px', 
      margin: '0 auto', 
      padding: '20px' 
      }}>
          <div style={{ marginBottom: 16 }}>
              <label>Role:</label>
              <select value={role} onChange={e => setRole(e.target.value)} style={{ marginRight: 16 }}>
                  <option value="Admin">Admin</option>
                  <option value="User">User</option>
              </select>

              <label>Username:</label>
              <input
                  type="text"
                  value={username}
                  onChange={e => setUsername(e.target.value)}
                  placeholder="Enter username"
                  style={{ marginLeft: 8 }}
              />
          </div>

      <h1>Items Manager</h1>
      <div style={{ marginBottom: 16 }}>
        <label htmlFor="backend-select">Backend:</label>
        <select 
          id="backend-select"
          value={backend} 
          onChange={e => setBackend(e.target.value)}
        >
          {BACKENDS.map(b => (
            <option key={b.url} value={b.url}>{b.name}</option>
          ))}
        </select>
      </div>
      <div style={{ marginBottom: 16 }}>
        <input
          type="text"
          value={input}
          onChange={e => setInput(e.target.value)}
          placeholder="Add new item..."
          disabled={loading}
          style={{ marginRight: 8 }}
        />
        <button 
          onClick={addItem} 
          disabled={loading || !input.trim()}
        >
          {loading ? 'Adding...' : 'Add'}
        </button>
      </div>
      {loading && <div style={{ 
        color: '#666', 
        fontStyle: 'italic', 
        textAlign: 'center', 
        padding: '10px' 
      }}>Loading...</div>}
      
      <ul style={{ listStyle: 'none', padding: 0 }}>
        {items.length === 0 && !loading ? (
          <li style={{ color: '#666', fontStyle: 'italic' }}>No items yet</li>
        ) : (
          items.map(item => (
            <li key={item} style={{ 
              display: 'flex', 
              justifyContent: 'space-between', 
              alignItems: 'center',
              padding: '8px 0',
              borderBottom: '1px solid #eee'
            }}>
              <span>{item}</span>
              <button 
                onClick={() => deleteItem(item)} 
                disabled={loading}
              >
                Delete
              </button>
            </li>
          ))
        )}
      </ul>
    </div>
  );
}

export default App;
