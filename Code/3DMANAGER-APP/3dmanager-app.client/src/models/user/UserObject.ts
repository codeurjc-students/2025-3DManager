export interface UserObject {
    userId: number;
    userName: string;
    userEmail: string;
    userPassword: string;
    role?: string;
    groupId?: number;
}