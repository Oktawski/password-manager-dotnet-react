import { Grid, Button } from "@mui/material"
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';
import { GridColDef, GridRenderCellParams, GridRowId } from "@mui/x-data-grid";


export const passwordsColumns = (showPassword: any): GridColDef[]  => {
    return [
        { field: 'id', headerName: 'ID', minWidth: 70, flex: 1 },
        { field: 'application', headerName: 'Application', minWidth: 130, flex: 1 },
        { field: 'currentValue', headerName: 'Value', minWidth: 130, flex: 1, },
        {
            field: "actions",
            headerName: "Actions",
            sortable: false,
            flex: 1,
            renderCell: (params) => {
                return actionsRow(params, () => showPassword(params.id.toString()));
            }
        }
    ];
};

const actionsRow = (params: any, showPasswordForId: React.MouseEventHandler) => {
    return (
        <Grid container
            direction="column"
            justifyContent="center"
            alignItems="stretch"
        >
            <Grid item>
                <Button fullWidth onClick={ showPasswordForId }>
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
                <Button fullWidth>
                    <DeleteIcon />
                    Remove
                </Button>
            </Grid>
        </Grid>
    );
};
