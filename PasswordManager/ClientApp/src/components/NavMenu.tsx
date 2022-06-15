import { Link } from 'react-router-dom';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import './NavMenu.css';
import { LinkStyle } from '../helpers/Styles';

export function NavMenu(props: any) {
    return (
        <Box sx={{ flexGrow: 1 }}>
            <AppBar position="relative" sx={{ zIndex: 1400 }}>
                <Toolbar>
                    {props.isLoggedIn &&
                        <IconButton
                            size="large"
                            edge="start"
                            color="inherit"
                            aria-label="menu"
                            onClick={props.handleDrawerOpen}
                            sx={{ mr: 2 }}
                        >
                            <MenuIcon />
                        </IconButton>
                    }
                    <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
                        <Link style={{ textDecoration: 'none', color: 'inherit' }}
                            to="/"
                        >
                            Home
                        </Link>
                    </Typography>
                    {!props.isLoggedIn &&
                        <Box>
                            <Link style={LinkStyle} to="/login">
                                <Button sx={{ mr: 2 }} color="inherit">
                                    Login
                                </Button>
                            </Link>
                            <Link style={LinkStyle} to="/register">
                                <Button color="inherit">
                                    Register
                                </Button>
                            </Link>
                        </Box>
                    }
                    {props.isLoggedIn &&
                        <Box>
                            <Button color="inherit" onClick={props.handleLogout}>Logout</Button>
                        </Box>
                    }
                </Toolbar>
            </AppBar>
        </Box>
    );
}
