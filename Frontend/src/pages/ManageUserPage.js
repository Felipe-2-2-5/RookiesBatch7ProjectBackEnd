// pages/ManageUserPage.js
import { ArrowDownward, ArrowDropDown, ArrowDropUp, ArrowUpward, Delete, Edit, Search } from "@mui/icons-material";
import { Sheet } from "@mui/joy";
import {
  Box,
  Button,
  FormControl,
  IconButton,
  InputAdornment,
  InputLabel,
  MenuItem,
  Pagination,
  Paper,
  Select,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
} from "@mui/material";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import { FilterRequest } from "../services/Service";

//reformat code from 	2017-09-18T00:00:00 to 19/08/2017
const formatDate = (dateString) => {
  const date = new Date(dateString);
  return date.toLocaleDateString('en-GB'); // en-GB format gives the desired "dd/mm/yyyy" format
};

const ManageUserPage = () => {
  const navigate = useNavigate();
  const [totalCount, setTotalCount] = useState();
  const [filterRequest, setFilterRequest] = useState({
    searchTerm: "",
    sortColumn: "",
    sortOrder: "",
    page: 1,
    pageSize: "20",
    type: ""
  });
  const [users, setUser] = useState([]);

  const getUsers = async (filterRequest) => {
    const res = await FilterRequest(filterRequest);

    setUser(res.data.data);
    setTotalCount(res.data.totalCount);
  };

  useEffect(() => {
    getUsers(filterRequest);
  }, [filterRequest]);


  //Search state to set in filter request after entered
  const [searchTerm, setSearchTerm] = useState("");
  const handleSearchChange = (e) => {
    console.log(e.target.value);
    setSearchTerm(e.target.value);
  };
  const handleKeyPress = (e) => {
    if (e.key === 'Enter') {
      setFilterRequest((prev) => ({
        ...prev,
        searchTerm: searchTerm,
      }));
    }
  };
  const handleOnBlur = () => {
    setFilterRequest((prev) => ({
      ...prev,
      searchTerm: searchTerm,
    }));
  }

  // console.log("filter", filterRequest);

  const handleTypeChange = (e) => {
    console.log(e.target.value);
    if (e.target.value === "All") {
      setFilterRequest({
        type: "",
      })
    } else {
      setFilterRequest({
        type: e.target.value,
      })
    }

  };

  const handlePageChange = (e, value) => {
    setFilterRequest((prev) => ({
      ...prev,
      page: value,
    }));
  };

  const handleHeaderClick = (column) => {
    setFilterRequest((prev) => {
      let newSortOrder;
      let newSortColumn;

      if (prev.sortColumn === column) {
        // Toggle between "descend", "", and reset
        if (prev.sortOrder === "descend") {
          newSortOrder = "";
          newSortColumn = column;
        } else if (prev.sortOrder === "") {
          newSortOrder = "";
          newSortColumn = "";
        } else {
          newSortOrder = "descend";
          newSortColumn = column;
        }
      } else {
        // If switching columns, start with "descend"
        newSortOrder = "descend";
        newSortColumn = column;
      }

      return {
        ...prev,
        sortColumn: newSortColumn,
        sortOrder: newSortOrder,
      };
    });
  };

  const getSortIcon = (column) => {
    if (filterRequest.sortColumn === column) {
      if (filterRequest.sortOrder === "descend") {
        return <ArrowDropDown />;
      }
      if (filterRequest.sortOrder === "") {
        return <ArrowDropUp />;
      }
    }
    return null;
  };
  return (
    <>
      <Paper
        elevation={3}
        style={{
          padding: "20px",
          marginLeft: "100px",
          width: "1200px",
          height: "calc(100vh - 150px)",
        }}
      >
        <h3 style={{ color: "#D6001C" }}>User List</h3>
        <Box
          sx={{ display: "flex", alignItems: "center", marginBottom: "20px" }}
        >
          <FormControl variant="outlined" sx={{ minWidth: 120 }}>
            <InputLabel>Type</InputLabel>
            <Select
              label="Type"
              value={filterRequest.type}
              name="type"
              onChange={handleTypeChange}
            >
              <MenuItem value="All">All</MenuItem>
              <MenuItem value="Admin">Admin</MenuItem>
              <MenuItem value="Staff">Staff</MenuItem>
            </Select>
          </FormControl>
          <TextField
            variant="outlined"
            label="Search"
            value={searchTerm}
            name="search"
            onChange={handleSearchChange}
            onBlur={handleOnBlur}
            onKeyPress={handleKeyPress}
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <Search />
                </InputAdornment>
              ),
            }}
            sx={{ marginLeft: "auto", marginRight: "20px" }}
          />
          <Button
            variant="contained"
            sx={{ backgroundColor: "#D6001C", height: "56px" }}
            onClick={() => navigate("/create-user")}
          >
            Create new user
          </Button>
        </Box>
        <TableContainer component={Paper}>
          <Sheet sx={{ height: 565, overflow: "auto" }}>
            <Table>
              <TableHead
                sx={{
                  fontWeight: "bold",
                  position: "sticky",
                  top: 0,
                  zIndex: 1,
                  backgroundColor: "white",
                }}
              >
                <TableRow>
                  <TableCell>
                    <Button
                      variant="text"
                      onClick={() => handleHeaderClick("code")}
                      endIcon={getSortIcon("code")}
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Staff Code
                    </Button>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="text"
                      onClick={() => handleHeaderClick("name")}
                      endIcon={getSortIcon("name")}
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Full Name
                    </Button>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="text"
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Username
                    </Button>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="text"
                      onClick={() => handleHeaderClick("date")}
                      endIcon={getSortIcon("date")}
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Joined Date
                    </Button>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="text"
                      onClick={() => handleHeaderClick("type")}
                      endIcon={getSortIcon("type")}
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Type
                    </Button>
                  </TableCell>
                  <TableCell>
                    <Button
                      variant="text"
                      onClick={() => handleHeaderClick("action")}
                      endIcon={getSortIcon("action")}
                      sx={{
                        fontWeight: "bold",
                        textTransform: "none",
                        padding: 0,
                        minWidth: "auto",
                        color: "black",
                      }}
                    >
                      Actions
                    </Button>
                  </TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {users.map((user, index) => (
                  <TableRow key={index}>
                    <TableCell>{user.staffCode}</TableCell>
                    <TableCell>
                      {user.firstName + " " + user.lastName}
                    </TableCell>
                    <TableCell>{user.userName}</TableCell>
                    <TableCell>{formatDate(user.joinedDate)}</TableCell>
                    <TableCell>{user.type}</TableCell>
                    <TableCell>
                      <IconButton color="primary">
                        <Edit />
                      </IconButton>
                      <IconButton color="secondary">
                        <Delete />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </Sheet>
        </TableContainer>
        <Box
          sx={{
            display: "flex",
            justifyContent: "flex-end",
            paddingTop: "10px",
          }}
        >
          <Pagination
            count={Math.ceil(totalCount / filterRequest.pageSize)}
            variant="outlined"
            shape="rounded"
            page={filterRequest.page}
            onChange={handlePageChange}
            sx={{
              "& .MuiPaginationItem-root": {
                color: "red",
              },
              "& .Mui-selected": {
                backgroundColor: "#D6001C",
                color: "white",
              },
            }}
          />
        </Box>
      </Paper>
    </>
  );
};

export default ManageUserPage;
