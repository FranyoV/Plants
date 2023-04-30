export class Post {

    constructor(
        public id : string,
        public title : string,
        public content : string ,
        public imageUrl : string | null,
        public dateOfCreation : Date,
        public userId : string,
        public userName : string | null | undefined
        ){}
}