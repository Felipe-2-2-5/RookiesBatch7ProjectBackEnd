import React, { useState, useEffect } from "react";
import {
  CssBaseline,
  Box,
  Typography,
  Paper,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  TextField,
} from "@mui/material";
import Header from "./Header";
import Footer from "./Footer";
import VerticalNavbar from "./VerticalNavbar";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { useAuthContext } from "../context/AuthContext";
import { AppRouter } from "../routes/AppRouter";

const Layout = ({ children }) => {
  const [showLogoutDialog, setShowLogoutDialog] = useState(false);
  const [newPassword, setNewPassword] = useState("");
  const [validationError, setValidationError] = useState("");
  const [showSuccessDialog, setShowSuccessDialog] = useState(false); // New state for success dialog
  const { currentUser, setIsAuthenticated } = useAuthContext();
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (token && currentUser.isFirst) {
      setShowLogoutDialog(true);
    }
  }, [currentUser.isFirst]);

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/");
  };

  const handleCloseDialog = () => {
    setShowLogoutDialog(false);
  };

  const handlePasswordChange = async () => {
    const oldPassword = localStorage.getItem("password");
    const newPasswordRegex =
      /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$/;
    if (newPassword === oldPassword || !newPasswordRegex.test(newPassword)) {
      setValidationError(
        "New password must be 8-16 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character. It must also be different from the old password."
      );
      return;
    }

    try {
      const token = localStorage.getItem("token");
      if (token) {
        const userId = currentUser.id;
        const response1 = await axios.post(
          "https://localhost:7083/api/users/change_password",
          {
            id: userId,
            oldPassword: oldPassword,
            newPassword: newPassword,
            confirmPassword: newPassword,
          }
        );

        if (response1.data === true) {
          const username = currentUser.name;
          const response2 = await axios.post(
            "https://localhost:7083/api/users/login",
            { userName: username, password: newPassword }
          );
          const data = response2.data;
          setIsAuthenticated(true);
          localStorage.setItem("token", data.token);
          currentUser.isFirst = false;

          setNewPassword("");
          setShowLogoutDialog(false);
          localStorage.removeItem("firstLogin");
          setShowSuccessDialog(true);
          localStorage.removeItem("password");
        } else {
          setValidationError("Failed to change password. Please try again.");
        }
      } else {
        setValidationError("User token not found. Please login again.");
        navigate("/");
      }
    } catch (error) {
      console.error("Error:", error);
      setValidationError(
        "An error occurred while changing password. Please try again later."
      );
    }
  };

  const handlePasswordBlur = () => {
    setValidationError("");
    const oldPassword = localStorage.getItem("password");
    if (!newPassword) {
      setValidationError("New password cannot be empty.");
    } else if (newPassword === oldPassword) {
      setValidationError(
        "New password must be different from the old password."
      );
    } else {
      const newPasswordRegex =
        /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$/;
      if (!newPasswordRegex.test(newPassword)) {
        setValidationError(
          "New password must be 8-16 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character."
        );
      }
    }
  };

  return (
    <div>
      <CssBaseline />
      <Header />
      <Box display="flex" p={2}>
        <Box>
          <VerticalNavbar />
        </Box>
        <Box flexGrow={1} ml={2}>
          <main style={{ p: "2" }}>
            <AppRouter />
          </main>
        </Box>
      </Box>
      <Footer />

      <Dialog
        open={currentUser.isFirst && showLogoutDialog}
        onClose={handleCloseDialog}
        disableBackdropClick
        disableEscapeKeyDown
      >
        <DialogTitle>Change Password</DialogTitle>
        <DialogContent>
          <Typography>
            This is the first time you've logged in. You must change your
            password to continue.
          </Typography>
          <Box mt={2}>
            <TextField
              autoFocus
              margin="dense"
              label="New Password"
              type="password"
              fullWidth
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              onBlur={handlePasswordBlur}
            />
          </Box>
          {validationError && (
            <Typography color="error" variant="caption" component="div">
              {validationError}
            </Typography>
          )}
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handlePasswordChange}
            variant="contained"
            color="primary"
          >
            Save
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog
        open={showSuccessDialog}
        onClose={() => setShowSuccessDialog(false)}
        disableBackdropClick
        disableEscapeKeyDown
      >
        <DialogTitle>Success</DialogTitle>
        <DialogContent>
          <Typography>Password changed successfully.</Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setShowSuccessDialog(false)} color="primary">
            OK
          </Button>
        </DialogActions>
      </Dialog>
    </div>
  );
};

export default Layout;
