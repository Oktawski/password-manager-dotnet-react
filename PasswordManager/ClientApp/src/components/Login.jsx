import React from 'react';
import { 
    FormControl, 
    FormGroup, 
    InputLabel, 
    TextField, 
    FormHelperText, 
    Button, 
    Typography
} from '@mui/material'

export class LoginPage extends React.Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <div sx={{ mt: 2 }}>
                <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Login</Typography>
                <form>
                    <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                        <FormControl>
                            <TextField id='username-input' label='Username' variant='outlined'/>
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='password-input' type='password' label='Password' variant='outlined'/>
                        </FormControl>

                        <Button sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' type='submit'>Login</Button>

                    </FormGroup>
                </form>
            </div>
        );
    }
}