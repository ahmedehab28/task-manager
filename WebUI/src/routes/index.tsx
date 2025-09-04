import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'

import MainLayout from '../pages/layouts/MainLayout'

import Home from '../pages/Home/Home'
import Login from '../pages/Login/Login'
import Signup from '../pages/Signup/Signup'
import Workspace from '../pages/Workspace/Workspace'
import Projects from '../pages/Projects/Projects'
import Boards from '../pages/Boards/Boards'


export default function AppRoutes() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Home />} />

        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />

        {/* Public for now. Later, wrap with PrivateRoute */}
        <Route path="/workspace" element={<MainLayout />} >
          <Route index element={<Workspace />} />
          <Route path="projects" element={<Projects />} />
          <Route path="boards" element={<Boards />} />
        </Route>

        {/* 404 fallback */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}