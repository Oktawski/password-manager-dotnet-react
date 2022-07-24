import LoadingButton from "@mui/lab/LoadingButton";
import { FormControl, Grid, TextField } from "@mui/material";
import React from "react";
import { useState } from "react";
import { AddPasswordRequest } from "../../requests/password.request";
import { passwordService } from "../../services/password.service";

export function AddPassword() {
    const [application, setApplication] = useState("");
    const [password, setPassword] = useState("");
    const [loading, setLoading] = useState(false);

    const handleAddPassword = async (event: any) => {
        event.preventDefault();

        setLoading(true);
            
        await passwordService.add(new AddPasswordRequest(application, password));
        await passwordService.fetchPasswords();

        clearForm();
        setLoading(false)
    }

    const clearForm = () => {
        setApplication("");
        setPassword("");
    }

    return(
        <form onSubmit={handleAddPassword}>
            <Grid container spacing={2} paddingLeft={1} paddingRight={1} marginBottom={2}>
                <Grid item xs={5}>
                    <FormControl fullWidth>
                        <TextField label="Application"
                            value={application}
                            variant="outlined"
                            required
                            onChange={e => setApplication(e.target.value)}
                        />
                    </FormControl>
                </Grid>
                <Grid item xs={5}>
                    <FormControl fullWidth>
                        <TextField label="Password"
                            value={password}
                            variant="outlined"
                            required
                            onChange={e => setPassword(e.target.value)}
                            type="password"
                        />
                    </FormControl>
                </Grid>
            
                <Grid item xs={2}>
                    <LoadingButton type="submit" variant="contained" loading={loading}>
                        Add
                    </LoadingButton>
                </Grid>
            </Grid>
        </form>
    )
}