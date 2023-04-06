import { Guid } from "guid-typescript";
import { User } from "./User";

export class Post {

    constructor(
        public id : Guid,
        public title : string,
        public content : string ,
        public imageUrl : string | null,
        public dateOfCreation : Date,
        public userId : Guid,
        public user : User | null | undefined
        ){}
}