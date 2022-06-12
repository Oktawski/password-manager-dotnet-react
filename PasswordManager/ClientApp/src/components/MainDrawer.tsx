import { styled } from '@mui/material/styles';
import Drawer from '@mui/material/Drawer';
import { AppBarProps as MuiAppBarProps } from '@mui/material/AppBar';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import KeyIcon from '@mui/icons-material/Key';
import { Link } from 'react-router-dom';
import { authenticationService } from '../services/authentication.service';
import { LinkStyle } from '../helpers/Styles';

const drawerWidth = 240;

interface AppBarProps extends MuiAppBarProps {
  open?: boolean;
}

const DrawerHeader = styled('div')(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  padding: theme.spacing(0, 1),
  // necessary for content to be below app bar
  ...theme.mixins.toolbar,
  justifyContent: 'flex-end',
}));

export const PersistentDrawerLeft = (props: any) => {

  if (!authenticationService.isLoggedIn) {
    return null;
  }

  return (
      <Drawer
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
        variant="temporary"
        anchor="left"
        open={props.open}
      >
        <DrawerHeader/>
        <List>
          <Link to="/passwords" style={LinkStyle} onClick={props.handleDrawerOpen}>
            <ListItem disablePadding>
              <ListItemButton>
                <ListItemIcon>
                  <KeyIcon/>
                </ListItemIcon>
                <ListItemText primary="Passwords"/>
              </ListItemButton>
            </ListItem>
          </Link>
        </List>

        <List sx={{ mt: "auto" }}>
          <Link to="/userAccount" style={LinkStyle} onClick={props.handleDrawerOpen}>
            <ListItem disablePadding>
              <ListItemButton>
                <ListItemIcon>
                  <AccountCircleIcon/>
                </ListItemIcon>
                <ListItemText primary="Account"/>
              </ListItemButton>
            </ListItem>
          </Link>
        </List>
      </Drawer>
  );
}
