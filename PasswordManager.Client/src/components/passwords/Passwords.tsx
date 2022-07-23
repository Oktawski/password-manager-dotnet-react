import { useEffect, useState } from "react";
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { Box, Button, IconButton } from "@mui/material";
import { Password } from "../../model/Password";
import { passwordService } from "../../services/password.service";
import { MappedPassword, passwordHelper } from "./password.helper";
import { AddPassword } from "./AddPassword";


export function Passwords() {
    const [passwords, setPasswords] = useState(Array<MappedPassword>());

    const columns: GridColDef[] = [
        { field: 'id', headerName: 'ID', minWidth: 70, flex: 1 },
        { field: 'application', headerName: 'Application', minWidth: 130, flex: 1 },
        { field: 'currentValue', headerName: 'Value', minWidth: 130, flex: 1, },
        { field: "actions", 
            headerName: "Actions", 
            sortable: false, 
            renderCell: (params) => {
                return actionsRow(params);
            }
            //     return <IconButton onClick={ () => showPasswordFor(params.id.toString()) }>Show</IconButton>
            // }
        }
    ]

    const actionsRow = (params: any) => {
        return (
            <Box>
                <Button onClick={ () => showPasswordForId(params.id.toString()) }>Show</Button>
                <IconButton>Edit</IconButton>
                <IconButton>Remove</IconButton>
            </Box>
        );
    }


    useEffect(() => {
        const getPasswords = async () => {
            const result: Array<Password> = await passwordService.getAll();
            console.log(result);
            const mappedPasswords = passwordHelper.mapPasswords(result);


            setPasswords(mappedPasswords);

            console.log(mappedPasswords);
            console.log(result);
        };

        getPasswords();
    }, []);

    const showPasswordForId = (id: string) => {
        console.log(id);
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


    return (
        <Box>
            <AddPassword/>
            <div style={{ height: 700, width: '100%' }}>
                <div style={{ display: 'flex', height: '100%' }}>
                    <div style={{ flexGrow: 1 }}>
                        <DataGrid 
                            autoHeight
                            getRowHeight={() => 'auto'}
                            columns={columns} 
                            rows={passwords}
                        />
                    </div>
                </div>
            </div>
        </Box>
    )
}
