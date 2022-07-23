import { BehaviorSubject } from "rxjs";
import { RegisterRequest } from "../requests/register.request";
import { RegisterResponse } from "../responses/register.response";

const accessTokenSubject = new BehaviorSubject(localStorage.getItem("accessToken"));
const isLoggedInSubject = new BehaviorSubject(accessTokenSubject.value !== null);

export const authenticationService = {
    login,
    register,
    logout,
    accessToken: accessTokenSubject.asObservable(),
    isLoggedInObservable: isLoggedInSubject.asObservable(),
    get accessTokenValue(): string | null { return accessTokenSubject.value; },
    get isLoggedIn(): boolean { return isLoggedInSubject.value; }
};

async function login(username: string, password: string) : Promise<boolean> {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify({ username, password })
    };

    const prodUrl = "https://localhost:7265/api/User/Authenticate";
    // const mockUrl = "https://949b2115-bb70-427a-b8c6-0b53627d0630.mock.pstmn.io/passwordManager/login";
    
    const response = await fetch(prodUrl, options);
    const body = await response.json();
    const isSuccess = response.ok;

    if (isSuccess) {
        const token = body["accessToken"];
        localStorage.setItem("accessToken", token);
        accessTokenSubject.next(token);
        isLoggedInSubject.next(isSuccess);
    }

    return isSuccess;

    // return await fetch(mockUrl, options)
    //     .then(response => {
    //         if (response.ok) {
                    // response.json() should be async to get proper body
    //             const body: any = response.json();
    //             const token: string = body["accessToken"];
            
    //             localStorage.setItem("accessToken", token);
    //             accessTokenSubject.next(token);
    //         }
    //         return response.ok;
    //     });
}

async function register(request: RegisterRequest) : Promise<RegisterResponse> {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify(request)
    };

    const response = await fetch("https://localhost:7265/api/User/Register", options);
    const body = await response.json();

    return new RegisterResponse(response.status, response.ok, body["message"]);
}

function logout() {
    localStorage.removeItem("accessToken");
    accessTokenSubject.next(null);
    isLoggedInSubject.next(false);

    console.log(localStorage.getItem("accessToken"));
}