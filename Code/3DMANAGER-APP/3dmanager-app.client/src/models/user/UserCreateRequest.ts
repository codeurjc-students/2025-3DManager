export interface UserCreateRequest {
    userName: string;
    userEmail: string;
    userPassword: string;
    imageFile?: File | null;
    
}