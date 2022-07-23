export class AddPasswordRequest {
    application: string;
    value: string;

    constructor(application: string,
        value: string) {
            this.application = application;
            this.value = value;
        }
};