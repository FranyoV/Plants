import { SafeUrl } from "@angular/platform-browser";

export class Post {

    constructor(
        public id : string,
        public title : string,
        public content : string ,
        public imageData : string | SafeUrl | null,
        public dateOfCreation : Date,
        public userId : string,
        public userName : string | null | undefined,
        public profileImage : SafeUrl | string | null,
        public replyCount : number | null | undefined
        ){}
}