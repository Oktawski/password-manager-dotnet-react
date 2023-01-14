import { Grid, Button } from "@mui/material"
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { DataGrid, GridColDef, GridToolbar } from "@mui/x-data-grid";
import { MappedPassword } from "./password.helper";


interface PasswordsProps {
    passwords: Array<MappedPassword>,
    showPassword: Function,
    deletePasswordAsync: Function
}

export function PasswordList(props: PasswordsProps) {

    const columns: GridColDef[] = [
        { field: 'id', headerName: 'ID', minWidth: 70, flex: 1 },
        { field: 'application', headerName: 'Application', minWidth: 130, flex: 1 },
        { field: 'login', headerName: 'Login', minWidth: 130, flex: 1},
        { field: 'currentValue', headerName: 'Value', minWidth: 130, flex: 1 },
        {
            field: "actions",
            headerName: "Actions",
            sortable: false,
            flex: 1,
            renderCell: (params) => {
                return actionsRow(params);
            }
        }
    ];
    
    const actionsRow = (params: any) => {
        return (
            <Grid container
                direction="column"
                justifyContent="center"
                alignItems="stretch"
            >
                <Grid item>
                    <Button fullWidth onClick={ () => props.showPassword(params.id) }>
                        { params.row.isHidden &&
                            <div>
                                <VisibilityIcon />
                                Show
                            </div>
                        }
                        { !params.row.isHidden &&
                            <div>
                                <VisibilityOffIcon />
                                Hide
                            </div>
                        }
                    </Button>
                </Grid>
                    
                <Grid item>
                    <Button fullWidth>
                        <EditIcon />
                        Edit
                    </Button>
                </Grid>

                <Grid item>
                    <Button fullWidth onClick={ () => props.deletePasswordAsync(params.id) }>
                        <DeleteIcon />
                        Remove
                    </Button>
                </Grid>
            </Grid>
        );
    };


    return (
        <DataGrid 
            autoHeight
            getRowHeight={() => 'auto'}
            columns={ columns } 
            rows={ props.passwords }
            disableColumnSelector
            disableDensitySelector
            components={{ Toolbar: GridToolbar }}
            componentsProps={{
                toolbar: {
                showQuickFilter: true,
                quickFilterProps: { debounceMs: 500 }
            }}}
        />
    )
};
