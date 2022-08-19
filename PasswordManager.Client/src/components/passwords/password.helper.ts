import { Password } from "../../model/Password";

export const passwordHelper = {
    mapPasswords,
    hideValue
}

export type MappedPassword = {
    id: string,
    application: string,
    login: string,
    hiddenValue: string,
    actualValue: string,
    currentValue: string,
    isHidden: boolean
}

function mapPasswords(passwords: Array<Password>): Array<MappedPassword> {
    return passwords.map<MappedPassword>(e => { 
        let asteriskedValue = hideValue(e.value);

        return { 
            id: e.id,
            application: e.applicationNormalized, 
            login: e.login,
            hiddenValue: asteriskedValue,
            actualValue: e.value,
            currentValue: asteriskedValue,
            isHidden: true 
        }
    });
}

function hideValue(value: string) {
     return "*".repeat(value.length); 
}