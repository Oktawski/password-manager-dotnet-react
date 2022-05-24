import { Route, Redirect } from 'react-router-dom';
import { authenticationService } from '../services/authentication.service';



export { PrivateRoute };

function PrivateRoute({ component: Component, ...rest }) {
    const auth = authenticationService.accessTokenValue !== null;
    console.log(authenticationService.accessTokenValue);
    return (
        <Route {...rest} render={props => {
            if (!auth) {
                return <Redirect to={{ pathname: '/login', state: { from: props.location } }} />
            }

            return <Component {...props} />
        }} />
    );
}