export class PlantDto {

    constructor(
        public id : string,
        public name : string,
        public description : string | null,
        public imageUrl: string | null | File,
        public note : string | null,
        public interval : number | null,
        public lastNotification: Date | null,
        public nextNotification: Date | null,
        public userId: string)
        {}
}