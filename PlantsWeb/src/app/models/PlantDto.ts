import { SafeUrl } from "@angular/platform-browser";

export class PlantDto {

    constructor(
        public id : string,
        public name : string,
        public description : string | null,
        public imageData: string | null | SafeUrl,
        public note : string | null ,
        public interval : number | null,
        public lastNotification: Date | null,
        public nextNotification: Date | null,
        public userId: string)
        {}
}