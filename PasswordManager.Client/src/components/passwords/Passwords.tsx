import { useEffect, useState } from "react";
import * as xDataGrid from '@mui/x-data-grid';
import { Box, CircularProgress } from "@mui/material";
import { passwordService } from "../../services/password.service";
import { MappedPassword, passwordHelper } from "./password.helper";
import { AddPassword } from "./AddPassword";
import { PasswordList } from "./PasswordList";


export function Passwords() {
    const [passwords, setPasswords] = useState(Array<MappedPassword>());
    const [loading, setLoading] = useState(false);

    const getPasswords = async () => {
        setLoading(true);
        await passwordService.fetchPasswords();
        setLoading(false);
    };

    useEffect(() => {
        getPasswords();
        const subscription = passwordService.passwordsObservable.subscribe(passwords => {
            const mappedPasswords = passwordHelper.mapPasswords(passwords);
            setPasswords(mappedPasswords);
        });

        return () => subscription.unsubscribe();
    }, []);

    const showPasswordForId = (id: string) => {
        const mappedPassword: MappedPassword = passwords.find(e => e.id === id)!;

        mappedPassword.isHidden 
            ? mappedPassword.currentValue = mappedPassword.actualValue
            : mappedPassword.currentValue = passwordHelper.hideValue(mappedPassword.actualValue);    
        
        mappedPassword.isHidden = !mappedPassword.isHidden;

        const mappedPasswords = passwords
            .map(password => password.id === id
                ? {...password, isHidden: mappedPassword.isHidden, currentValue: mappedPassword.currentValue }
                : password);

        setPasswords(mappedPasswords);
    }

    const deleteByIdAsync = async (id: string) => {
        await passwordService.deleteAsync(id);
        await passwordService.fetchPasswords();
    };

    const passwordListProps = {
        passwords: passwords,
        showPassword: showPasswordForId,
        deletePasswordAsync: deleteByIdAsync 
    };

    if (loading) {
        return (
            <Box>
                <AddPassword />
                <Box sx={{ display: "flex", justifyContent: "center", marginTop: 2 }} >
                    <CircularProgress />
                </Box>
            </Box>
        )
    }

    return (
        <Box>
            <AddPassword />
            <PasswordList {...passwordListProps} />
        </Box>
    )
}
