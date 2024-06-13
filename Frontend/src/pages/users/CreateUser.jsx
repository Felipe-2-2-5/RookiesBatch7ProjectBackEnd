import React, { useState, useEffect } from "react";
import {
  TextField,
  Radio,
  RadioGroup,
  FormControlLabel,
  Button,
  MenuItem,
  Select,
  InputLabel,
  FormControl,
  Container,
  Typography,
  Box,
  Grid,
  FormHelperText,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { DatePicker } from "@mui/x-date-pickers";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { vi } from "date-fns/locale";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFnsV3";
import { useAuthContext } from "../../context/AuthContext";
import { format } from "date-fns";
import { flushSync } from "react-dom";

const CreateUser = () => {
  const navigate = useNavigate();
  const { currentUser } = useAuthContext();
  const [users, setUsers] = useState({
    firstName: "",
    lastName: "",
    dateOfBirth: null,
    gender: 2,
    joinedDate: null,
    type: 0,
    location: "",
  });

  const [formErrors, setFormErrors] = useState({
    firstName: false,
    lastName: false,
    dateOfBirth: false,
    gender: false,
    joinedDate: false,
    type: false,
    location: false,
  });

  const [touched, setTouched] = useState({
    dateOfBirth: false,
    joinedDate: false,
  });

  const handleChange = (event) => {
    const { name, value } = event.target;
    setUsers({ ...users, [name]: value });
    const isValid = /^[a-zA-Z]{2,20}$/.test(value);

    let errorMessage = "";
    if (value.trim() === "") {
      errorMessage = `${
        name.charAt(0).toUpperCase() + name.slice(1)
      } is required`;
    } else if (value.length < 2 || value.length > 20) {
      errorMessage = `${
        name.charAt(0).toUpperCase() + name.slice(1)
      } must be between 2 and 20 letters`;
    } else if (!isValid) {
      errorMessage = `${
        name.charAt(0).toUpperCase() + name.slice(1)
      } does not contain space`;
    }

    setFormErrors({ ...formErrors, [name]: errorMessage });
  };

  const handleLastNameChange = (event) => {
    const { name, value } = event.target;
    const trimmedValue = value.replace(/\s+/g, " ");
    setUsers({ ...users, [name]: trimmedValue });
    const isValid = /^[a-zA-Z\s]{2,20}$/.test(trimmedValue);

    let errorMessage = "";
    if (trimmedValue.trim() === "") {
      errorMessage = `${
        name.charAt(0).toUpperCase() + name.slice(1)
      } is required`;
    } else if (trimmedValue.length < 2 || trimmedValue.length > 20) {
      errorMessage = `${
        name.charAt(0).toUpperCase() + name.slice(1)
      } must be between 2 and 20 letters`;
    }

    setFormErrors({ ...formErrors, [name]: errorMessage });
  };

  const handleTypeChange = (event) => {
    const { name, value } = event.target;
    setUsers({ ...users, [name]: value });
  };

  const handleDateChange = (name, date) => {
    setUsers({ ...users, [name]: date });
    setTouched({ ...touched, [name]: true });
  };

  const formatDate = (date) => {
    if (!date) return "";
    return format(date, "dd/MM/yyyy");
  };
  useEffect(() => {
    if (users.dateOfBirth) {
      const dob = new Date(users.dateOfBirth);
      const joined = new Date(users.joinedDate);
      const age = Math.floor(
        (Date.now() - dob) / (365.25 * 24 * 60 * 60 * 1000)
      );
      if (age < 18) {
        setFormErrors((prevErrors) => ({ ...prevErrors, dateOfBirth: true }));
      } else {
        setFormErrors((prevErrors) => ({ ...prevErrors, dateOfBirth: false }));
      }
      if (joined < dob) {
        setFormErrors((prevErrors) => ({ ...prevErrors, joinedDate: true }));
      } else {
        setFormErrors((prevErrors) => ({ ...prevErrors, joinedDate: false }));
      }
    } else {
      setFormErrors((prevErrors) => ({
        ...prevErrors,
        dateOfBirth: !users.dateOfBirth,
        joinedDate: !users.joinedDate,
      }));
    }
  }, [users.dateOfBirth, users.joinedDate]);

  const handleSubmit = async (event) => {
    console.log("location : ", currentUser.locality);
    event.preventDefault();
    console.log("adsasdasdsds");
    const hasErrors = Object.values(formErrors).some((error) => error);
    if (!hasErrors) {
      try {
        if (!users.location) {
          users.location = currentUser.locality;
        }
        if (users.location) {
          users.location === "HaNoi"
            ? (users.location = 1)
            : (users.location = 0);
        }
        if (users.gender) {
          users.gender = +users.gender;
        }
        const response = await axios.post("https://localhost:7083/api/users", {
          ...users,
          dateOfBirth: users.dateOfBirth ? formatDate(users.dateOfBirth) : null,
          joinedDate: users.joinedDate ? formatDate(users.joinedDate) : null,
        });
        console.log("Response:", response.data);
        console.log("User created successfully.");
        navigate("/manage-user");
      } catch (error) {
        console.error("Error:", error);
      }
    } else {
      console.log("Form has errors. Please fill all required fields.");
    }
  };

  return (
    <>
      <Container sx={{ display: "flex", justifyContent: "center", my: 4 }}>
        <Box sx={{ width: "60%", borderRadius: 1, p: 1 }}>
          <Typography
            variant="h5"
            sx={{
              mb: 2,
              color: "#d32f2f",
              fontWeight: "bold",
              fontSize: "20px",
            }}
          >
            Create New User
          </Typography>
          <form onSubmit={handleSubmit}>
            <Grid container spacing={1}>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  First Name
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <TextField
                  placeholder="First Name"
                  onBlur={handleChange}
                  fullWidth
                  name="firstName"
                  value={users.firstName}
                  onChange={handleChange}
                  margin="dense"
                  error={formErrors.firstName}
                />
                {formErrors.firstName && (
                  <FormHelperText error>{formErrors.firstName}</FormHelperText>
                )}
              </Grid>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  Last Name
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <TextField
                  placeholder="Last Name"
                  fullWidth
                  name="lastName"
                  value={users.lastName}
                  onBlur={handleLastNameChange}
                  onChange={handleLastNameChange}
                  margin="dense"
                  required
                  error={formErrors.lastName}
                />
                {formErrors.lastName && (
                  <FormHelperText error>{formErrors.lastName}</FormHelperText>
                )}
              </Grid>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  Date of Birth
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <LocalizationProvider dateAdapter={AdapterDateFns} locale={vi}>
                  <DatePicker
                    format="dd/MM/yyyy"
                    label="Date Of Birth"
                    value={users.dateOfBirth}
                    onChange={(date) => handleDateChange("dateOfBirth", date)}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        fullWidth
                        margin="dense"
                        error={formErrors.dateOfBirth && touched.dateOfBirth}
                      />
                    )}
                  />
                </LocalizationProvider>

                {formErrors.dateOfBirth && touched.dateOfBirth && (
                  <FormHelperText error>
                    {users.dateOfBirth
                      ? "User must be at least 18 years old!"
                      : "Date of Birth is required!"}
                  </FormHelperText>
                )}
              </Grid>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  Gender
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <RadioGroup
                  name="gender"
                  value={users.gender}
                  onChange={handleTypeChange}
                  row
                >
                  <FormControlLabel
                    value={1}
                    control={
                      <Radio
                        sx={{
                          color: "#d32f2f",
                          "&.Mui-checked": { color: "#d32f2f" },
                        }}
                      />
                    }
                    label="Female"
                  />
                  <FormControlLabel
                    value={2}
                    control={
                      <Radio
                        sx={{
                          color: "#d32f2f",
                          "&.Mui-checked": { color: "#d32f2f" },
                        }}
                      />
                    }
                    label="Male"
                  />
                </RadioGroup>
              </Grid>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  Joined Date
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <LocalizationProvider dateAdapter={AdapterDateFns} locale={vi}>
                  <DatePicker
                    format="dd/MM/yyyy"
                    label="Joined Date"
                    value={users.joinedDate}
                    onChange={(date) => handleDateChange("joinedDate", date)}
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        fullWidth
                        margin="dense"
                        required
                        error={formErrors.joinedDate && touched.joinedDate}
                      />
                    )}
                  />
                </LocalizationProvider>
                {formErrors.joinedDate && touched.joinedDate && (
                  <FormHelperText error>
                    {users.joinedDate
                      ? "Joined date must be greater than Date of Birth!"
                      : "Joined Date is required!"}
                  </FormHelperText>
                )}
              </Grid>
              <Grid item xs={3} sx={{ display: "flex", alignItems: "center" }}>
                <Typography>
                  Type
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid item xs={9}>
                <FormControl
                  fullWidth
                  margin="dense"
                  required
                  error={formErrors.type}
                >
                  <InputLabel id="type-label">Type</InputLabel>
                  <Select
                    labelId="type-label"
                    name="type"
                    value={users.type}
                    onChange={handleTypeChange}
                    label="Type"
                  >
                    <MenuItem value={1}>Admin</MenuItem>
                    <MenuItem value={0}>Staff</MenuItem>
                  </Select>
                  {formErrors.type && (
                    <FormHelperText error>Type is required!</FormHelperText>
                  )}
                </FormControl>
              </Grid>

              <Grid
                item
                xs={3}
                sx={{
                  display: users.type === 1 ? "flex" : "none",
                  alignItems: "center",
                }}
              >
                <Typography>
                  Location
                  <span style={{ color: "#d32f2f", marginLeft: "4px" }}>*</span>
                </Typography>
              </Grid>
              <Grid
                item
                xs={9}
                sx={{ display: users.type === 1 ? "block" : "none" }}
              >
                <FormControl
                  fullWidth
                  margin="dense"
                  required
                  error={formErrors.location}
                >
                  <InputLabel id="location-label">Location</InputLabel>
                  <Select
                    labelId="location-label"
                    name="location"
                    value={users.location}
                    onChange={handleChange}
                    label="Location"
                  >
                    <MenuItem value="hanoi">Ha Noi</MenuItem>
                    <MenuItem value="hochiminh">Ho Chi Minh</MenuItem>
                  </Select>
                  {formErrors.location && (
                    <FormHelperText error>Location is required!</FormHelperText>
                  )}
                </FormControl>
              </Grid>

              <Grid item xs={12}>
                <Box
                  sx={{ display: "flex", justifyContent: "flex-end", mt: 1 }}
                >
                  <Button
                    variant="contained"
                    type="submit"
                    sx={{ backgroundColor: "#d32f2f", mr: 3 }}
                    disabled={Object.values(formErrors).some((error) => error)}
                    onClick={handleSubmit}
                  >
                    Save
                  </Button>
                  <Button
                    variant="outlined"
                    color="secondary"
                    onClick={() => navigate("/manage-user")}
                  >
                    Cancel
                  </Button>
                </Box>
              </Grid>
            </Grid>
          </form>
        </Box>
      </Container>
    </>
  );
};

export default CreateUser;
