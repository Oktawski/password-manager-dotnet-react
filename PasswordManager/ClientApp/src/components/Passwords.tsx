import { useEffect, useState } from "react";
import { Password } from "../model/Password";
import { passwordService } from "../services/password.service";
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { IconButton } from "@mui/material";

type MappedPassword = {
    id: string,
    application: string,
    hiddenValue: string,
    actualValue: string,
    currentValue: string,
    isHidden: boolean
}


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
                return <IconButton onClick={ () => showPasswordFor(params.id.toString()) }>Show</IconButton>
            }
        }
    ]


    useEffect(() => {
        const getPasswords = async () => {
            const result: Array<Password> = await passwordService.getAll();
            const mappedPasswords = mapPasswords(result);

            setPasswords(mappedPasswords);

            console.log(mappedPasswords);
            console.log(result);
        };

        getPasswords();
    }, []);


    const mapPasswords = (passwords: Array<Password>): Array<MappedPassword> => {
        return passwords.map<MappedPassword>(e => { 
            let asteriskedValue = hideValue(e.value);

            return { id: e.id,
                application: e.applicationNormalized, 
                hiddenValue: asteriskedValue,
                actualValue: e.value,
                currentValue: asteriskedValue,
                isHidden: true 
            }
        });
    }

    const hideValue = (value: string) => { return "*".repeat(value.length); }

    const showPasswordFor = (id: string) => {
        console.log(id);
        const pass: MappedPassword = passwords.find(e => e.id === id)!;
        
        if (pass.isHidden) {
            pass.currentValue = pass.actualValue;
        } else {
            pass.currentValue = hideValue(pass.actualValue);
        }
        
        pass.isHidden = !pass.isHidden;

        const passes = passwords.map(e => e.id === id
            ? {...e, isHidden: pass.isHidden, currentValue: pass.currentValue }
            : e);

        setPasswords(passes);
    }


    return (
        <div style={{ height: 700, width: '100%' }}>
            <div style={{ display: 'flex', height: '100%' }}>
                <div style={{ flexGrow: 1 }}>
                    <DataGrid 
                        autoHeight
                        columns={columns} 
                        rows={passwords}
                    />
                </div>
            </div>
        </div>
    )
}
