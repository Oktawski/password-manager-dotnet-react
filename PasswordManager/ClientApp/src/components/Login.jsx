import React from 'react';
import { FormControl, FormGroup, InputLabel, Input, FormHelperText, Button } from '@mui/material'

export class LoginPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div>
                <h2>Login</h2>
                <form>
                    <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                        <FormControl>
                            <InputLabel htmlFor='username'>Username</InputLabel>
                            <Input id='username-input' aria-describedby='username-description'/>
                            <FormHelperText id='username-description'>Type your username</FormHelperText>
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <InputLabel htmlFor='password'>Password</InputLabel>
                            <Input id='password-input' aria-describedby='password-description'/>
                            <FormHelperText id='password-description'>Type your password</FormHelperText>
                        </FormControl>

                        <Button sx={{ mt: 2, mx: 'auto', width: '50%' }} variant='contained' type='submit'>Login</Button>

                    </FormGroup>
                </form>
            </div>
        );
    }
    
}