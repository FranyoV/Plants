export class UserInfoEditRequest
{
    constructor(
        public userId: string, 
        public userInfo: string,
        public password: string,
        )
    {
    }
}