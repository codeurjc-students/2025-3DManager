import { chromium } from '@playwright/test';
import { loginByApi } from './e2e/AuthHelperTest';

const isCI = !!process.env.CI;
const port = isCI ? 3001 : 3000;
const protocol = isCI ? 'http' : 'https';
const BASE_URL = `${protocol}://localhost:${port}`;

export default async function globalSetup() {
    const { user, token } = await loginByApi();

    const browser = await chromium.launch();
    const context = await browser.newContext({
        baseURL: BASE_URL,
        ignoreHTTPSErrors: true,
    });
    const page = await context.newPage();
    await page.goto('/login');

    await page.evaluate(({ user, token }) => {
        localStorage.setItem('user', JSON.stringify(user));
        localStorage.setItem('token', token);
    }, { user, token });

    await page.context().storageState({
        path: 'playwright/.auth/state.json'
    });

    await browser.close();
}
