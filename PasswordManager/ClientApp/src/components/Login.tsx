import React, { useEffect } from 'react';
import { Link, useHistory } from 'react-router-dom';
import { 
    FormControl, 
    FormGroup, 
    TextField, 
    Button, 
    Box,
    Typography} from '@mui/material';
import LoadingButton from '@mui/lab/LoadingButton';
import { useState } from 'react';
import { authenticationService } from '../services/authentication.service';

export function LoginPage() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);
    
    const history = useHistory();

    const handleSubmit = async (event: any) => {
        setLoading(true);
        event.preventDefault();

        let response: boolean = await authenticationService.login(username, password)

        if (response) {
            setLoading(false);
            history.push("/");
        } else {
            alert("Wrong credentials");
        }

        setLoading(false);
    }

    return (
        <Box sx={{ mt: 2 }}>
            <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Login</Typography>
            <form onSubmit={ handleSubmit }>
                <fieldset disabled={ loading }>
                    <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                        <FormControl>
                            <TextField id='username-input' 
                                label='Username' 
                                variant='outlined' 
                                value={ username } 
                                onChange={ e => setUsername(e.target.value )}
                            />
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='password-input' 
                                type='password' 
                                label='Password' 
                                variant='outlined' 
                                value={ password } 
                                onChange={ e => setPassword(e.target.value )}
                            />
                        </FormControl>

                        <LoadingButton sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' type='submit' loading={ loading }>
                            Login
                        </LoadingButton>

                    </FormGroup>
                </fieldset>
            </form>

            <Box sx={{ textAlign: "center", mt: 6 }}>
                <Typography>Dont have an account?</Typography>
                <Button>
                    <Link style={{ textDecoration: 'none', color: 'inherit' }} to="/register">Register</Link>
                </Button>
            </Box>
        </Box>
    );
}