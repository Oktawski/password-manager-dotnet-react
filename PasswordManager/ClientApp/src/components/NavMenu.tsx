import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import './NavMenu.css';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  render () {
    return (
        <Box sx={{ flexGrow: 1 }}>
          <AppBar position="static">
            <Toolbar>
              <IconButton
                size="large"
                edge="start"
                color="inherit"
                aria-label="menu"
                sx={{ mr: 2 }}
              >
                <MenuIcon />
              </IconButton>
                <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                  <Link style={{ textDecoration: 'none', color: 'inherit' }} to="/">Home</Link>
                </Typography>
              <Button color="inherit">
                <Link style={{ textDecoration: 'none', color: 'inherit' }} to="/login">Login</Link>
              </Button>
            </Toolbar>
          </AppBar>
        </Box>
    );
  }
}
