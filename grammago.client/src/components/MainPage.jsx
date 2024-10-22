import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; 
import './MainPage.css'; 

const MainPage = ({ darkMode }) => {
  const [menuVisible, setMenuVisible] = useState(false);
  const navigate = useNavigate();

  const toggleMenu = () => {
    setMenuVisible(!menuVisible);
  };

  const goToPersonalData = () => {
    navigate('/personal-data'); 
  };

  const handleLogout = () => {
    
    navigate('/login'); 
  };

  return (
    <div className={`main-page-container ${darkMode ? 'dark' : ''}`}>
      <header className="main-page-header">
        <h1>Bienvenido a GrammaGO!</h1>
        <p>Aquí podrás acceder a tus lecciones de gramática y ejercicios de inglés.</p>
      </header>

      <section className="exercise-links">
        
        <h2>Ejercicios de Inglés</h2>
        <div className="link-container">
          <a href="https://www.curso-ingles.com/aprender/cursos/nivel-basico" target="_blank" rel="noopener noreferrer" className="exercise-link" title="Ejercicios Nivel Básico">
            Nivel Básico
          </a>
          <a href="https://www.curso-ingles.com/aprender/cursos/nivel-intermedio" target="_blank" rel="noopener noreferrer" className="exercise-link" title="Ejercicios Nivel Intermedio">
            Nivel Intermedio
          </a>
          <a href="https://www.curso-ingles.com/aprender/cursos/nivel-avanzado" target="_blank" rel="noopener noreferrer" className="exercise-link" title="Ejercicios Nivel Avanzado">
            Nivel Avanzado
          </a>
          <a href="https://www.curso-ingles.com/aprender/cursos/ingles-negocios" target="_blank" rel="noopener noreferrer" className="exercise-link" title="Ejercicios Inglés de Negocios">
            Inglés de negocios
          </a>
          <a href="https://www.curso-ingles.com/aprender/cursos/vocabulario-viajar" target="_blank" rel="noopener noreferrer" className="exercise-link" title="Ejercicios Vocabulario para Viajar">
            Vocabulario para viajar
          </a>
        </div>
      </section>

      
      <div className="circular-button-container">
        <button className="circular-button" onClick={toggleMenu}>☰</button>
        {menuVisible && (
          <div className="menu">
            <button className="menu-item" onClick={goToPersonalData}>
              Datos personales
            </button>
            <button className="menu-item" onClick={handleLogout}>
              Logout
            </button>
          </div>
        )}
      </div>
    </div>
  );
};

export default MainPage;
