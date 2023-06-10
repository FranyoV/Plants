import { SafeUrl } from "@angular/platform-browser";

export class UserDto {
    id! : string;
    name! : string;
    emailAddress! : string;
    imageData! : SafeUrl;
}