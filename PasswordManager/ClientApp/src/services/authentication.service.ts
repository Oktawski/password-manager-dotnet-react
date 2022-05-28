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

function login(username: string, password: string) : Promise<number> {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify({ username, password })
    };

    return fetch("https://localhost:7265/api/User/Authenticate", options)
        .then(response => {
            if (response.ok) {
                const body: any = response.json();
                const token: string = body["accessToken"];

                localStorage.setItem("accessToken", token);
                accessTokenSubject.next(token);
            }
            return response.status;
        });
}

function register(request: RegisterRequest) : Promise<any> {
    const options = {
        method: "POST",
        headers: { 
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        body: JSON.stringify(request)
    };


    // return fetch("https://localhost:7265/api/User/Register", options)
    //     .then(response => {
    //         return response.status;
    //     });


    return fetch("https://localhost:7265/api/User/Register", options)
        .then(response => response.json())
        .then(body => {
            return {
                message: body["message"],
                code: 200
            }
        });
}

function logout() {
    localStorage.removeItem("accessToken");
    accessTokenSubject.next(null);
}