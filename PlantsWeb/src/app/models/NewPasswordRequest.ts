export class NewPassWordRequest
{
    constructor(
        public newPassword: string, 
        public currentPassword: string)
    {}
}