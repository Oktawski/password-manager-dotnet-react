import { Route, Redirect } from 'react-router-dom';
import { authenticationService } from '../services/authentication.service';

export function PrivateRoute({ component: Component, ...rest }) {
    return (
        <Route {...rest} render={props => {
            if (!authenticationService.isLoggedIn) {
                return <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
            }

            return <Component {...props} />
        }} />
    );
}