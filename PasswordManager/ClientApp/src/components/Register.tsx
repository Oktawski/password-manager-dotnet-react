import React from 'react';
import { useHistory } from 'react-router-dom';
import { 
    FormControl, 
    FormGroup, 
    TextField, 
    Button, 
    Box,
    Typography
} from '@mui/material';
import { useState } from 'react';
import { authenticationService } from '../services/authentication.service';
import { RegisterRequest } from '../requests/register.request';

export function RegisterPage() {
    const [username, setUsername] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");

    const history = useHistory();

    const handleRegister = async (event: any) => {
        event.preventDefault();
        if (password === confirmPassword) {
            let registerRequest = new RegisterRequest(username, email, password, confirmPassword);
            let response = await authenticationService.register(registerRequest);
        } else {
            alert ("Passwords should match each other (TODO pretty form validation you lazy person)");
        }
    }

    return (
        <Box sx={{ mt: 2 }}>
            <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Register</Typography>
            <form onSubmit={ handleRegister }>
                <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                    <FormControl>
                        <TextField id='username-input' 
                            label='Username' 
                            variant='outlined' 
                            value={ username } 
                            onChange={ e => setUsername(e.target.value) }
                            required
                        />
                    </FormControl>

                    <FormControl sx={{ mt: 4 }}>
                        <TextField id='email-input' 
                            label='Email' 
                            type="email"
                            variant='outlined' 
                            value={ email } 
                            onChange={ e => setEmail(e.target.value) }
                            required
                        />
                    </FormControl>

                    <FormControl sx={{ mt: 4 }}>
                        <TextField id='password-input' 
                            label='Password' 
                            type="password"
                            variant='outlined' 
                            value={ password } 
                            onChange={ e => setPassword(e.target.value) }
                            required
                        />
                    </FormControl>

                    <FormControl sx={{ mt: 4 }}>
                        <TextField id='confirm-password-input' 
                            type='password' 
                            label='Confirm Password' 
                            variant='outlined' 
                            value={ confirmPassword } 
                            onChange={ e => setConfirmPassword(e.target.value) }
                            required
                        />
                    </FormControl>

                    <Button sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' type='submit'>Register</Button>

                </FormGroup>
            </form>
        </Box>
    )
}

