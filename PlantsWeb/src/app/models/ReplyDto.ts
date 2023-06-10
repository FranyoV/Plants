import { SafeUrl } from "@angular/platform-browser";

export class ReplyDto {
    constructor(
        public id : string,
        public content : string,
        public dateOfCreation : Date,
        public username : string,
        public profileImage : SafeUrl | string | null,
        public postId : string,
        public userId : string,
    ){}

}