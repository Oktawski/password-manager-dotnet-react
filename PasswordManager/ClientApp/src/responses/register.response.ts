export class RegisterResponse {
    status: number;
    ok: boolean;
    message: string;

    constructor (status: number,
        ok: boolean,
        message: string) {
            this.status = status;
            this.ok = ok;
            this.message = message;
    }
}