import { Guid } from "guid-typescript";
import { Maintenance } from "./Maintenance";

export class PlantDto {

    constructor(
        public id : string,
        public name : string,
        public description : string | null,
        //public maintenance: Maintenance,
        public imageUrl: string | null,
        public note : string | null,
        public interval : number | null,
        public lastNotification: Date | null,
        public nextNotification: Date | null,
        public userId: string){
    }
}