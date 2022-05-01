import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { LoginPage } from './components/Login';
import { authenticationService } from './services/authentication.service';
import { PrivateRoute } from './components/PrivateRoute';

import './custom.css'
import { RegisterPage } from './components/Register';

export default class App extends Component {
  static displayName = App.name;

  constructor(props) {
    super(props);

    this.state = {
      accessToken: null
    }
  }

  componentDidMount() {
    authenticationService.accessToken.subscribe(token => this.setState({ accessToken: token }));
  }

  render () {
    const { accessToken } = this.state;
    return (
      <Layout>
        <Route path='/login' component={LoginPage} />
        <Route path="/register" component={RegisterPage} />
        <PrivateRoute exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
      </Layout>
    );
  }
}
