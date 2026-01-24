import { test, expect } from '@playwright/test'

test('listar impresoras correctamente', async ({ page }) => {

    await page.route('**/api/Printer/GetPrinterList', async route => {
        route.fulfill({
            status: 200,
            contentType: 'application/json',
            body: JSON.stringify({
                data: [
                    { printerName: 'Impresora Mock 1' },
                    { printerName: 'Impresora Mock 2' }
                ]
            })
        });
    });

    await page.goto('/');

    await expect(page.locator('h2')).toHaveText('Impresoras:');
    const count = await page.locator('li').count();
    expect(count).toBeGreaterThan(0);
});