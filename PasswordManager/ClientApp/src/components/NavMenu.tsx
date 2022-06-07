import { Link, useHistory } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import MenuIcon from '@mui/icons-material/Menu';
import './NavMenu.css';
import { authenticationService } from '../services/authentication.service';
import { useEffect, useState } from 'react';

export function NavMenu() {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);

    useEffect(() => {
        const subscription = authenticationService
            .isLoggedInObservable
            .subscribe(setIsLoggedIn);
        
        subscription.unsubscribe();
    });

    const history = useHistory();

    const handleLogout = async (event: any) => {
        event.preventDefault();

        await authenticationService.logout();

        if (!isLoggedIn) {
            history.push("/");
        }
    }

    const linkStyle = {
        textDecoration: "none",
        color: "inherit"
    };

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
                    { !isLoggedIn &&
                        <Box>
                            <Link style={linkStyle} to="/login">
                                <Button sx={{ mr: 2 }} color="inherit">
                                    Login
                                </Button>
                            </Link>
                            <Link style={linkStyle} to="/register">
                                <Button color="inherit">
                                    Register
                                </Button>
                            </Link>
                        </Box>
                    }
                    { isLoggedIn &&
                        <Box>
                            <Button color="inherit" onClick={handleLogout}>Logout</Button>
                        </Box>
                    }
                </Toolbar>
            </AppBar>
        </Box>
    );
}
