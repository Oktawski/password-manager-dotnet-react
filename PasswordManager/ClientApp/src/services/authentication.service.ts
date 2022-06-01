import { BehaviorSubject } from "rxjs";
import { RegisterRequest } from "../requests/register.request";
import { RegisterResponse } from "../responses/register.response";

const accessTokenSubject = new BehaviorSubject(localStorage.getItem("accessToken"));

export const authenticationService = {
    login,
    register,
    logout,
    accessToken: accessTokenSubject.asObservable(),
    get accessTokenValue(): string|null { return accessTokenSubject.value },
    get isLoggedIn(): boolean { return accessTokenSubject.value !== null }
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

    const response = await fetch("https://localhost:7265/api/User/Authenticate", options);
    const body = await response.json();

    if (response.ok) {
        const token = body["accessToken"];
        localStorage.setItem("accessToken", token);
        accessTokenSubject.next(token);
    }

    return response.ok 
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
}