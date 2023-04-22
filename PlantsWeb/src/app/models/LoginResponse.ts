import { LoginStatus } from "./LoginStatus";

export class LoginResponse
{
    public status!: LoginStatus;
    public userId!: number;
    public token!: string;
}