import { BehaviorSubject } from "rxjs";

const accessTokenSubject = new BehaviorSubject(localStorage.getItem("accessToken"));

export const authenticationService = {
    login,
    logout,
    accessToken: accessTokenSubject.asObservable(),
    get accessTokenValue() { return accessTokenSubject.value }
};

function login(username, password) {
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

function logout() {
    localStorage.removeItem("accessToken");
    accessTokenSubject.next(null);
}