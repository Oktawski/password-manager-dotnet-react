export class AddPasswordRequest {
    application: string;
    login: string;
    value: string;

    constructor(application: string,
        login: string,
        value: string,
    ) {
        this.application = application;
        this.login = login;
        this.value = value;
    }
};