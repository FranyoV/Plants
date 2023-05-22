import { ItemType } from "./ItemType";
import { User } from "./User";

export interface Item {

        id : string,
        name : string,
        type: ItemType,
        price: number,
        description : string | null,
        imageUrl : string | null,
        date: Date | null,
        sold: boolean,
        //User info
        userId: string
        user : User | null,        
}