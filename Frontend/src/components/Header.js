// components/Header.js
import React from "react";
import { AppBar, Toolbar, Typography, Box, Button } from "@mui/material";
import { useAuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
const Header = ({ title }) => {
  const { isAuthenticated, currentUser } = useAuthContext();
  const { setIsAuthenticated } = useAuthContext();
  const navigate = useNavigate();
  const handleLogout = () => {
    setIsAuthenticated(true); // Update authentication state
    localStorage.removeItem("token");
    // Remove token from local storage
    navigate("/login");
    window.location.reload();
  };

  return (
    <AppBar position="sticky" sx={{ bgcolor: "#D6001C", zIndex: 1100 }}>
      <Toolbar sx={{ justifyContent: "space-between" }}>
        <Typography variant="h6">Asset Management System {title}</Typography>
        <Box sx={{ display: "flex", alignItems: "center" }}>
          {isAuthenticated && (
            <Typography variant="body1" color="inherit" sx={{ marginRight: 1 }}>
              Welcome, {currentUser.name}!
            </Typography>
          )}
          {isAuthenticated ? (
            <Button color="inherit" onClick={handleLogout}>
              Logout
            </Button>
          ) : (
            <Button color="inherit" href="/login">
              Login
            </Button>
          )}
        </Box>
      </Toolbar>
    </AppBar>
  );
};

export default Header;
