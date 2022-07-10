import { Box, List, ListItem, TableRow } from "@mui/material";
import { useEffect, useState } from "react";
import { Password } from "../model/Password";
import { passwordService } from "../services/password.service";
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';

const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', width: 70 },
    { field: 'applicationNormalized', headerName: 'Application', width: 130 },
    { field: 'value', headerName: 'Value', width: 130 },
]


export function Passwords() {
    const [passwords, setPasswords] = useState(Array<Password>());

    useEffect(() => {
        const getPasswords = async () => {
            const result: Array<Password> = await passwordService.getAll();
            setPasswords(result);
            console.log(result);
        };

        getPasswords();
    }, []);


    return (
        <div style={{ height: 400, width: "100%" }}>
            <DataGrid 
                columns={columns} 
                rows={passwords}
            />
        </div>
        
    )
}