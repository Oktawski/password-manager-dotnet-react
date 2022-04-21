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

export function LoginPage() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    
    const history = useHistory();

    const handleSubmit = async (event: any) => {
        event.preventDefault();
        await authenticationService.login(username, password)
        if (authenticationService.accessTokenValue != null) {
            history.push("/");
        }
    }


    return (
        <Box sx={{ mt: 2 }}>
            <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Login</Typography>
            <form onSubmit={ handleSubmit }>
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

                    <Button sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' type='submit'>Login</Button>

                </FormGroup>
            </form>
        </Box>
    );
}