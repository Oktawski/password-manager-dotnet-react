export const authenticationService = {
    login,
    logout
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
        .then(body => console.log(body["accessToken"]));

}

function logout() {


}