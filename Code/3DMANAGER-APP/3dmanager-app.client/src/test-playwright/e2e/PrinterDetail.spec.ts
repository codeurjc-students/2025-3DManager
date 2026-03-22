import { test, expect } from '@playwright/test';

test.describe('PrinterDetailPage E2E', () => {

    test('Render correctly the page', async ({ page }) => {
        await page.goto('/dashboard/printer/detail/1');

        await expect(page.locator('h2')).toHaveText('Detalle de impresora');
        await expect(page.locator('input.input-value-3').nth(0)).toBeVisible(); 
        await expect(page.locator('input.input-value-4').nth(0)).toBeVisible(); 
        await expect(page.locator('textarea')).toBeVisible(); 
        await expect(page.locator('img.image-container')).toBeVisible();
        await expect(page.locator('#printerState')).toBeVisible();
        await expect(page.locator('h3')).toHaveText('Piezas impresas');
        await expect(page.locator('table')).toBeVisible({ timeout: 30000 });
    });

    test('Edtition is posible and work', async ({ page }) => {
        await page.goto('/dashboard/printer/detail/1');

        const nameInput = page.locator('input.input-value-3').nth(0);
        const modelInput = page.locator('input.input-value-4').nth(0);
        const descInput = page.locator('textarea');
        const stateSelect = page.locator('#printerState');
        const saveButton = page.locator('.button-yellow');
        await nameInput.fill('Impresora Playwright Test');
        await modelInput.fill('Modelo Playwright');
        await descInput.fill('Descripción generada por test E2E');
        await stateSelect.selectOption({ index: 1 });
        await saveButton.click();
        await expect(page.locator('.popup-container')).toContainText('La impresora ha sido guardado correctamente');
    });
});
