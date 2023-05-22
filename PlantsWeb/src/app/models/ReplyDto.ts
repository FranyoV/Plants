export class ReplyDto {
    constructor(
        public id : string,
        public content : string,
        public dateOfCreation : Date,
        public username : string,
        public postId : string,
        public userId : string,
    ){}

}