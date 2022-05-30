import { Link, useHistory } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import './NavMenu.css';
import { authenticationService } from '../services/authentication.service';

export function NavMenu() {

  const history = useHistory();

  const handleLogout = async (event: any) => {
    event.preventDefault();

    await authenticationService.logout();

    if (authenticationService.accessTokenValue === null) {
      history.push("/");
    }
  }
  
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
            { authenticationService.accessTokenValue === null &&
              <Box>
                <Button color="inherit">
                  <Link style={{ textDecoration: 'none', color: 'inherit' }} to="/login">Login</Link>
                </Button>
                <Button color="inherit">
                  <Link style={{ textDecoration: "none", color: "inherit" }} to="/register">Register</Link>
                </Button>
              </Box>
              
            }
            { authenticationService.accessTokenValue !== null &&
              <Button color="inherit" onClick={ handleLogout }>Logout</Button>
            }
          </Toolbar>
        </AppBar>
      </Box>
  );
}
  