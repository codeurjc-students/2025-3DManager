import type { Page } from "@playwright/test";

export async function loginIfCI(page: Page) {
    const isCI = process.env.ASPNETCORE_ENVIRONMENT === 'CI';

    if (!isCI) {
        await page.click('button:text("Acceder como invitado")');
        return;
    }

    await page.fill('#userLogin', 'ci_user');
    await page.fill('#userPass', 'ci_password');
    await page.click('button[type="submit"]');
    await page.waitForURL('/dashboard');
}
