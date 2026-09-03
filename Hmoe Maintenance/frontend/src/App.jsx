import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            
            {/* Admin Routes Placeholder */}
            <Route path="/admin/companies" element={<div>Admin Companies (WIP)</div>} />
            <Route path="/admin/technicians" element={<div>Admin Technicians (WIP)</div>} />

            {/* Company Owner Routes Placeholder */}
            <Route path="/company/profile" element={<div>Company Profile (WIP)</div>} />
            <Route path="/company/requests" element={<div>Company Requests (WIP)</div>} />

            {/* Technician Routes Placeholder */}
            <Route path="/technical/jobs" element={<div>Technician Jobs (WIP)</div>} />

            {/* Client Routes Placeholder */}
            <Route path="/client/requests" element={<div>Client Requests (WIP)</div>} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App;
