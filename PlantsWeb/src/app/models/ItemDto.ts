import { SafeUrl } from "@angular/platform-browser";
import { ItemType } from "./ItemType";

export interface ItemDto {
        id : string,
        name : string,
        type: ItemType,
        price: number,
        description : string | null,
        imageData : string | null | SafeUrl,
        date: Date | null,
        sold: boolean,
        username: string,
        email: string,
        userId: string    
}