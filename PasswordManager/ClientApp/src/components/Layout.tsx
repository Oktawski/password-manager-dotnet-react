import { Component } from 'react';
import { Container } from '@mui/material';
import { NavMenu } from './NavMenu';
import { PersistentDrawerLeft } from './MainDrawer';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div style={{ height: "100vh" }}>
                <NavMenu />
                <Container>
                    {this.props.children}
                </Container>
            </div>
        );
    }
}
