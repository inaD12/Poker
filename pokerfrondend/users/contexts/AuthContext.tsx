"use client";

import {
  createContext,
  useContext,
  useEffect,
  useState,
  type ReactNode,
} from "react";
import userService from "../services/users.services";

interface AuthContextType {
  isAuthenticated: boolean;
  login: () => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

   useEffect(() => {
    checkAuth();
  }, []);

  const checkAuth = async () => {
    try {
      await userService.checkAuth();
      setIsAuthenticated(true);
    } catch {
      setIsAuthenticated(false);
    }
  };

  const login = async () => {
    setIsAuthenticated(true);
  };

  const logout = async () => {
    try {
      await userService.logout();
    } finally {
      setIsAuthenticated(false);
      window.location.href = "/login";
    }
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within an AuthProvider");
  return context;
};
