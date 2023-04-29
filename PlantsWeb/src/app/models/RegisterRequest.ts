export class RegisterRequest
{
    constructor(
        public email: string,
        public userName: string, 
        public password: string,
        )
    {
    }
}