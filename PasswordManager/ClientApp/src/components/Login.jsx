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

        this.state = {
            username: "",
            password: ""
        };
    }

    render() {
        return (
            <div sx={{ mt: 2 }}>
                <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Login</Typography>
                <form>
                    <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                        <FormControl>
                            <TextField id='username-input' label='Username' variant='outlined' value={this.state.username} onChange={e => this.setState({ username: e.target.value})}/>
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='password-input' type='password' label='Password' variant='outlined' value={this.state.password} onChange={e => this.setState({ password: e.target.value })}/>
                        </FormControl>

                        <Button sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' onClick={() => alert("Implement service layer you fool")}>Login</Button>

                    </FormGroup>
                </form>
            </div>
        );
    }
}