import { Guid } from "guid-typescript";
import { Maintenance } from "./Maintenance";
import { User } from "./User";

export class Plant {

    constructor(
        //Plant data
        public id : string,
        public name : string,
        public description : string | null,
        public imageUrl : string | null,

        //Maintenance data
        public note : string | null,
        public interval : number | null,
        public lastNotification: Date | null,
        public nextNotification: Date | null,
        
        //User info
        public user : User | null,
        public userId: string){
    }
}