import type { UserListResponse } from "../user/UserListResponse";

export interface GroupBasicDataResponse{
    groupName: string;
    groupDescription: string;
    groupDate: Date;
    groupOwner: string;
    groupMembers: UserListResponse[];

}