import React, { useState } from 'react';

const ThemeToggle = () => {
  const [darkMode, setDarkMode] = useState(false); 

  
  const toggleTheme = () => {
    setDarkMode(!darkMode);
    
    document.body.classList.toggle('dark-mode', !darkMode);
  };

  return (
    <button
      onClick={toggleTheme}
      style={{
        position: 'absolute',
        top: '20px',
        right: '20px',
        padding: '10px',
        backgroundColor: darkMode ? '#ffffff' : '#242424',
        color: darkMode ? '#242424' : '#ffffff',
        border: 'none',
        borderRadius: '5px',
        cursor: 'pointer',
      }}
    >
      {darkMode ? 'Modo Claro' : 'Modo Oscuro'}
    </button>
  );
};

export default ThemeToggle;
