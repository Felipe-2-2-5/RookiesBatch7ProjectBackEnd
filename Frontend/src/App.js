import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import AuthProvider from "./context/AuthContext";
import HomePage from "./pages/HomePage";
import LoginPage from "./pages/LoginPage";
import ManageUserPage from "./pages/ManageUserPage";
import ManageAssetPage from "./pages/ManageAssetPage";
import ManageAssignmentPage from "./pages/ManageAssignmentPage";
import RequestForReturningPage from "./pages/RequestForReturningPage";
import ReportPage from "./pages/ReportPage";
import CreateUser from "./pages/users/CreateUser";
import Layout from "./components/Layout";

function App() {
  return (
    <div className="App">
      <Layout />
    </div>
  );
}

export default App;
