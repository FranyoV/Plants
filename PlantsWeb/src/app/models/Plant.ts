import { Guid } from "guid-typescript";
import { Maintenance } from "./Maintenance";

export class Plant {

    constructor(
        public id : Guid,
        public name : string,
        public description : string,
        //public maintenance: Maintenance,
        public imageUrl: string | null,
        public userId: Guid){
    }
}