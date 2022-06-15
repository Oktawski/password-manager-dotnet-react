import { Container, styled } from '@mui/material';
import { Link, useHistory } from 'react-router-dom';
import Box from '@mui/material/Box';
import './NavMenu.css';
import { authenticationService } from '../services/authentication.service';
import { useEffect, useState } from 'react';
import { LinkStyle } from '../helpers/Styles';
import Drawer from '@mui/material/Drawer';
import CssBaseline from '@mui/material/CssBaseline';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import KeyIcon from '@mui/icons-material/Key';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import { NavMenu } from './NavMenu';


const drawerWidth = 240;

const Main = styled('main', { shouldForwardProp: (prop) => prop !== 'open' })<{
    open?: boolean;
}>(({ theme, open }) => ({
    flexGrow: 1,
    padding: theme.spacing(3),
    transition: theme.transitions.create('margin', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    marginLeft: `-${drawerWidth}px`,
    ...(open && {
        transition: theme.transitions.create('margin', {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
        marginLeft: 0,
    }),
}));

const DrawerHeader = styled('div')(({ theme }) => ({
    display: 'flex',
    alignItems: 'center',
    padding: theme.spacing(0, 1),
    // necessary for content to be below app bar
    ...theme.mixins.toolbar,
    justifyContent: 'flex-end',
}));

const PersistentDrawerLeft = (props: any) => {
    return (
        <Box sx={{ display: 'flex' }}>
            <CssBaseline />
            {props.isLoggedIn &&
                <Drawer
                    sx={{
                        width: drawerWidth,
                        flexShrink: 0,
                        '& .MuiDrawer-paper': {
                            width: drawerWidth,
                            boxSizing: 'border-box',
                        },
                    }}
                    variant="persistent"
                    anchor="left"
                    open={props.open}
                >
                    <DrawerHeader />
                    <List>
                        <Link to="/passwords" style={LinkStyle}>
                            <ListItem disablePadding>
                                <ListItemButton>
                                    <ListItemIcon>
                                        <KeyIcon />
                                    </ListItemIcon>
                                    <ListItemText primary="Passwords" />
                                </ListItemButton>
                            </ListItem>
                        </Link>
                    </List>

                    <List sx={{ mt: "auto" }}>
                        <Link to="/userAccount" style={LinkStyle}>
                            <ListItem disablePadding>
                                <ListItemButton>
                                    <ListItemIcon>
                                        <AccountCircleIcon />
                                    </ListItemIcon>
                                    <ListItemText primary="Account" />
                                </ListItemButton>
                            </ListItem>
                        </Link>
                    </List>
                </Drawer>
            }

            <Main open={props.open}>
                <Container>
                    {props.children}
                </Container>
            </Main>
        </Box>
    );
}


export function Layout(props: any) {
    const [isLoggedIn, setIsLoggedIn] = useState<boolean>(false);
    const [open, setOpen] = useState<boolean>(true);

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

    const handleDrawerOpen = () => {
        setOpen(!open);
    }
    return (
        <div style={{ height: "100vh" }}>
            <NavMenu isLoggedIn={isLoggedIn} handleDrawerOpen={handleDrawerOpen} handleLogout={handleLogout} />
            <PersistentDrawerLeft open={open} isLoggedIn={isLoggedIn} children={props.children} />
        </div>
    );
}