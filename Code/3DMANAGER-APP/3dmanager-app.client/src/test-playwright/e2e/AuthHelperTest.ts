import { request, type APIRequestContext, type Page } from "@playwright/test";

const API_URL = process.env.CI
    ? 'http://localhost:5000'
    : 'https://localhost:443';

export async function loginByApi() {
    const context = await request.newContext({
        ignoreHTTPSErrors: true,
    });
    
    const response = process.env.CI ? await context.post(
        `${API_URL}/api/v1/users/Login`,
        {
            data: {
                userName: 'user_test',
                userPassword: 'password123'
            }
        }
    ) : await context.post(`${API_URL}/api/v1/users/LoginGuest`);

    if (!response.ok()) {
        throw new Error('Login por API falló');
    }

    const body = await response.json();

    return {
        user: body.data.user,
        token: body.data.token
    };
}
