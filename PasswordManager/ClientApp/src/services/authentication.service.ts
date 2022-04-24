import { BehaviorSubject } from "rxjs";
import { RegisterRequest } from "../requests/register.request";

const accessTokenSubject = new BehaviorSubject(localStorage.getItem("accessToken"));

export const authenticationService = {
    login,
    register,
    logout,
    accessToken: accessTokenSubject.asObservable(),
    get accessTokenValue() { return accessTokenSubject.value }
};

function login(username: string, password: string) {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify({ username, password })
    };

    return fetch("https://localhost:7265/api/User/Authenticate", options)
        .then(response => response.json())
        .then(body => {
            const token = body["accessToken"];
            localStorage.setItem("accessToken", token);
            accessTokenSubject.next(token);
        });
}

function register(request: RegisterRequest) {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify(request)
    };

    // return fetch("https://localhost:7265/api/User/Register", options)
    //     .then(response => response.json())
    //     .then(body => {
    //         return body["message"];
    //     });

    alert(JSON.stringify(request));
}

function logout() {
    localStorage.removeItem("accessToken");
    accessTokenSubject.next(null);
}