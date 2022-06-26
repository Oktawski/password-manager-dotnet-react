import { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/navigation/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { LoginPage } from './components/authentication/Login';
import { Passwords } from './components/Passwords';
import { authenticationService } from './services/authentication.service';
import { PrivateRoute } from './components/PrivateRoute';

import './custom.css'
import { RegisterPage } from './components/authentication/Register';
import { UserAccount } from './components/UserAccount';

export default class App extends Component {
    static displayName = App.name;

    constructor(props) {
        super(props);

        this.state = {
            accessToken: null
        }
    }

    componentDidMount() {
        authenticationService.accessToken.subscribe(token => {
            this.setState({ accessToken: token });
            console.log(this.state.accessToken);
        }); 
    }

    render() {
        return (
            <Layout>
                <Route path='/login' component={LoginPage} />
                <Route path="/register" component={ RegisterPage} />
                <PrivateRoute exact path='/' component={Home} />
                <PrivateRoute exact path='/passwords' component={Passwords} />
                <PrivateRoute exact path='/userAccount' component={UserAccount} />
                <PrivateRoute exact path='/counter' component={Counter} />
                <PrivateRoute exact path='/fetch-data' component={FetchData} />
            </Layout>
        );
    }
}
