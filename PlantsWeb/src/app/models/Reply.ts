export class Reply {
    constructor(
        public id : string,
        public content : string,
        public dateOfCreation : Date,
        
        public postId : string,
        public userId : string,
    ){}

}