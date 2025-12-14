import { test, expect } from '@playwright/test';

test.describe('DashboardActionsE2E', () => {


    test('DashboardManager', async ({ page }) => {
        
        await page.goto('/');
        await page.fill('#userLogin', 'ivicenter');
        await page.fill('#userPass', '12345');
        await page.click('button[type="submit"]');
        await page.waitForURL('/dashboard');

        await expect(page.locator('text=Subir archivo G-Code')).toBeVisible();
        await expect(page.locator('text=Añadir')).toBeVisible();
    });

    test('Dashboard actions for Guest user', async ({ page }) => {
        await page.goto('/');

        await page.click('button:text("Acceder como invitado")');

        await page.waitForURL('/dashboard');

        const uploadButton = page.locator('text=Subir archivo G-Code');
        await expect(uploadButton).toBeVisible();
        await expect(uploadButton).toBeDisabled();

        await expect(page.locator('text=Añadir')).not.toBeVisible();
    });
});
