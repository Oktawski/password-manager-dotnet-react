import { Password } from "../model/Password";
import { AddPasswordRequest } from "../requests/password.request";
import { authenticationService } from "./authentication.service";

export const passwordService = {
    getAll,
    add
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
    // const mockUrl = "https://949b2115-bb70-427a-b8c6-0b53627d0630.mock.pstmn.io/passwordManager/password/getall";

    const response = await fetch(prodUrl, options);
    const body: Array<Password> = await response.json();

    return body;
}

async function add(request: AddPasswordRequest): Promise<string> {
    const options = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*",
            "Authorization": `Bearer ${authenticationService.accessTokenValue!}`
        },
        body: JSON.stringify(request)
    };

    const prodUrl = "https://localhost:7265/api/Password/add";

    const response = await fetch(prodUrl, options);
    console.log(response);
    const body = await response.json();
    console.log(body);
    
    

    return body;
}