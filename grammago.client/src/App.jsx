import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ForgotPassword from './components/ForgotPassword';
import ThemeToggle from './components/ThemeToggle';
import InfoButton from './components/InfoButton';
import MainPage from './components/MainPage';
import PersonalData from './components/PersonalData';

const AppContent = ({ darkMode, setDarkMode }) => {
  const location = useLocation(); 


  const userId = 123;

  return (
    <div className={darkMode ? 'dark-mode' : ''}>
      {/* Mostrar el título y el InfoButton solo si no estamos en la MainPage */}
      {location.pathname !== '/main' && location.pathname !== '/personal-data' && (
        <>
          <h1><span translate="no">GrammaGO!</span></h1>
          <InfoButton darkMode={darkMode} />
        </>
      )}

      <ThemeToggle darkMode={darkMode} setDarkMode={setDarkMode} />

      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="/login" element={<Login darkMode={darkMode} />} />
        <Route path="/register" element={<Register darkMode={darkMode} />} />
        <Route path="/forgot-password" element={<ForgotPassword darkMode={darkMode} />} />
        <Route path="/main" element={<MainPage darkMode={darkMode} />} />
        <Route path="/personal-data" element={<PersonalData darkMode={darkMode} userId={userId} />} />
      </Routes>
    </div>
  );
};

const App = () => {
  const [darkMode, setDarkMode] = useState(false);

  return (
    <Router>
      <AppContent darkMode={darkMode} setDarkMode={setDarkMode} />
    </Router>
  );
};

export default App;
