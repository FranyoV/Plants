import { Maintenance } from "./Maintenance";

export class Plant {

    constructor(
        public id : string,
        public name : string,
        public description : string,
        public maintenance: Maintenance,
        public imageUrl: string | undefined,
        public userId: string){

    }

}