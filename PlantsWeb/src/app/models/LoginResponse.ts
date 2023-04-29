import { LoginStatus } from "./LoginStatus";

export class LoginResponse
{
    public status!: LoginStatus;
    public userId!: string;
    public token!: string;
}