export class RegisterRequest {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;


    constructor(username: string,
                email: string,
                password: string,
                confirmPassword:string) {
        this.username = username;
        this.email = email;
        this.password = password;
        this.confirmPassword = confirmPassword;
    }
}