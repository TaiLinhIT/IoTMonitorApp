import React from 'react';
import { Link } from 'react-router-dom';

const PageNotFound: React.FC = () => {
  return (
    <div className="relative w-screen h-screen">
      <img src="src/assets/img/404.png" alt="Page Not Found" className="absolute inset-0 w-full h-full object-cover" />
      <Link to="/" className="absolute top-4 left-4 bg-blue-600 text-white px-4 py-2 rounded">Go to Home</Link>
    </div>

    
  );
};

export default PageNotFound;
