import { test, expect } from '@playwright/test';

test.describe('DashboardActionsE2E', () => {

    test('renders printers and dashboard action buttons', async ({ page }) => {

        await page.goto('/dashboard');
        await page.waitForURL('/dashboard', { timeout: 10000 });
        expect(page.url()).toContain('/dashboard');
        await page.waitForSelector('#dashboard');

        await expect(page.locator('#dashboard')).toBeVisible();

        await expect(page.getByRole('heading', { name: 'Impresoras' })).toBeVisible();

        const printerCards = page.locator('button.printer-card');
        await expect(printerCards.first()).toBeVisible({ timeout: 30000 });
        const count = await printerCards.count();

        expect(count).toBeGreaterThanOrEqual(1);

        await expect(page.getByText('Filamentos', { exact: true })).toBeVisible();

        await expect(page.getByText('Usuarios', { exact: true })).toBeVisible();

        await expect(page.getByText('Piezas', { exact: true })).toBeVisible();

        const gcodeButton = page.getByRole('button', {
            name: /Subir archivo G-Code/i
        });

        await expect(gcodeButton).toBeVisible();
    });

});
