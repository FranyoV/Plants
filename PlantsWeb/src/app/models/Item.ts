import { ItemType } from "./ItemType";
import { User } from "./User";

export interface Item {

        id : string,
        name : string,
        description : string | null,
       // imageUrl : string | null,
        type: ItemType,
        date: Date | null,
        price: number,
         
         //User info
        user : User | null,
        userId: string
         
}