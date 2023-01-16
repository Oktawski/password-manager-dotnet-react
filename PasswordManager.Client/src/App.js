import { Component, useEffect, useState } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/navigation/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { LoginPage } from './components/authentication/Login';
import { authenticationService } from './services/authentication.service';
import { PrivateRoute } from './components/PrivateRoute';
import { Passwords } from './components/passwords/Passwords';

import './custom.css'
import { RegisterPage } from './components/authentication/Register';
import { UserAccount } from './components/UserAccount';

export default function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        var subscription = authenticationService.isLoggedInObservable.subscribe(setIsLoggedIn);

        return () => subscription.unsubscribe();
    });

    return (
        <Layout isLoggedIn={isLoggedIn} >
            <Route path='/login' component={LoginPage} />
            <Route path="/register" component={RegisterPage} />
            <PrivateRoute exact path='/' component={Home} />
            <PrivateRoute exact path='/passwords' component={Passwords} />
            <PrivateRoute exact path='/userAccount' component={UserAccount} />
            <PrivateRoute exact path='/counter' component={Counter} />
            <PrivateRoute exact path='/fetch-data' component={FetchData} />
        </Layout>
    )
}
