import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword'; // Añadimos el componente de Olvidar Contraseña
import ThemeToggle from './components/ThemeToggle'; 
import InfoButton from './components/InfoButton'; 
import MainPage from './components/MainPage';  // Componente para la página principal

const App = () => {
  const [darkMode, setDarkMode] = useState(false); 

  return (
    <div className={darkMode ? 'dark-mode' : ''}> 
      <h1><span translate="no">GrammaGO!</span></h1>

      <InfoButton darkMode={darkMode} /> 
      <ThemeToggle darkMode={darkMode} setDarkMode={setDarkMode} /> 

      <Router>
        <Routes>
          <Route path="/" element={<Navigate to="/login" />} />
          <Route path="/login" element={<Login darkMode={darkMode} />} />
          <Route path="/register" element={<Register darkMode={darkMode} />} />
          <Route path="/forgot-password" element={<ForgotPassword darkMode={darkMode} />} /> 
          <Route path="/main" element={<MainPage darkMode={darkMode} />} /> {/* Nueva ruta */}
        </Routes>
      </Router>
    </div>
  );
};

export default App;
