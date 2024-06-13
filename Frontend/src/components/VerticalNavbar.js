// components/VerticalNavbar.js
import React from 'react';
import { List, ListItem, ListItemText, Box, Typography } from '@mui/material';
import { Link } from 'react-router-dom';
import Logo from '../assets/images/nashTech-logo-red.svg'; // replace with your logo path
import { useAuthContext } from '../context/AuthContext';

const VerticalNavbar = () => {
    const { isAuthenticated } = useAuthContext();

    return (
        <Box sx={{ bgcolor: 'grey.300', position: 'sticky', top: 64, width: '300px' }}>
            <Box sx={{ bgcolor: 'white', padding: '1rem', textAlign: 'center', display: 'flex', flexDirection: 'column', alignItems: 'flex-start' }}>
                <img src={Logo} alt="Logo" style={{ width: '100px', height: '100px' }} />
                <Typography variant="h6" style={{ color: '#D6001C' }}><b>Online Asset Management</b></Typography>
            </Box>
            <List component="nav">
                <ListItem button component={Link} to="/" sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Home</b>} />
                </ListItem>
                <ListItem button component={Link} to="/manage-user" disabled={!isAuthenticated} sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Manage User</b>} />
                </ListItem>
                <ListItem button component={Link} to="/manage-asset" disabled={!isAuthenticated} sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Manage Asset</b>} />
                </ListItem>
                <ListItem button component={Link} to="/manage-assignment" disabled={!isAuthenticated} sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Manage Assignment</b>} />
                </ListItem>
                <ListItem button component={Link} to="/request-for-returning" disabled={!isAuthenticated} sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Request for Returning</b>} />
                </ListItem>
                <ListItem button component={Link} to="/report" disabled={!isAuthenticated} sx={{ '&:hover': { bgcolor: '#D6001C', '& .MuiListItemText-primary': { color: 'white' } } }}>
                    <ListItemText primary={<b>Report</b>} />
                </ListItem>
            </List>
        </Box>
    );
};

export default VerticalNavbar;
