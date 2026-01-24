export interface UserObject {
    userId: number;
    userName: string;
    userEmail: string;
    userPassword: string;
    rolId?: string;
    groupId?: number;
    groupName?: string;
}