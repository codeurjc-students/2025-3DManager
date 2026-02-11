export interface UserObject {
    userId: number;
    userName: string;
    userEmail: string;
    userPassword: string;
    rolId: string | null;
    groupId: number | null;
    groupName?: string | null;
}
