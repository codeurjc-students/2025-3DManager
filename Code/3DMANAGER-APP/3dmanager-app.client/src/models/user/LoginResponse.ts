import type { UserObject } from "./UserObject";

export type LoginResponse = {
    token: string;
    user: UserObject;
};



