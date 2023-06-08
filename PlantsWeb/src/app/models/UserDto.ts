import { SafeUrl } from "@angular/platform-browser";

export class UserDto {
    id! : string;
    name! : string;
    email! : string;
    image! : SafeUrl;
}