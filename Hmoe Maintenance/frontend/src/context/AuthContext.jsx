import React, { createContext, useState, useEffect } from 'react';
import { authService } from '../api/authService';

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simple mock logic to decode token or check if logged in
    const token = localStorage.getItem('token');
    if (token) {
      try {
        // Decode JWT token to get roles and userId
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        const decoded = JSON.parse(jsonPayload);
        
        // Extract role - roles can be array or string in JWT depending on setup
        // The claim type for roles in .NET is typically "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        const roleKey = Object.keys(decoded).find(key => key.includes('role'));
        
        setUser({
          id: decoded.nameid || decoded.sub || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
          email: decoded.email || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
          roles: roleKey ? (Array.isArray(decoded[roleKey]) ? decoded[roleKey] : [decoded[roleKey]]) : []
        });
      } catch (e) {
        console.error("Token decoding failed", e);
        localStorage.removeItem('token');
      }
    }
    setLoading(false);
  }, []);

  const login = async (credentials) => {
    const data = await authService.login(credentials);
    // After login, we decode the token
    const token = data.token;
    if(token) {
      const base64Url = token.split('.')[1];
      const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
          return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join(''));
      
      const decoded = JSON.parse(jsonPayload);
      const roleKey = Object.keys(decoded).find(key => key.includes('role'));
      
      setUser({
        id: decoded.nameid || decoded.sub || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        email: decoded.email || decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        roles: roleKey ? (Array.isArray(decoded[roleKey]) ? decoded[roleKey] : [decoded[roleKey]]) : []
      });
    }
  };

  const logout = async () => {
    await authService.logout();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};
