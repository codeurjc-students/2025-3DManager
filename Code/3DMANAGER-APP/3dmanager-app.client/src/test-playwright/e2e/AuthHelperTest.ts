import type { Page } from "@playwright/test";

export async function loginIfCI(page: Page) {
    const isCI = process.env.VITE_CI === 'true';

    if (!isCI) {
        await page.click('button:text("Acceder como invitado")');
        return;
    }
    
    await page.fill('#userLogin', 'user_test');
    await page.fill('#userPass', 'password123');
    await page.click('button[type="submit"]');
    await page.waitForURL('/dashboard');
}
