export class Maintenance{
    id!: string;
    title!: string;
    note!: string;  
    interval! : number;
    lastNotification! : Date;
    nextNotification! : Date;

}