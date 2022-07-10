import { OptionUnstyled } from "@mui/base";
import { Password } from "../model/Password";
import { authenticationService } from "./authentication.service";

export const passwordService = {
    getAll
}

async function getAll(): Promise<Array<Password>> {
    const options = {
        method: "GET",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
            "Authorization": `Bearer ${authenticationService.accessTokenValue!}`
        }
    };

    const prodUrl = "https://localhost:7265/api/Password";
    const mockUrl = "https://949b2115-bb70-427a-b8c6-0b53627d0630.mock.pstmn.io/passwordManager/password/getall";

    const response = await fetch(prodUrl, options);
    const body: Array<Password> = await response.json();

    return body;
}