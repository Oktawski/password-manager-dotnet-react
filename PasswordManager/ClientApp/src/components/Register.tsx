import React from 'react';
import {
    FormControl,
    FormGroup,
    TextField,
    Box,
    Typography,
    Snackbar,
    Button,
    IconButton
} from '@mui/material';
import LoadingButton from '@mui/lab/LoadingButton';
import CloseIcon from '@mui/icons-material/Close';
import { useState } from 'react';
import { authenticationService } from '../services/authentication.service';
import { RegisterRequest } from '../requests/register.request';
import { RegisterResponse } from '../responses/register.response';

export function RegisterPage() {
    const [username, setUsername] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [confirmPassword, setConfirmPassword] = useState<string>("");
    const [loading, setLoading] = useState(false);
    const [open, setOpen] = useState(false);
    const [message, setMessage] = useState("");

    const handleRegister = async (event: any) => {
        setLoading(true);
        event.preventDefault();

        if (password !== confirmPassword) {
            alert("Passwords do not match")
            setLoading(false);
            return;
        }

        let registerRequest = new RegisterRequest(username, email, password, confirmPassword);
        let response: RegisterResponse = await authenticationService.register(registerRequest);

        openSnackbar(response.message);
        setLoading(false);
    }

    function openSnackbar(message: string) {
        setOpen(true);
        setMessage(message);
    }

    function closeSnackbar() { setOpen(false); }


    const action = (
        <React.Fragment>
            <IconButton
                size="small"
                aria-label="close"
                color="inherit"
                onClick={closeSnackbar}
            >
                <CloseIcon fontSize="small" />
            </IconButton>
        </React.Fragment>
    );

    return (
        <Box sx={{ mt: 2 }}>
            <Snackbar
                anchorOrigin={{ vertical: "top", horizontal: "center" }}
                open={open}
                autoHideDuration={6000}
                message={message}
                onClose={closeSnackbar}
                action={action}
            />

            <Typography variant="h3" sx={{ textAlign: "center", my: 2 }}>Register</Typography>
            <form onSubmit={handleRegister}>
                <fieldset disabled={loading}>
                    <FormGroup sx={{ mx: 'auto', mt: 8, width: '50%' }}>
                        <FormControl>
                            <TextField id='username-input'
                                label='Username'
                                variant='outlined'
                                value={username}
                                onChange={e => setUsername(e.target.value)}
                                required
                            />
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='email-input'
                                label='Email'
                                type="email"
                                variant='outlined'
                                value={email}
                                onChange={e => setEmail(e.target.value)}
                                required
                            />
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='password-input'
                                label='Password'
                                type="password"
                                variant='outlined'
                                value={password}
                                onChange={e => setPassword(e.target.value)}
                                required
                            />
                        </FormControl>

                        <FormControl sx={{ mt: 4 }}>
                            <TextField id='confirm-password-input'
                                type='password'
                                label='Confirm Password'
                                variant='outlined'
                                value={confirmPassword}
                                onChange={e => setConfirmPassword(e.target.value)}
                                required
                            />
                        </FormControl>

                        <LoadingButton sx={{ mt: 3, mx: 'auto', width: '50%' }} variant='contained' type='submit' loading={loading}>
                            Register
                        </LoadingButton>

                    </FormGroup>
                </fieldset>
            </form>
        </Box>
    )
}
