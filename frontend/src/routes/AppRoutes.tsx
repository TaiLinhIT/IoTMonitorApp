import React from "react";
import { Routes, Route } from "react-router-dom";
// import Home from '../pages/Home';
// import About from '../pages/About';
import Login from "../pages/auth/Signin";
import Register from "../pages/auth/Signup";

const AppRoutes = () => {
  return (
    <Routes>
      {/* <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} /> */}
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
    </Routes>
  );
};

export default AppRoutes;
