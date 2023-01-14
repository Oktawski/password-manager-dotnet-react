import { Button, Dialog, DialogActions, DialogContentText, DialogTitle, IconButton } from "@mui/material";
import DeleteIcon from '@mui/icons-material/Delete';


interface ConfirmDeletePasswordProps {
    deletePasswordAsync: Function,
    id: string|null,
    open: boolean,
    handleClose: any 
}


export function ConfirmDeletePassword(props: ConfirmDeletePasswordProps) {
    const deletePassword = () => {
        props.deletePasswordAsync(props.id);
        props.handleClose();
    };

    return (
        <div>
            <Dialog
                open={props.open}
                onClose={props.handleClose}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
            >
                <DialogTitle id="alert-dialog-title">
                    Do you want to delete this password?
                </DialogTitle>
                <DialogActions>
                    <Button onClick={props.handleClose}>Close</Button>
                    <IconButton onClick={deletePassword}>
                        Delete
                        <DeleteIcon />
                    </IconButton>
                </DialogActions>
            </Dialog>
        </div>
    );
}
